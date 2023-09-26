using MTLServiceBot.API;
using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MTLServiceBot.Bot.Commands
{
    public class Login : Command
    {
        public Login(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication) { }

        public override async Task Handle(ITelegramBotClient botClient, Message message, Session session)
        {
            if (session.IsAuthorized)
            {
                await botClient.SendTextMessageAsync(message.Chat, $"Вы уже авторизованы, как {session.User.Login}.", null, ParseMode.Markdown);
                return;
            }
            switch (session.AuthStep)
            {
                case AuthStep.None:
                    await HandleStartAuthentication(botClient, message, session);
                    break;
                case AuthStep.Username:
                    await HandleUserLoginInput(botClient, message, session);
                    break;
                case AuthStep.Password:
                    await HandleUserPasswordInput(botClient, message, session);
                    break;
            }
        }
        public async Task HandleStartAuthentication(ITelegramBotClient botClient, Message message, Session session)
        {
            await botClient.SendTextMessageAsync(message.Chat, "Введите имя пользователя");
            session.AuthStep = AuthStep.Username;
        }
        public async Task HandleUserLoginInput(ITelegramBotClient botClient, Message message, Session session)
        {
            var usename = message.Text;
            if (string.IsNullOrEmpty(usename))
            {
                await botClient.SendTextMessageAsync(message.Chat, "Имя пользователя не может быть пустым!");
                session.AuthStep = AuthStep.None;
                return;
            }
            session.User.Login = message.Text;
            await botClient.SendTextMessageAsync(message.Chat, "Введите пароль");
            session.AuthStep = AuthStep.Password;
        }
        private async Task HandleUserPasswordInput(ITelegramBotClient botClient, Message message, Session session)
        {
            session.AuthStep = AuthStep.None;
            var password = message.Text;
            if (string.IsNullOrEmpty(password))
            {
                await botClient.SendTextMessageAsync(message.Chat, "Пароль не может быть пустым!");
                return;
            }
            session.User.Password = password;

            var api = ServiceAPI.GetInstance();
            var response = await api.Authorize(session);
            if (response.IsSuccess)
                await botClient.DeleteMessageAsync(message.Chat, message.MessageId, default); // Убираем из истории чата введенный пароль
            await botClient.SendTextMessageAsync(message.Chat, $"Добро пожаловать, {session.User.Name}! Вы успешно авторизованы!");
        }
    }
}
