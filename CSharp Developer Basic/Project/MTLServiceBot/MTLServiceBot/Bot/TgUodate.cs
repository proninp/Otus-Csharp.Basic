using MTLServiceBot.Users;
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
    }
}
