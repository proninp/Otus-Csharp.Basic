using MTLServiceBot.Bot.Commands;
using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ICommand = MTLServiceBot.Bot.Commands.ICommand;

namespace MTLServiceBot.Bot
{
    public class TgUpdateHandler
    {
        private static Dictionary<long, Session> userSessions = new Dictionary<long, Session>();
        private readonly string _commandLogTemplate = "Command {@command}, exception message: {@message}";
        private readonly Command _unknownCommand;
        private readonly List<Command> _commands;
        public List<Command> Commands { get => _commands; }

        public TgUpdateHandler()
        {
            _unknownCommand = new Command("/unknown", "Оповещение о неизвестной команде", false, new Unknown());
            _commands = new List<Command>()
            {
                new Command("/start", "Запускает использование бота", false, new Start()),
                new Command("/login", "Инициирует процесс авторизации", false, new Login()),
                new Command("/getRequests", "Получание информацию о записях сервисных запросов", true, new RequestsList()),
                new Command("/stop", "Завершает использования бота", false, new Stop())
            };
            _commands.Add(new Command("/help", "Начать использование бота", false, new Help(_commands.Select(c => (c.Name, c.Description)))));
            _commands.Add(_unknownCommand);
        }
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            if (update.Type != UpdateType.Message)
            {
                Console.WriteLine($"Я еще не умею обрабатывать тип {update.Type}");
                return;

            }
            var command = message?.Text;
            Console.WriteLine("Received {@MessageText} message from {@ChatId} chat", message.Text, message.Chat.Id); // Log
            if (userSessions.ContainsKey(message.From.Id) && userSessions[message.From.Id].IsAuthenticated)
            {

            }
            else
            {
                var session = new Session(message.From.Id, message.Chat.Id);
                
                // создать пользователя, если не найден и добавить в словарь
                // пока что просто добавляем в словарь
            }


            ICommand handler = _commands.Find(cmd => cmd.Name == command)?.TgCommand ?? _unknownCommand.TgCommand;
            try
            {
                await handler.Handle(botClient, message, userSessions[message.From.Id].User);
            }
            catch (Exception e)
            {
                Console.WriteLine(_commandLogTemplate, command, e.Message);
            }
        }
    }
}
