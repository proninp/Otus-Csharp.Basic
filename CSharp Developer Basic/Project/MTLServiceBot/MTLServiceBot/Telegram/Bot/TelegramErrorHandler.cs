using Telegram.Bot.Exceptions;
using Telegram.Bot;


namespace MTLServiceBot.Telegram.Bot
{
    public class TelegramErrorHandler
    {
        //private readonly ILogger<TelegramErrorHandler> _logger;

        //public TelegramErrorHandler(ILogger<TelegramErrorHandler> logger)
        //{
        //    _logger = logger;
        //}

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]:\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            //_logger.LogError(errorMessage);
            return Task.CompletedTask;
        }
    }
}
