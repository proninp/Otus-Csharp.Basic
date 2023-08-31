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
            Client = new TelegramBotClient(AppConfig.BotToken ??
                throw new InvalidOperationException("Необходимо установить токен для бот-клиента")
            );
            _tgUser = Client.GetMeAsync().Result;
        }
    }
}
