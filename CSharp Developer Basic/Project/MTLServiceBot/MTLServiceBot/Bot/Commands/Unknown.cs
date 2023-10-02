﻿using MTLServiceBot.Assistants;
using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MTLServiceBot.Bot.Commands
{
    public class Unknown : Command
    {
        public Unknown(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication)
        {
        }

        public override Task HandleAsync(ITelegramBotClient botClient, Update update, Session userSession)
        {
            _ = botClient.SendTextMessageAsync(update.Message!.Chat,
                TextConsts.UnknownCommand,
                replyMarkup: new ReplyKeyboardRemove());
            return Task.CompletedTask;
        }
    }
}
