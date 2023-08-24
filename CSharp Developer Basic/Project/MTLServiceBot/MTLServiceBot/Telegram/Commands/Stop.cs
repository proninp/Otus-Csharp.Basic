using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MTLServiceBot.Telegram.Commands
{
    public class Stop : ICommand
    {
        public async Task<bool> Handle(ITelegramBotClient botClient, Message message, User user)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, $"До свидания, {user.UserName}! Позднее, мы очистим историю взаимодействий с ботом.");

            return true;
        }
    }
}
