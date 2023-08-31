using MTLServiceBot.SQL;
using Telegram.Bot;

namespace MTLServiceBot
{
    internal class Program
    {
        private static ITelegramBotClient _bot;

        static void Main(string[] args)
        {
            //TgBot bot = new TgBot();
            //bot.RunBot();
            var usersCount = UserRepository.GetUsersQty();
            Console.WriteLine(usersCount);

            var users = UserRepository.GetUsers();
            foreach (var user in users)
                Console.WriteLine(user.ToString());
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