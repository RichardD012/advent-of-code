using DotNetEnv;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace AdventCode;

class Program
{
#if DEBUG

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
            _ = AdventUtils.GetCurrentYear();
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
        var dayValue = AdventUtils.GetDay();
        var type = asm.GetType($"AdventCode.Tasks{AdventUtils.GetCurrentYear()}.Day{dayValue}Task");
        if (type == null)
        {
            throw new DayNotCreatedException(dayValue);
        }
        return type;
    }



    private static async Task ExceuteTask(IAdventCodeTask task)
    {
        if (task.TaskDay == 0)
        {
            throw new NotImplementedException("Provided Day was not implemented since the task has a day of 0");
        }
#if DEBUG
        logger?.LogInformation("{CurrentYear} Day {TaskDay}", AdventUtils.GetCurrentYear(), task.TaskDay);
#else 
        Console.WriteLine("{0} Day {1}", AdventUtils.GetCurrentYear(), task.TaskDay);
#endif
        try
        {
            try
            {
                var task1Answer = await task.GetFirstTaskAnswerAsync();
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
            catch (InvalidAnswerException)
            {
#if DEBUG
                logger?.LogInformation(" The first task had an invalid computation");
#else
                Console.WriteLine(" The first task had an invalid computation");
#endif
            }
            try
            {
                var task2Answer = await task.GetSecondTaskAnswerAsync();
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
            catch (InvalidAnswerException)
            {
#if DEBUG
                logger?.LogInformation(" The second task had an invalid computation");
#else
                Console.WriteLine(" The second task had an invalid computation");
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
        services.AddSingleton(new AdventConfig { SessionCookie = adventCookie });
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

