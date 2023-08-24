using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MTLServiceBot.Telegram.Commands
{
    public class Help : ICommand
    {
        public async Task<bool> Handle(ITelegramBotClient botClient, Message message, User user)
        {
            var helpMessage = "*Инструкция по работе с ботом*\r\n\r\n"
                + "*Список команд:*\r\n"
                + "/start - Начать использование бота\r\n"
                + "/login - Авторизация\r\n"
                + "/listtasks - Получить список заявок\r\n"
                + "/listtasksmessages - Получить список заявок сообщениями";

            await botClient.SendTextMessageAsync(message.Chat, helpMessage, null, ParseMode.Markdown);
            return true;
        }
    }
}