using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MTLServiceBot.Bot;
using Serilog;
using Serilog.Formatting.Json;

namespace MTLServiceBot
{
    internal class Program
    {
        private static TgBot? _bot;

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(new JsonFormatter(), AppConfig.ImportantLogsFile, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
                .WriteTo.File(AppConfig.RegularLogsFile, rollingInterval: RollingInterval.Month)
                .MinimumLevel.Debug()
                .CreateLogger();

            using var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<TgBotClient>();
                    services.AddSingleton<TgBot>();
                    services.AddSingleton<TgErrorHandler>();
                    services.AddSingleton<TgUpdateHandler>();
                    services.AddSingleton<TgUpdate>();
                })
                .Build();

            _bot = host.Services.GetService<TgBot>();
            _bot!.RunBot();
            Console.ReadKey();
        }
    }
}