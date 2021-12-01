using System;
using System.Linq;
using System.Threading.Tasks;
using DotNetEnv;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using AdventCode.Tasks;
using AdventCode.Utils;
using System.Net.Http;
using System.Net;

namespace AdventCode;

class Program
{
    private static Logger logger = LogManager.GetCurrentClassLogger();
    static async Task Main(string[] args)
    {
        try
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(System.IO.Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();

            var servicesProvider = RegisterServices(config);
            using (servicesProvider as IDisposable)
            {
                //TODO: Put the class here that you want to execute.
                if (servicesProvider.GetRequiredService<Day1Task>() is IAdventCodeTask task)
                {
                    await ExceuteTask(task);
                }
            }
        }
        catch (Exception ex)
        {
            // NLog: catch any exception and log it.
            logger.Error(ex, $"Exception executing advent calendar [{ex.Message}]");
            //throw;
        }
        finally
        {
            // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
            LogManager.Shutdown();
        }
    }

    private static async Task ExceuteTask(IAdventCodeTask task)
    {
        logger.Info("Day {TaskDay}", task.TaskDay);
        try
        {
            try
            {
                var task1Answer = await task.GetFirstTaskAnswer();
                logger.Info(" Task 1: {Task1Answer}", task1Answer);
            }
            catch (TaskIncompleteException)
            {
                logger.Info(" The first task has not been completed yet");
            }
            try
            {
                var task2Answer = await task.GetSecondTaskAnswer();
                logger.Info(" Task 2: {Task2Answer}", task2Answer);
            }
            catch (TaskIncompleteException)
            {
                logger.Info(" The second task has not been completed yet");
            }
        }
        catch (NoDataException)
        {
            logger.Error(" No data is available for {TaskDay}", task.TaskDay);
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
        services.AddLogging(loggingBuilder =>
                {
                    // configure Logging with NLogd
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    loggingBuilder.AddNLog(config);
                });
        return services.BuildServiceProvider(true);
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

