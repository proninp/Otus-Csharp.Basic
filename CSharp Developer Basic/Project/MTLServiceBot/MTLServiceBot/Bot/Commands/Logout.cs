using MTLServiceBot.Assistants;
using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MTLServiceBot.Bot.Commands
{
    public class Logout : Command
    {
        public Logout(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication)
        {
        }

        public override Task HandleAsync(ITelegramBotClient botClient, TgUpdate update, Session session)
        {
            if (!session.IsAuthorized)
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    AppConfig.Instance.LogoutUnathorized,
                    parseMode: ParseMode.Markdown);
            }
            else
            {
                session.EndSession();
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(AppConfig.Instance.LogoutSuccess, session.User.Name),
                    parseMode: ParseMode.Markdown,
                    replyMarkup: new ReplyKeyboardRemove());
            }
            return Task.CompletedTask;
        }
    }
}
