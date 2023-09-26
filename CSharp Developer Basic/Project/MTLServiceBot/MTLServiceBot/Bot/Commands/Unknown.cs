using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MTLServiceBot.Bot.Commands
{
    public class Unknown : Command
    {
        public Unknown(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication)
        {
        }

        public override async Task Handle(ITelegramBotClient botClient, Message message, Session userSession)
        {
            await botClient.SendTextMessageAsync(message.Chat,
                "Извините, команда не распознана.\nДля получения списка команд введите /help",
                replyMarkup: new ReplyKeyboardRemove());
        }
    }
}
