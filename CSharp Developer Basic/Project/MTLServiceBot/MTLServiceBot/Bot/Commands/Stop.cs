using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MTLServiceBot.Bot.Commands
{
    public class Stop : ICommand
    {
        public async Task<bool> Handle(ITelegramBotClient botClient, Message message, Users.User userSession)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, $"До свидания, {userSession.Name}! Позднее, мы очистим историю взаимодействий с ботом.");

            return true;
        }
    }
}
