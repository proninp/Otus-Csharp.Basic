using MTLServiceBot.API;
using MTLServiceBot.Assistants;
using MTLServiceBot.SQL;
using MTLServiceBot.Users;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MTLServiceBot.Bot.Commands
{
    public class Login : Command
    {

        public Login(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication) 
        { 
            
        }

        public override async Task HandleAsync(ITelegramBotClient botClient, TgUpdate update, Session session)
        {
            if (session.IsAuthorized)
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.LoginAlreadyAuthorized, session.User.Login),
                    parseMode: ParseMode.Html,
                    replyMarkup: new ReplyKeyboardRemove());
                return;
            }

            if (!session.CheckAvailableAuthorizationAttempts())
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.LoginNoAuthorizedAttempts, session.User.Login),
                    parseMode: ParseMode.Html);
                return;
            }

            switch (session.AuthStep)
            {
                case AuthStep.None:
                    HandleStartAuthentication(botClient, update.Message, session);
                    break;
                case AuthStep.Username:
                    HandleUserLoginInput(botClient, update.Message, session);
                    break;
                case AuthStep.Password:
                    await HandleUserPasswordInput(botClient, update.Message, session);
                    break;
            }
            if (session.AuthStep != AuthStep.None)
                session.WorkFlowState = WorkFlow.Login;
        }

        private void HandleStartAuthentication(ITelegramBotClient botClient, Message message, Session session)
        {
            session.AuthStep = AuthStep.Username;
            _ = botClient.SendTextMessageAsync(message.Chat, 
                string.Format(TextConsts.EnterLogin, session.AvailableAuthorizationAttemts), 
                parseMode: ParseMode.Html);
        }

        private void HandleUserLoginInput(ITelegramBotClient botClient, Message message, Session session)
        {
            if (string.IsNullOrEmpty(message.Text))
            {
                session.AuthStep = AuthStep.None;
                _ = botClient.SendTextMessageAsync(message.Chat, TextConsts.LoginEmptyError);
                return;
            }
            session.User.Login = message.Text;
            session.AuthStep = AuthStep.Password;
            _ = botClient.SendTextMessageAsync(message.Chat, TextConsts.EnterPassword);
        }

        private async Task HandleUserPasswordInput(ITelegramBotClient botClient, Message message, Session session)
        {
            session.AuthStep = AuthStep.None;
            var password = message.Text;
            if (string.IsNullOrEmpty(password))
            {
                _ = botClient.SendTextMessageAsync(message.Chat, TextConsts.PasswordEmptyError);
                return;
            }
            
            session.User.Password = password;
            var api = new ServiceAPI(session);
            var response = await api.Authorize();
            
            var authMessage = new StringBuilder();
            authMessage.AppendLine(response.Message);

            if (response.IsSuccess)
                await botClient.DeleteMessageAsync(message.Chat, message.MessageId, default); // Убираем из истории чата введенный пароль
            else
            {
                session.DecreaseAvailableSessions();
                authMessage.AppendLine(string.Format(TextConsts.AvailableAuthAttemptsPhrase, session.AvailableAuthorizationAttemts));
            }
            _ = botClient.SendTextMessageAsync(message.Chat, authMessage.ToString(), parseMode: ParseMode.Html);
        }
    }
}
