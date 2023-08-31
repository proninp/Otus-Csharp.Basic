using Telegram.Bot;
using Telegram.Bot.Types;

namespace MTLServiceBot.Bot.Commands
{
    public class Start : ICommand
    {
        public async Task<bool> Handle(ITelegramBotClient botClient, Message message, Users.User userSession)
        {
            await botClient.SendTextMessageAsync(message.Chat, $"Привет, {userSession.Name}!");
            await botClient.SendTextMessageAsync(message.Chat, "Введите /login для авторизации.");
            return true;
        }
    }
}
