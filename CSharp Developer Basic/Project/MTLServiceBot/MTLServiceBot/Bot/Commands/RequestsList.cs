namespace MTLServiceBot.Bot.Commands
{
    public class RequestsList : Command
    {
        public RequestsList(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication)
        {
        }

        //public override async Task<bool> Handle(ITelegramBotClient botClient, Message message, Session session)
        //{
        //    await botClient.SendTextMessageAsync(message.Chat, $"Я пока что не умею обрабатывать данную команду", null, ParseMode.Markdown);
        //    return true;
        //}
    }
}
