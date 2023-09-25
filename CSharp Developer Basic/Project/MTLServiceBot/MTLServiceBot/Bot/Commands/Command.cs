using MTLServiceBot.Users;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Azure;
using MTLServiceBot.API;

namespace MTLServiceBot.Bot.Commands
{
    public abstract class Command
    {
        private readonly string _name;
        private readonly string _description;
        private readonly bool _isRequireAuthentication;
        public string Name { get => _name; }
        public string Description { get => _description; }
        public bool IsRequireAuthentication { get => _isRequireAuthentication; }
        public Command(string name, string description, bool isRequireAuthentication)
        {
            _name = name;
            _description = description;
            _isRequireAuthentication = isRequireAuthentication;
        }
        public async Task<bool> CheckAuthentication(ITelegramBotClient botClient, Message message, Session session)
        {
            if (!IsRequireAuthentication || session.IsAuthorized)
                return true;

            var unauthMessage = $"Для выполнения команды {Name} требуется авторизация.";
            await botClient.SendTextMessageAsync(message.Chat, unauthMessage, null, ParseMode.Markdown);
            
            return false;
        }

        public virtual async Task<bool> ReAuthentication(ITelegramBotClient botClient, Message message, Session session)
        {
            var api = ServiceAPI.GetInstance();
            var authResponse = await api.Authorize(session);
            switch (authResponse.Status)
            {
                case ApiResponseStatus.Unauthorized:
                    await botClient.SendTextMessageAsync(message.Chat, CaptionConstants.UnauthorizedError);
                    return false;
                case ApiResponseStatus.Error:
                    await botClient.SendTextMessageAsync(message.Chat, CaptionConstants.ServerConnectionError);
                    return false;
                case ApiResponseStatus.Success:
                    session.SetSessionAuthorization(authResponse.ResponseText);
                    return true;
                default:
                    Program.ColoredPrint($"Обработка ответа {nameof(authResponse)} со значением {authResponse} не реализована в системе", ConsoleColor.Red);
                    return false;
            }
        }

        public virtual async Task Handle(ITelegramBotClient botClient, Message message, Session session)
        {
            await botClient.SendTextMessageAsync(message.Chat, $"Я пока что не умею обрабатывать данную команду", null, ParseMode.Markdown);
        }
    }
}
