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

        public override async Task<bool> Handle(ITelegramBotClient botClient, Message message, Session userSession)
        {
            await botClient.SendTextMessageAsync(message.Chat, $"Привет, {userSession.User.Name}!");
            await botClient.SendTextMessageAsync(message.Chat, "Введите /login для авторизации.");
            return true;
        }
    }
}
