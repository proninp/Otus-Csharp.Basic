using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MTLServiceBot.Bot
{
    public class TgUpdate
    {
        public long UpdateId { get; private set; }
        public UpdateType UpdateType { get; private set; }
        public User From { get; private set; }
        public Message Message { get; private set; }
        public Chat Chat { get => Message!.Chat; }
        public string? Text { get => Message.Text; }
        public CallbackQuery? CallbackQuery { get; private set; }

        public TgUpdate(long updateId, UpdateType type, User from, Message message, CallbackQuery? callbackQuery)
        {
            UpdateId = updateId;
            UpdateType = type;
            From = from;
            Message = message;
            CallbackQuery = callbackQuery;
        }

        public string GetUserName()
        {
            string username = "";
            if (string.IsNullOrEmpty(From.FirstName) && string.IsNullOrEmpty(From.LastName))
                username = $"{From.FirstName} {From.LastName}";
            else if (string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(From.Username))
                username = From.Username;
            else
                username = From.Id.ToString();
            return username;
        }

        public bool HasAttachment() =>
            Message.Type is (MessageType.Photo or
            MessageType.Video or
            MessageType.Document) &&
            (Message.Photo is not null ||
            Message.Video is not null ||
            Message.Document is not null);
    }
}
