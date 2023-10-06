using MTLServiceBot.Assistants;
using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace MTLServiceBot.Bot.Commands
{
    public abstract class Command
    {
        private readonly string _name;
        private readonly string _description;
        private readonly bool _isRequireAuthentication;
        public string Name { get => _name; }
        public string Description { get => _description; }
        public bool IsRequireAuthentication { get => _isRequireAuthentication; }
        public Command(string name, string description, bool isRequireAuthentication)
        {
            _name = name;
            _description = description;
            _isRequireAuthentication = isRequireAuthentication;
        }

        public bool CheckAuthorization(ITelegramBotClient botClient, TgUpdate messageData, Session session)
        {
            if (!IsRequireAuthentication || session.IsAuthorized)
                return true;

            var unauthMessage = string.Format(TextConsts.AuthorizationRequired, Name);
            _ = botClient.SendTextMessageAsync(messageData.Chat!, unauthMessage, null, ParseMode.Markdown);
            return false;
        }

        public virtual async Task HandleAsync(ITelegramBotClient botClient, TgUpdate messageData, Session session)
        {
            await botClient.SendTextMessageAsync(messageData.Chat!, TextConsts.UnknownCommand, null, ParseMode.Markdown);
        }
    }
}
