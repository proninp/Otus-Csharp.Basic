using Telegram.Bot;

namespace MTLServiceBot.Telegram.Bot
{
    public class TgBotClient
    {
        public readonly ITelegramBotClient Client;
        public TgBotClient()
        {
            Client = new TelegramBotClient(AppConfig.BotToken ??
                throw new InvalidOperationException("Необходимо установить токен для бот-клиента")
            );
        }
    }
}
