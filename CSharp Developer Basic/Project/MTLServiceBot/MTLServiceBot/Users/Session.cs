using MTLServiceBot.SQL;

namespace MTLServiceBot.Users
{
    public class Session
    {
        public TgUser User { get; set; }
        public long ChatId { get; set; }
        public AuthStep AuthStep { get; set; }
        public bool IsAuthenticated { get; set; }
        public DateTime LoginDatetime { get; set; }
        private DateTime _logoutDatetime;
        public DateTime LogoutDatetime 
        {
            get => _logoutDatetime;
            set
            {
                _logoutDatetime = value;
                if (_logoutDatetime != DateTime.Parse("1753-01-01"))
                {
                    IsAuthenticated = false;
                    AuthStep = AuthStep.None;
                    this.LogoutSession();
                }
            }
        }
        public Session(long id, long chatId, string? username, DateTime loginDatetime, DateTime logoutDatetime)
        {
            User = new TgUser(id, username);
            ChatId = chatId;
            LoginDatetime = loginDatetime;
            LogoutDatetime = logoutDatetime;
            AuthStep = AuthStep.None;
        }
        public Session(long id, long chatId, string login, string password, DateTime loginDatetime)
        {
            User = new TgUser(id, "", login, password);
            ChatId = chatId;
            LoginDatetime = loginDatetime;
        }
        public void SetCredentials(Session? session)
        {
            if (session == null)
                return;
            var pass = !string.IsNullOrEmpty(session.User.Password) ?
                    EncryptionHelper.Decrypt(session.User.Password, session.User.Id.ToString(), session.ChatId.ToString()) : "";
            User.Login = session.User.Login;
            User.Password = pass;
            LoginDatetime = session.LoginDatetime;
            IsAuthenticated = (User.Login?.Length > 0) && (User.Password?.Length > 0);
        }
        
        public override string ToString() =>
            $"UserId: {User.Id}; Username: {User.Name}; ChatId: {ChatId}; Login Datetime: {LoginDatetime}";
    }
}
