using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace MTLServiceBot.Bot.Commands
{
    public class Unknown : Command
    {
        public Unknown(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication)
        {
        }

        public override Task HandleAsync(ITelegramBotClient botClient, TgUpdate update, Session userSession)
        {
            _ = botClient.SendTextMessageAsync(update.Chat,
                AppConfig.Instance.UnknownCommandFullDescription,
                replyMarkup: new ReplyKeyboardRemove());
            return Task.CompletedTask;
        }
    }
}
