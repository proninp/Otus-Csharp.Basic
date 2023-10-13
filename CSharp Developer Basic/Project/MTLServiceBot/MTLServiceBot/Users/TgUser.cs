using System.Net.Http.Headers;
using System.Text;

namespace MTLServiceBot.Users
{
    public class TgUser
    {
        public long Id { get; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? AuthToken { get; set; }
        
        public TgUser(long id, string? name, string? firstname, string? lastname): this(id, name)
        {
            Firstname = firstname ?? "";
            Lastname = lastname ?? "";
        }
        
        public TgUser(long id, string? name): this(id)
        {
            Name = name ?? "";
        }

        public TgUser(long id, string login = "", string password = "")
        {
            Id = id;
            Login = login;
            Password = password;
            Name = "";
            Firstname = "";
            Lastname = "";
        }

        public string GetAuthUserPasswordValue() => GetAuthValue($"{Login}:{Password}");

        public string GetAuthByTokenValue() => GetAuthValue($"{Login}:{AuthToken}");

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
            $"User: {Name}; SetSessionAuthorization: {Login}";
    }
}
