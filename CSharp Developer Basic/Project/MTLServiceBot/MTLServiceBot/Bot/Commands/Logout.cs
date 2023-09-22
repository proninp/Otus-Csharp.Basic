using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MTLServiceBot.Bot.Commands
{
    public class Logout : Command
    {
        public Logout(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication)
        {
        }

        public override async Task Handle(ITelegramBotClient botClient, Message message, Session session)
        {
            if (!session.IsAuthorized)
            {
                await botClient.SendTextMessageAsync(message.Chat, "Вы не авторизованы в системе, выполнение команды невозможно", null, ParseMode.Markdown);
                return;
            }
            session.Logout();
            await botClient.SendTextMessageAsync(message.Chat, $"{session.User.Name}, ваша сессия успешно завершена", null, ParseMode.Markdown);
        }
    }
}
