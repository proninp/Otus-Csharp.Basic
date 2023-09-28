using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MTLServiceBot.Bot.Commands
{
    public abstract class Command
    {
        private readonly string _name;
        private readonly string _description;
        private readonly bool _isRequireAuthentication;
        public bool WorkflowMode { get; set; } // режим работы с одной командой, переключается только в случае, если пришла другая команда
        public string Name { get => _name; }
        public string Description { get => _description; }
        public bool IsRequireAuthentication { get => _isRequireAuthentication; }
        public Command(string name, string description, bool isRequireAuthentication)
        {
            _name = name;
            _description = description;
            _isRequireAuthentication = isRequireAuthentication;
        }
        public async Task<bool> CheckAuthentication(ITelegramBotClient botClient, Message message, Session session)
        {
            if (!IsRequireAuthentication || session.IsAuthorized)
                return true;

            var unauthMessage = $"Для выполнения команды {Name} требуется авторизация.";
            await botClient.SendTextMessageAsync(message.Chat, unauthMessage, null, ParseMode.Markdown);

            return false;
        }

        public virtual async Task Handle(ITelegramBotClient botClient, Message message, Session session)
        {
            await botClient.SendTextMessageAsync(message.Chat, $"Я пока что не умею обрабатывать данную команду", null, ParseMode.Markdown);
        }
    }
}
