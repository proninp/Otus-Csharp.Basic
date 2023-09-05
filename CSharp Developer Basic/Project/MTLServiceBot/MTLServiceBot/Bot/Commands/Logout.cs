using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MTLServiceBot.Bot.Commands
{
    public class Logout : Command
    {
        public Logout(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication)
        {
        }

        public override Task<bool> Handle(ITelegramBotClient botClient, Message message, Session userSession)
        {
            throw new NotImplementedException();
        }
    }
}
