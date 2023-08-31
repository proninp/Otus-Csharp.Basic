using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MTLServiceBot.Bot.Commands
{
    public interface ICommand
    {
        public Task<bool> Handle(ITelegramBotClient botClient, Message message, Users.User userSession);
    }
}
