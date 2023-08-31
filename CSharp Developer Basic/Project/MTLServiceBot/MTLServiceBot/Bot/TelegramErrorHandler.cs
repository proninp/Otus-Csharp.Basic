using Telegram.Bot.Exceptions;
using Telegram.Bot;


namespace MTLServiceBot.Bot
{
    public class TelegramErrorHandler
    {
        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]:\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(errorMessage); // TODO Log
            return Task.CompletedTask;
        }
    }
}
