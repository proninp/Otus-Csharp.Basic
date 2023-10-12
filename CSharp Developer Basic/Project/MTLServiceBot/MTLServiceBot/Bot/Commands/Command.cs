﻿using MTLServiceBot.Assistants;
using MTLServiceBot.Users;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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

        public bool CheckAuthorization(ITelegramBotClient botClient, TgUpdate messageData, Session session)
        {
            if (!IsRequireAuthentication || session.IsAuthorized)
                return true;

            var unauthMessage = string.Format(TextConsts.AuthorizationRequired, Name);
            _ = botClient.SendTextMessageAsync(messageData.Chat!, unauthMessage, null, ParseMode.Markdown);
            return false;
        }

        public virtual async Task HandleAsync(ITelegramBotClient botClient, TgUpdate messageData, Session session)
        {
            await botClient.SendTextMessageAsync(messageData.Chat!, TextConsts.UnknownCommand, null, ParseMode.Markdown);
        }

        protected virtual void SendNotification(ITelegramBotClient botClient, Chat chat, IReplyMarkup? replyButtons,
            string tgMessage, LogStatus logStatus = LogStatus.Information, string logMessage = "")
        {
            SendLogMessage(tgMessage, logStatus, logMessage);
            if (replyButtons is not null)
                _ = botClient.SendTextMessageAsync(chat, tgMessage, parseMode: ParseMode.Html, replyMarkup: replyButtons);
            else
                _ = botClient.SendTextMessageAsync(chat, tgMessage, parseMode: ParseMode.Html);
        }

        protected virtual void SendNotification(ITelegramBotClient botClient, Chat chat, string tgMessage,
            LogStatus logStatus = LogStatus.Information, string logMessage = "") => 
            SendNotification(botClient, chat, null, tgMessage, logStatus, logMessage);

        private void SendLogMessage(string tgMessage, LogStatus logStatus = LogStatus.Information, string logMessage = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format(TextConsts.LogNotificationDescription, Name));
            sb.AppendLine(tgMessage);
            if (!string.IsNullOrEmpty(logMessage))
                sb.AppendLine(string.Format(TextConsts.LogDescription, logMessage));

            AssistLog.ColoredPrint(sb.ToString(), logStatus);
        }
    }
}
