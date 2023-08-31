using MTLServiceBot.Users;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MTLServiceBot.Bot.Commands
{
    public class Help : ICommand
    {
        private readonly string _commandDescription;
        public Help(IEnumerable<(string Name, string Description)> commandsInfo)
        {
            _commandDescription = string.Join("\r\n", commandsInfo.Select(cmd => $"{cmd.Name} - {cmd.Description}"));
        }

        public async Task<bool> Handle(ITelegramBotClient botClient, Message message, Users.User userSession)
        {
            var helpMessage = "*Инструкция по работе с ботом*\r\n\r\n"
                + "*Список команд:*\r\n"
                + _commandDescription;

            await botClient.SendTextMessageAsync(message.Chat, helpMessage, null, ParseMode.Markdown);
            return true;
        }
    }
}