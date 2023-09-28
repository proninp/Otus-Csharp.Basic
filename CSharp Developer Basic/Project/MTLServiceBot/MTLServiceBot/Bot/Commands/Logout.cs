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

        public override async Task Handle(ITelegramBotClient botClient, Message message, Session session)
        {
            if (!session.IsAuthorized)
            {
                await botClient.SendTextMessageAsync(message.Chat,
                    TextConsts.LogoutUnathorizedMsg,
                    parseMode: ParseMode.Markdown);
                return;
            }
            session.EndSession();
            await botClient.SendTextMessageAsync(message.Chat,
                string.Format(TextConsts.LogoutSuccessMsg, session.User.Name), 
                parseMode: ParseMode.Markdown,
                replyMarkup: new ReplyKeyboardRemove());
        }
    }
}
