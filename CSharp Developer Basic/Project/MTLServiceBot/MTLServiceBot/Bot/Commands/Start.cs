using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MTLServiceBot.Bot.Commands
{
    public class Start : Command
    {
        public Start(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication)
        {
        }

        public override async Task HandleAsync(ITelegramBotClient botClient, TgUpdate update, Session session)
        {
            await botClient.SendTextMessageAsync(update.Chat, $"Привет, {session.User.Name}!");
        }
    }
}
