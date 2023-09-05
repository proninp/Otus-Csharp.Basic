using System.Runtime.InteropServices.JavaScript;
using System.Xml.Linq;

namespace MTLServiceBot
{
    public class AppConfig
    {
        
        public static readonly string? BotToken = Environment.GetEnvironmentVariable("MTL_TG_BOT_TOKEN");       
        private static readonly string? DbHost = Environment.GetEnvironmentVariable("DB_HOST");
        private static readonly string? DbName = Environment.GetEnvironmentVariable("DB_NAME");
        private static readonly string? DbUser = Environment.GetEnvironmentVariable("DB_USER");
        private static readonly string? DbPass = Environment.GetEnvironmentVariable("DB_PASS");
        public static readonly string EncryptionKey = Environment.GetEnvironmentVariable("ENCRYPTION_KEY") ?? "";
        public static readonly string EncryptionSalt = Environment.GetEnvironmentVariable("ENCRYPTION_SALT") ?? "";
        public static readonly string ConnectionString = $"Data Source={DbHost};Initial Catalog={DbName};User ID={DbUser};" +
            $"Password={DbPass};Integrated Security=SSPI;TrustServerCertificate=True;";
        
        private static readonly string? mainApiUrl = Environment.GetEnvironmentVariable("MTL_MAIN_API_URL");
        public static string AcceptHeader = "application/json";
        public static readonly string? AuthApiUrl = $"{mainApiUrl}/GetST";
        public static readonly string? ServiceTasksApiUrl = $"{mainApiUrl}/ServiceEngineerRequests";
        public static readonly string? SetTaskStatusApiUrl = $"{mainApiUrl}/SetStatus";
        public static readonly string? SetTaskFilesListUrl = $"{mainApiUrl}/ServiceFilesList";
        public static readonly string? AddCommentApiUrl = $"{mainApiUrl}/AddRequestTaskComment";
    }
}
