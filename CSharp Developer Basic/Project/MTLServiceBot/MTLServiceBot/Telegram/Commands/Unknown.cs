using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MTLServiceBot.Telegram.Commands
{
    public class Unknown : ICommand
    {
        public async Task<bool> Handle(ITelegramBotClient botClient, Message message, User user)
        {
            await botClient.SendTextMessageAsync(message.Chat, $"{user.UserName}, извините, команда не распознана.\nДля получения списка команд введите /help");
            return true;
        }
    }
}
