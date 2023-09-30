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

        public override async Task Handle(ITelegramBotClient botClient, Message message, Session userSession)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
                string.Format(TextConsts.StopCommandMsg, userSession.User.Name),
                replyMarkup: new ReplyKeyboardRemove());
        }
    }
}
