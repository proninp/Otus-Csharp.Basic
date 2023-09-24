using MTLServiceBot.Bot;
using MTLServiceBot.SQL;
using MTLServiceBot.Users;
using Telegram.Bot;

namespace MTLServiceBot
{
    internal class Program
    {
        private static TgBot? _bot;

        static void Main(string[] args)
        {
            _bot = new TgBot();
            _bot.RunBot();
            Console.ReadKey();
        }

        public static void ColoredPrint(string text, ConsoleColor color = ConsoleColor.DarkCyan)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;
        }
    }
}