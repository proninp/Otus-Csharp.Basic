using MTLServiceBot.Bot;

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
    }
}