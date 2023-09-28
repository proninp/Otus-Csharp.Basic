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
        private readonly string _commandLogTemplate = "Команда {0}, исключение: {1}";
        private readonly Command _unknownCommand;
        private readonly Command _login;
        private readonly Command _logout;
        private readonly Command _serviceTasksRequest;
        private readonly List<Command> _commands;
        public List<Command> Commands { get => _commands; }

        public TgUpdateHandler()
        {
            _login = new Login(TextConsts.LoginCommandName, TextConsts.LoginCommandDescription, false);
            _logout = new Logout(TextConsts.LogoutCommandName, TextConsts.LogoutCommandDescription, true);
            _serviceTasksRequest = new SerciceTasksRequest(TextConsts.ServiceTasksCommandName, TextConsts.ServiceCommandDescription, true);
            _unknownCommand = new Unknown(TextConsts.UnknownCommandName, TextConsts.UnknownCommandDescription, false);
            _commands = new List<Command>()
            {
                new Start(TextConsts.StartCommandName, TextConsts.StartCommandDescription, false),
                _login,
                _logout,
                _serviceTasksRequest,
                new Stop(TextConsts.StopCommandName, TextConsts.StopCommandDescription, false)
            };

            var helpCommadInfo = _commands.Select(c => (c.Name, c.Description));
            _commands.Add(new Help(TextConsts.HelpCommandName, TextConsts.HelpCommandDescription, false, helpCommadInfo));
            _commands.Add(_unknownCommand);
        }
        
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (!await CheckUpdateRefferenceValid(botClient, update))
                return;
            var message = update.Message;
            var commandText = message?.Text ?? "";

            Program.ColoredPrint($"Получена команда [{commandText}] в чате [{message.Chat.Id}] от: [{message.From.Id}]", ConsoleColor.Green); // TODO Logging

            Session session = GetUserSession(message);
            Command command = GetCommand(session, commandText);
            try
            {
                var isAuthorized = await command.CheckAuthentication(botClient, message, session);
                if (isAuthorized)
                    await command.Handle(botClient, message, session);
            }
            catch (Exception e)
            {
                Program.ColoredPrint(string.Format(_commandLogTemplate, commandText, e.Message), ConsoleColor.Red); // TODO Logging
            }
        }
        
        private async Task<bool> CheckUpdateRefferenceValid(ITelegramBotClient botClient, Update? update)
        {
            if (update == null)
            {
                Program.ColoredPrint($"Нераспознано полученное обновление", ConsoleColor.Red); // TODO Logging
                return false;
            }

            if (update.Type != UpdateType.Message)
            {
                if (update.Message != null)
                    await botClient.SendTextMessageAsync(update.Message.Chat, $"Я еще не умею обрабатывать сообщения с типом {update.Type}", parseMode: ParseMode.Markdown);
                Program.ColoredPrint($"Получен запрос с типом {update?.Type} для которого не предусмотрено обработчика", ConsoleColor.Red);
                return false;
            }
            if (update.Message is null)
            {
                Program.ColoredPrint($"Нераспознано сообщение {update.Id}", ConsoleColor.Red);
                return false;
            }

            if (update.Message.From == null)
            {
                Program.ColoredPrint($"Нераспознан пользователь в сообщении {update.Id}", ConsoleColor.Red);
                return false;
            }
            return true;
        }

        private Session GetUserSession(Message message)
        {
            Session userSession;
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
            return userSession;
        }

        private Command GetCommand(Session session, string commandText)
        {
            Command command;
            
            command = _commands.Find(c => c.Name == commandText) ?? _unknownCommand;
            if (command.GetType().Equals(_unknownCommand.GetType()))
            {
                if (_commands.Any(c => c.WorkflowMode)) // Если неизвестная команда, то это может быть workflow активной команды
                    command = _commands.First(c => c.WorkflowMode);
            }
            else
                _commands.ForEach(cmd => cmd.WorkflowMode = false); // Если пришла новая команда, то сбрасываем все Workflow
            return command;
        }
    }
}
