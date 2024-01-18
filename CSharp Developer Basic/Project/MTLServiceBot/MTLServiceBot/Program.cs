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
                .WriteTo.File(new JsonFormatter(),
                    "important logs.json",
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
                .WriteTo.File("all.logs",
                    rollingInterval: RollingInterval.Month)
                .MinimumLevel.Debug()
                .CreateLogger();

            _bot = new TgBot();
            _bot.RunBot();
            Console.ReadKey();
        }
    }
}