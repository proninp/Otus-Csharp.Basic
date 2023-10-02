using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MTLServiceBot.Bot.Commands
{
    public class Help : Command
    {
        private readonly string _commandDescription;

        public Help(string name, string description, bool isRequireAuthentication, IEnumerable<(string Name, string Description)> commandsInfo) :
            base(name, description, isRequireAuthentication)
        {
            _commandDescription = string.Join("\r\n", commandsInfo.Select(cmd => $"{cmd.Name} - {cmd.Description}"));
        }

        public override async Task HandleAsync(ITelegramBotClient botClient, Update update, Session session)
        {
            var helpMessage = "*Инструкция по работе с ботом*\r\n\r\n"
                + "*Список команд:*\r\n"
                + _commandDescription;

            await botClient.SendTextMessageAsync(update.Message!.Chat, helpMessage,
                parseMode: ParseMode.Markdown,
                replyMarkup: new ReplyKeyboardRemove());
        }
    }
}