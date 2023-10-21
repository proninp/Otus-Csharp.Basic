using MTLServiceBot.Users;
using Telegram.Bot;

namespace MTLServiceBot.Bot.Commands
{
    public class Otpgen : Command
    {
        public Otpgen(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication)
        {
        }

        public override async Task HandleAsync(ITelegramBotClient botClient, TgUpdate update, Session session)
        {

        }
    }
}
