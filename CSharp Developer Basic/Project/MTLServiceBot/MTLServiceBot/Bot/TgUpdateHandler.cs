using MTLServiceBot.Assistants;
using MTLServiceBot.Bot.Commands;
using MTLServiceBot.Bot.Commands.ServiceRequest;
using MTLServiceBot.Users;
using System.Collections.Concurrent;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MTLServiceBot.Bot
{
    public class TgUpdateHandler
    {
        private static ConcurrentDictionary <long, Session> _userSessions = new ConcurrentDictionary<long, Session>();
        private readonly Command _unknownCommand;
        private readonly Command _login;
        private readonly Command _logout;
        private readonly Command _serviceTasksRequest;
        private readonly Command _otpgen;

        private readonly List<Command> _commands;
        private readonly Dictionary<WorkFlow, Command> _workflows;
        public List<Command> Commands { get => _commands; }

        public TgUpdateHandler()
        {
            _login = new Login(TextConsts.LoginCommandName, TextConsts.LoginCommandDescription, false);
            _logout = new Logout(TextConsts.LogoutCommandName, TextConsts.LogoutCommandDescription, true);
            _serviceTasksRequest = new ServiceRequest(TextConsts.ServiceTasksCommandName, TextConsts.ServiceCommandDescription, true);
            _otpgen = new Otpgen(TextConsts.OtpgenCommandName, TextConsts.OtpgenCommandDescription, true);
            _unknownCommand = new Unknown(TextConsts.UnknownCommandName, TextConsts.UnknownCommandDescription, false);
            _commands = new()
            {
                new Start(TextConsts.StartCommandName, TextConsts.StartCommandDescription, false),
                _login,
                _logout,
                _serviceTasksRequest,
                _otpgen,
                new Stop(TextConsts.StopCommandName, TextConsts.StopCommandDescription, false)
            };

            var helpCommadInfo = _commands.Select(c => (c.Name, c.Description));
            _commands.Add(new Help(TextConsts.HelpCommandName, TextConsts.HelpCommandDescription, false, helpCommadInfo));
            _commands.Add(_unknownCommand);
            _workflows = new() 
            { 
                { WorkFlow.Login, _login },
                { WorkFlow.ServiceRequests, _serviceTasksRequest },
                { WorkFlow.ServiceRequestAddFile, _serviceTasksRequest },
                { WorkFlow.Otpgen, _otpgen },
            };
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (!CheckUpdateValid(botClient, update))
                return;
            var tgUpdate = new TgUpdate(update.Id, update.Type, GetUserFromUpdate(update)!, GetMessageFromUpdate(update)!, update.CallbackQuery);
            _ = HandleUpdateAsync(botClient, tgUpdate, cancellationToken);
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, TgUpdate update, CancellationToken cancellationToken)
        {
            var commandText = update.Message!.Text ?? "";
            var logText = string.Format(TextConsts.UpdateNewReceivingLog, update.UpdateType.ToString(),
                update.Chat!.Id, update.From!.Id, update.From.Username, commandText);
            AssistLog.ColoredPrint(logText, LogStatus.Warning);

            Session session = GetUserSession(update);
            Command command = GetCommand(commandText, session);

            try
            {
                if (command.CheckAuthorization(botClient, update, session))
                    await command.HandleAsync(botClient, update, session);
            }
            catch (Exception e)
            {
                AssistLog.ColoredPrint(string.Format(TextConsts.UpdateCommandExceptionTemplate, commandText, e.Message), LogStatus.Error);
            }
        }

        private bool CheckUpdateValid(ITelegramBotClient botClient, Update update)
        {
            var msg = GetMessageFromUpdate(update);
            var from = GetUserFromUpdate(update);
            if (update.Type is not (UpdateType.Message or UpdateType.CallbackQuery))
            {
                ShowWarning(botClient, msg, string.Format(TextConsts.UpdateTypeUnknownLog, update.Type));
                return false;
            }
            if (msg is null || msg.Chat is null)
            {
                ShowWarning(botClient, msg, string.Format(TextConsts.UpdateTypeDataUnknown, update.Id));
                return false;
            }
            if (from is null)
            {
                ShowWarning(botClient, msg, string.Format(TextConsts.UpdateFromUnknown, update.Id));
                return false;
            }
            if (update.Type is UpdateType.Message &&
                msg.Type is not (MessageType.Text or MessageType.Photo or MessageType.Video or MessageType.Document))
            {
                ShowWarning(botClient, msg, TextConsts.UpdateMessageTypeError);
                return false;
            }
            if (from.IsBot)
            {
                var logMsg = string.Format(TextConsts.UpdateMessageFromBotError, msg.Chat.Id, from.Id, from.Username, msg.Text);
                AssistLog.ColoredPrint(logMsg, LogStatus.Attention);
                return false;
            }
            return true;
        }

        private Message? GetMessageFromUpdate(Update update) =>
            update.Message ?? update.CallbackQuery?.Message;

        private User? GetUserFromUpdate(Update update) =>
            update.Message?.From ?? update.CallbackQuery?.From;

        private Session GetUserSession(TgUpdate update)
        {
            Session session;
            var userId = update.From.Id;
            if (_userSessions.ContainsKey(userId))
                session = _userSessions[userId];
            else
            {
                session = new Session(userId, update.Chat.Id, update.From.Username, DateTime.Now, DateTime.Parse("1753-01-01"));
                _userSessions.TryAdd(userId, session);
            }
            TgUser? user = session.User;
            if (user is null)
            {
                user = new TgUser(userId, update.From.Username, update.From.FirstName, update.From.LastName);
                session.User = user;
            }
            return session;
        }

        private Command GetCommand(string commandText, Session session)
        {
            Command command = _commands.Find(c => c.Name == commandText) ?? _unknownCommand;
            if (command.GetType().Equals(_unknownCommand.GetType())) // Если неизвестная команда, то это может быть workflow активной команды
            {
                if (session.WorkFlowState != WorkFlow.None && _workflows.ContainsKey(session.WorkFlowState))
                    command = _workflows[session.WorkFlowState];
            }
            else
                session.WorkFlowState = WorkFlow.None; // Если пришла новая команда, то сбрасываем все Workflow
            return command;
        }

        private void ShowWarning(ITelegramBotClient botClient, Message? msg, string warningText)
        {
            AssistLog.ColoredPrint(warningText, LogStatus.Error);
            if (msg is not null && msg.Chat is not null)
                botClient.SendTextMessageAsync(msg.Chat, warningText, parseMode: ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
        }
    }
}
