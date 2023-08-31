namespace MTLServiceBot.Users
{
    public class Session
    {
        public User? User { get; set; }
        public long Id { get; set; }
        public long ChatId { get; set; }
        public AuthenticationStep AuthenticationStep { get; set; }
        public bool IsAuthenticated { get; set; }
        public Session(long id, long chatId)
        {
            Id = id;
            ChatId = chatId;
            AuthenticationStep = AuthenticationStep.None;
        }
    }
}
