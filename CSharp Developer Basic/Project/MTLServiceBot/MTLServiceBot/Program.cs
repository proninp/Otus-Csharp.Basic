using MTLServiceBot.API;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MTLServiceBot
{
    internal class Program
    {
        private static ITelegramBotClient _bot;

        static void Main(string[] args)
        {
            

            var api = new ServiceAPI();

            var userTest = new User(0, 0, Environment.GetEnvironmentVariable("API_TEST_LOGIN"));
            userTest.SetAuthPasswordTest(Environment.GetEnvironmentVariable("API_TEST_PASSWORD"));

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