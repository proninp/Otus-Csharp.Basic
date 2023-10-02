﻿using MTLServiceBot.API;
using MTLServiceBot.Assistants;
using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MTLServiceBot.Bot.Commands
{
    public class Login : Command
    {
        public Login(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication) { }

        public override async Task HandleAsync(ITelegramBotClient botClient, Update update, Session session)
        {
            var message = update.Message!;
            if (session.IsAuthorized)
            {
                _ = botClient.SendTextMessageAsync(message.Chat,
                    string.Format(TextConsts.LoginAlreadyAuthorized, session.User.Login),
                    parseMode: ParseMode.Markdown,
                    replyMarkup: new ReplyKeyboardRemove());
                return;
            }
            switch (session.AuthStep)
            {
                case AuthStep.None:
                    HandleStartAuthentication(botClient, message, session);
                    break;
                case AuthStep.Username:
                    HandleUserLoginInput(botClient, message, session);
                    break;
                case AuthStep.Password:
                    await HandleUserPasswordInput(botClient, message, session);
                    break;
            }
            WorkflowMode = session.AuthStep != AuthStep.None;
        }

        public void HandleStartAuthentication(ITelegramBotClient botClient, Message message, Session session)
        {
            session.AuthStep = AuthStep.Username;
            _ = botClient.SendTextMessageAsync(message.Chat, TextConsts.EnterLogin);
        }

        public void HandleUserLoginInput(ITelegramBotClient botClient, Message message, Session session)
        {
            if (string.IsNullOrEmpty(message.Text))
            {
                session.AuthStep = AuthStep.None;
                _ = botClient.SendTextMessageAsync(message.Chat, TextConsts.LoginEmptyError);
                return;
            }
            session.User.Login = message.Text;
            session.AuthStep = AuthStep.Password;
            _ = botClient.SendTextMessageAsync(message.Chat, TextConsts.EnterPassword);
        }

        private async Task HandleUserPasswordInput(ITelegramBotClient botClient, Message message, Session session)
        {
            session.AuthStep = AuthStep.None;
            var password = message.Text;
            if (string.IsNullOrEmpty(password))
            {
                _ = botClient.SendTextMessageAsync(message.Chat, TextConsts.PasswordEmptyError);
                return;
            }
            session.User.Password = password;

            var api = ServiceAPI.GetInstance();
            var response = await api.Authorize(session);
            if (response.IsSuccess)
                await botClient.DeleteMessageAsync(message.Chat, message.MessageId, default); // Убираем из истории чата введенный пароль
            _ = botClient.SendTextMessageAsync(message.Chat, response.Message);
        }
    }
}
