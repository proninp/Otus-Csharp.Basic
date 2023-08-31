using Telegram.Bot;
using Telegram.Bot.Types;

namespace MTLServiceBot.Bot.Commands
{
    public class RequestsList : ICommand
    {
        public bool IsRequireAuthentication { get; } = true;
        public Task<bool> Handle(ITelegramBotClient botClient, Message message, Users.User userSession)
        {
            throw new NotImplementedException();
        }
    }
}
