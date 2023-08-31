using System.Net.Http.Headers;
using System.Text;

namespace MTLServiceBot.Users
{
    public class User
    {
        public int Id { get; }
        public string? Name { get; init; }
        public string? Login { get; init; }
        public string? Password { get; set; }
        public bool IsActive 
        { 
            get => _isActive != 0;
            set
            {
                _isActive = value ? (byte)1 : (byte)0;
            }
        }
        public bool IsAdmin
        {
            get => _isAdmin != 0;
            set
            {
                _isAdmin = value ? (byte)1 : (byte)0;
            }
        }
        private string? _authToken;
        private byte _isActive = 0;
        private byte _isAdmin = 0;
        public User(int id, string name = "", string login = "", string password = "", byte isActive = 0, byte isAdmin = 0)
        {
            Id = id;
            Name = name;
            Login = login;
            Password = password;
            _isActive = isActive;
            _isAdmin = isAdmin;
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
            $"User: {Name}; Login: {Login}; Passord: {Password}; Is Active: {IsActive}; Is Admin: {IsAdmin}";

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
