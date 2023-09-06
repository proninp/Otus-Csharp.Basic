using MTLServiceBot.SQL;
using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MTLServiceBot.Bot.Commands
{
    public class Login : Command
    {
        public Login(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication) { }

        public override async Task<bool> Handle(ITelegramBotClient botClient, Message message, Session session)
        {
            if (!session.IsAuthenticated)
                if (session.CheckActiveSessionExists())
                {
                    session.GetActiveSessionCredentials();
                    session.IsAuthenticated = (session.User.Login?.Length > 0) && (session.User.Password?.Length > 0);
                }
            if (session.IsAuthenticated)
            {
                await botClient.SendTextMessageAsync(message.Chat, $"Вы уже авторизованы, как {session.User.Login}.", null, ParseMode.Markdown);
                return true;
            }
            switch (session.AuthStep)
            {
                case AuthStep.None:
                    break;
                case AuthStep.Username:
                    break;
                case AuthStep.Password:
                    break;
                case AuthStep.CheckAuthentication:
                    break;


            }
            return true;
        }
        
    }
}
