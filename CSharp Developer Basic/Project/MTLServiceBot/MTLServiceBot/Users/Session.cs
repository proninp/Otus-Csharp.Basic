namespace MTLServiceBot.Users
{
    public class Session
    {
        public TgUser User { get; set; }
        public long ChatId { get; set; }
        public bool IsAuthenticated { get; set; }
        public DateTime LoginDatetime { get; set; }
        public DateTime LogoutDatetime { get; set; }
        public Session(long id, long chatId, string? username, DateTime loginDatetime, DateTime logoutDatetime)
        {
            User = new TgUser(id, username);
            ChatId = chatId;
            LoginDatetime = loginDatetime;
            LogoutDatetime = logoutDatetime;
        }
        public Session(long id, long chatId, string login, string password, DateTime loginDatetime)
        {
            User = new TgUser(id, "", login, password);
            ChatId = chatId;
            LoginDatetime = loginDatetime;
        }
        public override string ToString() =>
            $"UserId: {User.Id}; Username: {User.Name}; ChatId: {ChatId}; Login Datetime: {LoginDatetime}";
    }
}
