﻿using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MTLServiceBot.Telegram.Commands
{
    internal class Login : ICommand
    {
        public Task<bool> Handle(ITelegramBotClient botClient, Message message, User user)
        {

        }
    }
}
