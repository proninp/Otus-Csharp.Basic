using Telegram.Bot;
using Telegram.Bot.Types;

namespace MTLServiceBot.Bot.Commands
{
    public class Unknown : ICommand
    {
        public async Task<bool> Handle(ITelegramBotClient botClient, Message message, Users.User userSession)
        {
            await botClient.SendTextMessageAsync(message.Chat, $"{userSession.Name}, извините, команда не распознана.\nДля получения списка команд введите /help");
            return true;
        }
    }
}
