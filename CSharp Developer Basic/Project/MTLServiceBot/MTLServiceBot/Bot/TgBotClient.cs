using MTLServiceBot.SQL;
using Telegram.Bot;

namespace MTLServiceBot.Bot
{
    public class TgBotClient
    {
        private readonly Telegram.Bot.Types.User _tgUser;
        public Telegram.Bot.Types.User TgUser { get => _tgUser; }
        public readonly ITelegramBotClient Client;
        public TgBotClient()
        {
            Client = new TelegramBotClient(ConfigRepository.GetBotToken());
            _tgUser = Client.GetMeAsync().Result;
        }
    }
}
