using System.Net.Http.Headers;
using System.Text;

namespace MTLServiceBot.Users
{
    public class TgUser
    {
        public long Id { get; }
        public string Name { get; init; }
        public string? Login { get; init; }
        public string? Password { get; set; }
        private string? _authToken;
        public TgUser(long id, string? name = "", string login = "", string password = "")
        {
            Id = id;
            Name = name ?? "";
            Login = login;
            Password = password;
        }

        public string GetAuthUserPasswordValue() => GetAuthValue($"{Login}:{Password}");

        public string GetAuthTokenValue() => GetAuthValue($"{Login}:{_authToken}");

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

        public override string ToString() =>
            $"User: {Name}; Login: {Login}; Passord: {Password}";

        #region Тестовые функции, которые нужно вынести в конструктор и убрать
        public void SetAuthPasswordTest(string? authPsw)
        {
            Password = authPsw;
        }
        public void SetAuthTokenTest(string? authToken)
        {
            _authToken = authToken;
        }
        #endregion

        
    }
}
