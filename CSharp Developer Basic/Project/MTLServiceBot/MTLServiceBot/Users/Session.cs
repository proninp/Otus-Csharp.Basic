using MTLServiceBot.Assistants;
using MTLServiceBot.SQL;
using Telegram.Bot.Types;

namespace MTLServiceBot.Users
{
    public class Session
    {
        private static int _defaultAuthAttemtsCount = ConfigRepository.GetAvailabelAuthorizationCount();
        private bool _isAuthorized;

        public TgUser User { get; set; }
        public long ChatId { get; init; }
        public AuthStep AuthStep { get; set; }
        public bool IsAuthorized { get => CheckAuthorization(); }
        public int AvailableAuthorizationAttemts { get; private set; }
        public WorkFlow WorkFlowState { get; set; } // режим работы с одной командой, переключается только в случае, если пришла другая команда
        public string WorkFlowTaskId { get; set; }
        public DateTime LoginDatetime { get; set; }
        public DateTime LogoutDatetime { get; set; }

        public Session(long id, long chatId, string? username, DateTime loginDatetime, DateTime logoutDatetime)
            : this(id, chatId, "", "", loginDatetime)
        {
            User.Name = username ?? "";
            LogoutDatetime = logoutDatetime;
            UpdateAvailableAuthorizationAttempts();
        }

        public Session(long id, long chatId, string login, string password, DateTime loginDatetime)
        {
            User = new TgUser(id, login, password);
            ChatId = chatId;
            LoginDatetime = loginDatetime;
            AuthStep = AuthStep.None;
            WorkFlowTaskId = "";
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
            User.AuthToken = apiToken;
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
            SetDefaultAvailableAuthorizationAttempts();
        }

        private void SetDefaultAvailableAuthorizationAttempts()
        {
            AvailableAuthorizationAttemts = _defaultAuthAttemtsCount;
            this.UpdateDbUserAttemptsCount();
        }

        private void UpdateAvailableAuthorizationAttempts()
        {
            if (!this.CheckAuthAttemptsRecordExists())
                SetDefaultAvailableAuthorizationAttempts();
            else
                AvailableAuthorizationAttemts = this.GetAvailableAuthAttemptsCount();
        }

        public bool CheckAvailableAuthorizationAttempts()
        {
            UpdateAvailableAuthorizationAttempts();
            return AvailableAuthorizationAttemts > 0;
        }

        public void DecreaseAvailableSessions()
        {
            AvailableAuthorizationAttemts--;
            this.UpdateDbUserAttemptsCount();
        }

        public override string ToString() =>
            $"UserId: {User.Id}; Username: {User.Name}; ChatId: {ChatId}; SetSessionAuthorization Datetime: {LoginDatetime}";
    }
}
