﻿using Serilog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace MTLServiceBot.Bot
{
    public class TgBot
    {
        private readonly TgBotClient _bot;
        private readonly TgErrorHandler _errorHandling;
        private readonly TgUpdateHandler _updateHandler;

        public TgBot(TgErrorHandler errorHandling, TgUpdateHandler updateHandler, TgBotClient bot)
        {
            _errorHandling = errorHandling;
            _updateHandler = updateHandler;
            _bot = bot;
        }

        public void RunBot()
        {
            Log.Debug($"Запущен бот {_bot.TgUser.FirstName}");
            var cts = new CancellationTokenSource();

            RegisterCommands();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // receive all update types
            };
            _bot._client.StartReceiving(_updateHandler.HandleUpdateAsync, _errorHandling.HandlePollingErrorAsync, receiverOptions, cts.Token);
        }

        private void RegisterCommands()
        {
            var commands = _updateHandler.Commands
                .Select(cmd => new BotCommand { Command = GetCommandName(cmd.Name), Description = cmd.Description })
                .ToArray();
            var task = _bot._client.SetMyCommandsAsync(commands);
            task.Wait();
        }

        private string GetCommandName(string name) => (name.Length > 0 && name.StartsWith("/") ? name.Substring(1) : name);

        public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n" +
                    $"[{apiRequestException.ErrorCode}]:\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            Log.Error(errorMessage);
            return Task.CompletedTask;
        }
    }
}
