using Microsoft.Extensions.Logging;
using MTLServiceBot.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace MTLServiceBot.Telegram.Bot
{
    public class TelegramBot
    {
        private readonly TgBotClient _bot;
        private readonly TelegramErrorHandler _errorHandling;
        //private readonly ILogger<TgBot> _logger;
        //private readonly TgUpdateHandler _updateHandler;

        private static Dictionary<long, User> _userSessions = new Dictionary<long, User>();

        public static async Task InitBot()
        {
            _bot = new TelegramBotClient(AppConfig.BotToken);
            var me = await _bot.GetMeAsync();
            Console.WriteLine("Запущен бот " + me.FirstName); // TODO log
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            await RegisterCommands();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // receive all update types
            };
            _bot.StartReceiving(
                HandleUpdateAsync,
                HandlePollingErrorAsync,
                receiverOptions,
                cancellationToken
            );

            Console.ReadLine();

        }
        private static async Task RegisterCommands()
        {
            var commands = new[]
            {
                new BotCommand { Command = "start", Description = "Начать использование бота" },
                new BotCommand { Command = "login", Description = "Авторизация" },
                new BotCommand { Command = "listrequest", Description = "Получить список заявок" },
                new BotCommand { Command = "listrequestmessages", Description = "Получить список заявок сообщениями" },
                new BotCommand { Command = "help", Description = "Вывести список команд бота" },
            };
            await _bot.SetMyCommandsAsync(commands);
        }
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
        }
        public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n" +
                    $"[{apiRequestException.ErrorCode}]:\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

    }
}
