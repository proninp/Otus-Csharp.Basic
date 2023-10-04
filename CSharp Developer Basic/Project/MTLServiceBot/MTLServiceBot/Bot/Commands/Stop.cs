using MTLServiceBot.Assistants;
using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MTLServiceBot.Bot.Commands
{
    public class Stop : Command
    {
        public Stop(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication)
        {
        }

        public override Task HandleAsync(ITelegramBotClient botClient, TgUpdate update, Session session)
        {
            _ = botClient.SendTextMessageAsync(update.Chat.Id,
                string.Format(TextConsts.StopCommandMsg, session.User.Name),
                replyMarkup: new ReplyKeyboardRemove());
            return Task.CompletedTask;
        }
    }
}
