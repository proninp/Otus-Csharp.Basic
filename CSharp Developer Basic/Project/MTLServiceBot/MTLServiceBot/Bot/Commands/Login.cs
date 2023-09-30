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

        public override async Task Handle(ITelegramBotClient botClient, Message message, Session session)
        {
            if (session.IsAuthorized)
            {
                await botClient.SendTextMessageAsync(message.Chat,
                    string.Format(TextConsts.LoginAlreadyAuthorizedMsg, session.User.Login),
                    parseMode: ParseMode.Markdown,
                    replyMarkup: new ReplyKeyboardRemove());
                return;
            }
            switch (session.AuthStep)
            {
                case AuthStep.None:
                    await HandleStartAuthentication(botClient, message, session);
                    break;
                case AuthStep.Username:
                    await HandleUserLoginInput(botClient, message, session);
                    break;
                case AuthStep.Password:
                    await HandleUserPasswordInput(botClient, message, session);
                    break;
            }
            WorkflowMode = session.AuthStep != AuthStep.None;
        }
        public async Task HandleStartAuthentication(ITelegramBotClient botClient, Message message, Session session)
        {
            await botClient.SendTextMessageAsync(message.Chat, "Введите имя пользователя");
            session.AuthStep = AuthStep.Username;
        }
        public async Task HandleUserLoginInput(ITelegramBotClient botClient, Message message, Session session)
        {
            var usename = message.Text;
            if (string.IsNullOrEmpty(usename))
            {
                await botClient.SendTextMessageAsync(message.Chat, "Имя пользователя не может быть пустым!");
                session.AuthStep = AuthStep.None;
                return;
            }
            session.User.Login = message.Text;
            await botClient.SendTextMessageAsync(message.Chat, "Введите пароль");
            session.AuthStep = AuthStep.Password;
        }
        private async Task HandleUserPasswordInput(ITelegramBotClient botClient, Message message, Session session)
        {
            session.AuthStep = AuthStep.None;
            var password = message.Text;
            if (string.IsNullOrEmpty(password))
            {
                await botClient.SendTextMessageAsync(message.Chat, "Пароль не может быть пустым!");
                return;
            }
            session.User.Password = password;

            var api = ServiceAPI.GetInstance();
            var response = await api.Authorize(session);
            if (response.IsSuccess)
                await botClient.DeleteMessageAsync(message.Chat, message.MessageId, default); // Убираем из истории чата введенный пароль
            await botClient.SendTextMessageAsync(message.Chat, response.Message);
        }
    }
}
