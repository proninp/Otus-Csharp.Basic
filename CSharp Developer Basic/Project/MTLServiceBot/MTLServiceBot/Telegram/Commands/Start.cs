using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MTLServiceBot.Telegram.Commands
{
    public class Start : ICommand
    {
        public async Task<bool> Handle(ITelegramBotClient botClient, Message message, User user)
        {
            await botClient.SendTextMessageAsync(message.Chat, $"Привет, {user.UserName}!");
            await botClient.SendTextMessageAsync(message.Chat, "Введите /login для авторизации.");
            return true;
        }
    }
}
