using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MTLServiceBot.Bot.Commands
{
    public class Login : ICommand
    {
        public async Task<bool> Handle(ITelegramBotClient botClient, Message message, Users.User userSession)
        {
            throw new NotImplementedException();
        }
    }
}
