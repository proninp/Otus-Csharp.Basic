using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MTLServiceBot.Bot.Commands
{
    public class Login : Command
    {
        public Login(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication) { }

        public override async Task<bool> Handle(ITelegramBotClient botClient, Message message, Session userSession)
        {
            var isAlreadyAuthenticatedText = $"Вы уже авторизованы под логином {userSession.User.Login}.";
            if (userSession.IsAuthenticated)
            {
                await botClient.SendTextMessageAsync(message.Chat, isAlreadyAuthenticatedText, null, ParseMode.Markdown);
                return true;
            }
            return true;
        }
        
    }
}
