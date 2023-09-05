using MTLServiceBot.Bot.Commands;
using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MTLServiceBot.Bot
{
    public class TgUpdateHandler
    {
        private static Dictionary<long, Session> userSessions = new Dictionary<long, Session>();
        private readonly string _commandLogTemplate = "Команда {@commandText}, исключение: {@message}";
        private readonly Command _unknownCommand;
        private readonly List<Command> _commands;
        public List<Command> Commands { get => _commands; }

        public TgUpdateHandler()
        {
            _unknownCommand = new Unknown("/unknown", "Оповещение о неизвестной команде", false);
            _commands = new List<Command>()
            {
                new Start("/start", "Запускает использование бота", false),
                new Login("/login", "Инициирует процесс авторизации", false),
                new RequestsList("/getRequests", "Получание информацию о записях сервисных запросов", true),
                new Stop("/stop", "Завершает использования бота", false)
            };
            _commands.Add(new Help("/help", "Инструкции по использованию бота", false, _commands.Select(c => (c.Name, c.Description))));
            _commands.Add(_unknownCommand);
        }
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (!CheckUpdateRefferenceValid(update))
                return;
            var message = update.Message;
            var commandText = message.Text;
            

            Console.WriteLine($"Received {message.Text} message in '{message.Chat.Id}' chat from id: {message.From.Id}"); // Log

            GetUserSession(out Session userSession, message);

            var command = _commands.Find(cmd => cmd.Name == commandText) ?? _unknownCommand;
            try
            {
                var isAuthorized = await command.CheckAuthentication(botClient, message, userSession);
                if (isAuthorized)
                    await command.Handle(botClient, message, userSession);
            }
            catch (Exception e)
            {
                Console.WriteLine(_commandLogTemplate, commandText, e.Message);
            }
        }
        private bool CheckUpdateRefferenceValid(Update? update)
        {
            if (update?.Type != UpdateType.Message)
            {
                Console.WriteLine($"Я еще не умею обрабатывать тип {update.Type}");
                return false;

            }
            if (update.Message == null)
            {
                Console.WriteLine($"Нераспознано сообщение {update.Id}");
                return false;
            }
            if (update.Message.From == null)
            {
                Console.WriteLine($"Нераспознан пользователь в сообщении {update.Id}");
                return false;
            }
            return true;
        }

        private void GetUserSession(out Session userSession, Message message)
        {
            var userId = message.From.Id;
            TgUser? user = null;
            if (userSessions.ContainsKey(userId))
            {
                userSession = userSessions[userId];
                user = userSession.User;
            }
            else
            {
                userSession = new Session(userId, message.Chat.Id, message.From.Username, DateTime.Now, DateTime.Parse("1753-01-01"));
                userSessions.Add(userId, userSession);
            }
            if (user == null)
            {
                user = new TgUser(userId, message.From.Username);
                userSession.User = user;
            }
        }
    }
}
