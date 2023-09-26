﻿using MTLServiceBot.Bot.Commands;
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
        private readonly List<Command> _commands;
        public List<Command> Commands { get => _commands; }

        public TgUpdateHandler()
        {
            _login = new Login(CaptionConstants.LoginCaption, CaptionConstants.LoginDescription, false);
            _unknownCommand = new Unknown(CaptionConstants.UnknownCaption, CaptionConstants.UnknownDescription, false);
            _commands = new List<Command>()
            {
                new Start(CaptionConstants.StartCaption, CaptionConstants.StartDescription, false),
                _login,
                new Logout(CaptionConstants.LogoutCaption, CaptionConstants.LogoutDescription, true),
                new GetServiceTasks(CaptionConstants.ServiceTasksCaption, CaptionConstants.ServiceDescription, true),
                new Stop(CaptionConstants.StopCaption, CaptionConstants.StopDescription, false)
            };

            _commands.Add(new Help(CaptionConstants.HelpCaption, CaptionConstants.HelpDescription, false, _commands.Select(c => (c.Name, c.Description))));
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
            if (session.AuthStep is not AuthStep.None)
                command = _login;
            else
                command = _commands.Find(cmd => cmd.Name == commandText) ?? _unknownCommand;
            return command;
        }
    }
}
