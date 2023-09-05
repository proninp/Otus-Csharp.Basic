using MTLServiceBot.SQL;
using MTLServiceBot.Users;
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
            
            //Session session = new Session(100, 120, "pps", DateTime.Now);
            //session.Save();
            //var sessions = SessionRepository.GetActiveSessions();
            //sessions.ForEach(s => Console.WriteLine(s.ToString()));
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