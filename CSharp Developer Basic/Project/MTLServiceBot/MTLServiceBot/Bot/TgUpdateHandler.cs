using MTLServiceBot.Assistants;
using MTLServiceBot.Bot.Commands;
using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MTLServiceBot.Bot
{
    public class TgUpdateHandler
    {
        private static Dictionary<long, Session> _userSessions = new Dictionary<long, Session>();
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
            _serviceTasksRequest = new ServiceTasksRequest(TextConsts.ServiceTasksCommandName, TextConsts.ServiceCommandDescription, true);
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
            var message = update.Message ?? update.CallbackQuery?.Message;
            var from = update.Message?.From ?? update.CallbackQuery?.From;

            var updateValidator = CheckUpdateValid(update, message, from);
            if (!updateValidator.isValid)
            {
                ShowWarning(botClient, message, updateValidator.errorMsg);
                return;
            }

            var commandText = message!.Text ?? "";
            Program.ColoredPrint(string.Format(TextConsts.NewUpdateLog, update.Type.ToString(), message?.Chat.Id, from?.Id, message?.Text));

            Session session = GetUserSession(message!);
            Command command = GetCommand(commandText);

            try
            {
                if(command.CheckAuthorization(botClient, update, session))
                    await command.HandleAsync(botClient, update, session);
            }
            catch (Exception e)
            {
                Program.ColoredPrint(string.Format(TextConsts.CommandExceptionLogTemplate, commandText, e.Message),
                    ConsoleColor.Red); // TODO Logging
            }
        }

        private (bool isValid, string errorMsg) CheckUpdateValid(Update update, Message? message, User? from)
        {
            if (update.Type is not (UpdateType.Message or UpdateType.CallbackQuery))
                return (false, string.Format(TextConsts.ReceivedUpdateTypeUnknownLog, update.Type));
            
            if (message is null)
                return (false, string.Format(TextConsts.ReceivedUpdateTypeDataUnknown, update.Id));

            if (from is null)
                return (false, string.Format(TextConsts.ReceivedUpdateFromUnknown, update.Id));

            if (update.Type is UpdateType.Message && message.Type is not MessageType.Text)
                return (false, TextConsts.MessageTypeError);

            return (true, "");
        }

        private Session GetUserSession(Message message)
        {
            Session session;
            var userId = message.From!.Id;
            TgUser? user = null;
            if (_userSessions.ContainsKey(userId))
                session = _userSessions[userId];
            else
            {
                session = new Session(userId, message.Chat.Id, message.From.Username, DateTime.Now, DateTime.Parse("1753-01-01"));
                _userSessions.Add(userId, session);
            }
            user = session.User;
            if (user is null)
            {
                user = new TgUser(userId, message.From.Username);
                session.User = user;
            }
            return session;
        }

        private Command GetCommand(string commandText)
        {
            Command command = _commands.Find(c => c.Name == commandText) ?? _unknownCommand;
            if (command.GetType().Equals(_unknownCommand.GetType()))
            {
                if (_commands.Any(c => c.WorkflowMode)) // Если неизвестная команда, то это может быть workflow активной команды
                    command = _commands.First(c => c.WorkflowMode);
            }
            else
                _commands.ForEach(cmd => cmd.WorkflowMode = false); // Если пришла новая команда, то сбрасываем все Workflow
            return command;
        }

        private void ShowWarning(ITelegramBotClient botClient, Message? msg, string warningText)
        {
            Program.ColoredPrint(warningText, ConsoleColor.Red); // TODO Log
            if (msg is not null && msg.Chat is not null)
                botClient.SendTextMessageAsync(msg.Chat, warningText, parseMode: ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
        }
    }
}
