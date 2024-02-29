using MTLServiceBot.SQL;
using Telegram.Bot;

namespace MTLServiceBot.Bot
{
    public class TgBotClient
    {
        private readonly Telegram.Bot.Types.User _tgUser;
        public Telegram.Bot.Types.User TgUser { get => _tgUser; }
        public readonly ITelegramBotClient _client;
        
        public TgBotClient()
        {
            _client = new TelegramBotClient(ConfigRepository.GetBotToken());
            _tgUser = _client.GetMeAsync().Result;
        }
    }
}
