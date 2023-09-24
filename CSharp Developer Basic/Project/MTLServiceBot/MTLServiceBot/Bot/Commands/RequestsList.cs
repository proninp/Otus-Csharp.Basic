using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MTLServiceBot.Bot.Commands
{
    public class RequestsList : Command
    {
        public RequestsList(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication)
        {
        }

        public override async Task Handle(ITelegramBotClient botClient, Message message, Session session)
        {
                        
        }
    }
}
