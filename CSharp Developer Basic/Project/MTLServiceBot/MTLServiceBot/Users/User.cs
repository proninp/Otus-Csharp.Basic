using System.Net.Http.Headers;
using System.Text;

namespace MTLServiceBot.Users
{
    public class User
    {
        
        public string? UserName { get; set; }
        private string? _authLogin;
        private string? _authPassword;
        private string? _authToken;
        
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public User(string? login = "", string userName = "", bool isAdmin = false, bool isActive = true)
        {
            _authLogin = login;
            UserName = userName;
            IsActive = isActive;
            IsAdmin = isAdmin;
        }
        public string GetAuthUserPasswordValue() => GetAuthValue($"{_authLogin}:{_authPassword}");

        public string GetAuthTokenValue() => GetAuthValue($"{_authLogin}:{_authToken}");

        private string GetAuthValue(string credentials)
        {
            if (string.IsNullOrEmpty(credentials))
                return "";
            if (string.IsNullOrEmpty(credentials))
                return "";
            var credBytes = Encoding.ASCII.GetBytes(credentials);
            var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credBytes));
            return authHeader.ToString();
        }

        #region Тестовые функции, которые нужно вынести в конструктор и убрать
        public void SetAuthPasswordTest(string? authPsw)
        {
            _authPassword = authPsw;
        }
        public void SetAuthTokenTest(string? authToken)
        {
            _authToken = authToken;
        }
        #endregion
    }
}
