using MTLServiceBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MTLServiceBot.Bot.Commands
{
    public class Stop : Command
    {
        public Stop(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication)
        {
        }

        public override async Task Handle(ITelegramBotClient botClient, Message message, Session userSession)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, $"До свидания, {userSession.User.Name}! Позднее, мы очистим историю взаимодействий с ботом.");
        }
    }
}
