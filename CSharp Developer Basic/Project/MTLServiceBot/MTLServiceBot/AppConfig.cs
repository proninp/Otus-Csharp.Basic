using System.Runtime.InteropServices.JavaScript;
using System.Xml.Linq;

namespace MTLServiceBot
{
    public class AppConfig
    {
        
        private static readonly string? DbHost = Environment.GetEnvironmentVariable("DB_HOST");
        private static readonly string? DbName = Environment.GetEnvironmentVariable("DB_NAME");
        private static readonly string? DbUser = Environment.GetEnvironmentVariable("DB_USER");
        private static readonly string? DbPass = Environment.GetEnvironmentVariable("DB_PASS");
        public static readonly string? SetupId = Environment.GetEnvironmentVariable("MTL_S_BOT_ID");
        public static readonly string ConnectionString = $"Data Source={DbHost};Initial Catalog={DbName};User ID={DbUser};" +
            $"Password={DbPass};Integrated Security=SSPI;TrustServerCertificate=True;";
        public static string AcceptHeaderName = "Accept";
        public static string AcceptHeaderValue = "application/json";
        public static string AuthHeaderName = "Authorization";
    }
}
