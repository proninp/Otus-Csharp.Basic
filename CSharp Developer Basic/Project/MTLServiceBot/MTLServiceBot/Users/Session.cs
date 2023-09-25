using MTLServiceBot.SQL;

namespace MTLServiceBot.Users
{
    public class Session
    {
        public TgUser User { get; set; }
        public long ChatId { get; set; }
        public AuthStep AuthStep { get; set; }
        private bool _isAuthorized;
        public bool IsAuthorized { get => CheckAuthorization(); }
        private string _authToken;
        public string AuthToken
        {
            set
            {
                if (value?.Length > 0)
                    _authToken = value;
            }
        }
        public DateTime LoginDatetime { get; set; }
        public DateTime LogoutDatetime { get; set; }


        public Session(long id, long chatId, string? username, DateTime loginDatetime, DateTime logoutDatetime)
            : this(id, chatId, username, "", "", loginDatetime)
        {
            LogoutDatetime = logoutDatetime;
        }

        public Session(long id, long chatId, string login, string password, DateTime loginDatetime) :
            this(id, chatId, "", login, password, loginDatetime)
        {

        }

        private Session(long id, long chatId, string? username, string login, string password, DateTime loginDatetime)
        {
            User = new TgUser(id, username, login, password);
            ChatId = chatId;
            LoginDatetime = loginDatetime;
            AuthStep = AuthStep.None;
            _authToken = "";
        }

        private DateTime GetZeroDateTime() => DateTime.Parse("1753-01-01");

        private bool CheckAuthorization()
        {
            if (_isAuthorized)
                return true;
            if (this.CheckActiveSessionExists())
            {
                this.GetActiveSessionCredentials();
                _isAuthorized = (User.Login?.Length > 0) && (User.Password?.Length > 0);
            }
            return _isAuthorized;
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
            _isAuthorized = (User.Login?.Length > 0) && (User.Password?.Length > 0);
        }

        public void SetSessionAuthorization(string apiToken)
        {
            _isAuthorized = true;
            _authToken = apiToken;
            SaveSession();
        }

        private void SaveSession()
        {
            if (!this.CheckActiveSessionExists())
            {
                LoginDatetime = DateTime.Now;
                LogoutDatetime = GetZeroDateTime();
                this.Save();
            }
        }

        public void EndSession()
        {
            _isAuthorized = false;
            AuthStep = AuthStep.None;
            LogoutDatetime = DateTime.Now;
            this.LogoutSession();
        }

        public override string ToString() =>
            $"UserId: {User.Id}; Username: {User.Name}; ChatId: {ChatId}; SetSessionAuthorization Datetime: {LoginDatetime}";
    }
}
