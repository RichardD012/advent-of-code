using System;
using System.Linq;
using System.Threading.Tasks;
using DotNetEnv;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AdventCode.Tasks;
using AdventCode.Utils;
using System.Net.Http;
using System.Net;
using Microsoft.Extensions.Logging.Configuration;

namespace AdventCode;

class Program
{
#if DEBUG
    //manual override for testing upcoming days when you may not have the automatic day detection in effect
    private const int DayOverride = -1;
    private static ILogger<Program>? logger;
#endif
    static async Task Main(string[] args)
    {
#if DEBUG
        ILoggerFactory? loggerFactory = null;
#endif
        try
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(System.IO.Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();
#if DEBUG
            loggerFactory = LoggerFactory.Create(builder => GenerateBuilder(builder, config));
            logger = loggerFactory.CreateLogger<Program>();
#endif
            var servicesProvider = RegisterServices(config);
            using (servicesProvider as IDisposable)
            {
                var type = GetDayType();
                if (servicesProvider.GetRequiredService(type) is IAdventCodeTask task)
                {
                    await ExceuteTask(task);
                }
            }
        }
        catch (DayNotCreatedException dex)
        {
#if DEBUG
            logger?.LogInformation("Day {TaskDay} not created", dex.TaskDay);
#else
            Console.WriteLine("Day {0} not created", dex.TaskDay);
#endif
        }
        catch (Exception ex)
        {
#if DEBUG
            logger?.LogError(ex, "Unhandled Exception executing advent calendar [{ExceptionMessage}]", ex.Message);
#else
            Console.WriteLine("Unhandled Exception executing advent calendar [{0}]", ex.Message);
#endif
            Environment.ExitCode = -1;
        }
        finally
        {
#if DEBUG
            loggerFactory?.Dispose(); //flush any remaining data to the log
#endif
        }
    }

    private static Type GetDayType()
    {
        //TODO: Handle input
        var asm = typeof(Program).Assembly;
        var dayValue = GetDay();
        var type = asm.GetType($"AdventCode.Tasks.Day{dayValue}Task");
        if (type == null)
        {
            throw new DayNotCreatedException(dayValue);
        }
        return type;
    }

    private static int GetDay()
    {
#if DEBUG
#pragma warning disable CS0162
        if (DayOverride >= 1 && DayOverride <= 25)
            return DayOverride;
        var currentTime = DateTime.Now;
        var convertedTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "Eastern Standard Time");
        return convertedTime.Day;
#pragma warning restore CS0162
#else
        var correctDay = 0;
        var correctInput = false;
        do
        {
            Console.Write("Please Enter the Day: ");
            var day = Console.ReadLine();
            if ((int.TryParse(day, out var parsedDay) == false) || (parsedDay < 1 || parsedDay > 25))
            {
                Console.WriteLine("Please only enter a number 1-25");
            }
            else
            {
                correctInput = true;
                correctDay = parsedDay;
            }
        } while (correctInput == false);
        return correctDay;
#endif
    }

    private static async Task ExceuteTask(IAdventCodeTask task)
    {
#if DEBUG
        logger?.LogInformation("Day {TaskDay}", task.TaskDay);
#else 
        Console.WriteLine("Day {0}", task.TaskDay);
#endif
        try
        {
            try
            {
                var task1Answer = await task.GetFirstTaskAnswer();
#if DEBUG
                logger?.LogInformation(" Task 1: {Task1Answer}", task1Answer);
#else
                Console.WriteLine(" Task 1: {0}", task1Answer);
#endif
            }
            catch (TaskIncompleteException)
            {
#if DEBUG
                logger?.LogInformation(" The first task has not been completed yet");
#else
                Console.WriteLine(" The first task has not been completed yet");
#endif
            }
            try
            {
                var task2Answer = await task.GetSecondTaskAnswer();
#if DEBUG
                logger?.LogInformation(" Task 2: {Task2Answer}", task2Answer);
#else
                Console.WriteLine(" Task 2: {0}", task2Answer);
#endif
            }
            catch (TaskIncompleteException)
            {
#if DEBUG
                logger?.LogInformation(" The second task has not been completed yet");
#else
                Console.WriteLine(" The second task has not been completed yet");
#endif
            }
        }
        catch (NoDataException)
        {
#if DEBUG
            logger?.LogInformation(" No data is available for day {TaskDay}", task.TaskDay);
#else
            Console.WriteLine(" No data is available for day {0}", task.TaskDay);
#endif
        }
    }


    private static IServiceProvider RegisterServices(IConfiguration config)
    {
        Env.Load(".env");
        var adventCookie = ReadEnvironmentVariable("COOKIE");
        var services = new ServiceCollection();
        var dataTaskTypes = (typeof(Program).Assembly).GetTypes().Where(x => x.IsAbstract == false && typeof(IAdventCodeTask).IsAssignableFrom(x));
        foreach (var type in dataTaskTypes)
        {
            services.AddSingleton(type);
        }
        services.AddHttpClient<IAdventWebClient, AdventWebClient>(
                client =>
                {
                    client.BaseAddress = new Uri("https://adventofcode.com/");
                    if (client.DefaultRequestHeaders.Contains("cookie") == false)
                    {
                        client.DefaultRequestHeaders.Add("cookie", adventCookie);
                    }
                }
            ).ConfigurePrimaryHttpMessageHandler(sp => new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });
#if DEBUG
        services.AddLogging(builder => GenerateBuilder(builder, config));
#endif
        return services.BuildServiceProvider(true);
    }

    private static void GenerateBuilder(ILoggingBuilder builder, IConfiguration config)
    {
        builder.AddConfiguration(config.GetSection("Logging"));
        builder.AddSimpleConsole(options =>
        {
            options.IncludeScopes = false;
            options.SingleLine = true;
            options.TimestampFormat = "hh:mm:ss ";
        });
    }

    private static string ReadEnvironmentVariable(string variableName)
    {
        var value = Environment.GetEnvironmentVariable(variableName);
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }
        return value;
    }

}

