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
            if (!CheckUpdateValid(botClient, update))
                return;
            var messageData = new TgUpdate(update.Id, update.Type, GetUserFromUpdate(update)!, GetMessageFromUpdate(update)!, update.CallbackQuery);

            var commandText = messageData.Message!.Text ?? "";
            Program.ColoredPrint(string.Format(TextConsts.NewUpdateLog, messageData.UpdateType.ToString(),
                messageData.Chat!.Id, messageData.From!.Id, commandText));

            Session session = GetUserSession(messageData.Message!);
            Command command = GetCommand(commandText);

            try
            {
                if(command.CheckAuthorization(botClient, messageData, session))
                    await command.HandleAsync(botClient, messageData, session);
            }
            catch (Exception e)
            {
                Program.ColoredPrint(string.Format(TextConsts.CommandExceptionLogTemplate, commandText, e.Message),
                    ConsoleColor.Red); // TODO Logging
            }
        }

        private bool CheckUpdateValid(ITelegramBotClient botClient, Update update)
        {
            var msg = GetMessageFromUpdate(update);
            var from = GetUserFromUpdate(update);
            if (update.Type is not (UpdateType.Message or UpdateType.CallbackQuery))
            {
                ShowWarning(botClient, msg, string.Format(TextConsts.ReceivedUpdateTypeUnknownLog, update.Type));
                return false;
            }
            if (msg is null || msg.Chat is null)
            {
                ShowWarning(botClient, msg, string.Format(TextConsts.ReceivedUpdateTypeDataUnknown, update.Id));
                return false;
            }
            if (from is null)
            {
                ShowWarning(botClient, msg, string.Format(TextConsts.ReceivedUpdateFromUnknown, update.Id));
                return false;
            }
            if (update.Type is UpdateType.Message && msg.Type is not MessageType.Text)
            {
                ShowWarning(botClient, msg, TextConsts.MessageTypeError);
                return false;
            }
            return true;
        }

        private Message? GetMessageFromUpdate(Update update) =>
            update.Message ?? update.CallbackQuery?.Message;

        private User? GetUserFromUpdate(Update update) =>
            update.Message?.From ?? update.CallbackQuery?.From;

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
