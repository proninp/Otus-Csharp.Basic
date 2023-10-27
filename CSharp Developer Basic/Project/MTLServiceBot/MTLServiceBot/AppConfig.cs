using MTLServiceBot.SQL;

namespace MTLServiceBot
{
    public class AppConfig
    {
        private static readonly string? DbHost = Environment.GetEnvironmentVariable("DB_HOST");
        private static readonly string? DbName = Environment.GetEnvironmentVariable("DB_NAME");
        private static readonly string? DbUser = Environment.GetEnvironmentVariable("DB_USER");
        private static readonly string? DbPass = Environment.GetEnvironmentVariable("DB_PASS");
        public static readonly string ConnectionString = $"Data Source={DbHost};Initial Catalog={DbName};User ID={DbUser};" +
            $"Password={DbPass};Integrated Security=SSPI;TrustServerCertificate=True;";

        public static readonly string? SetupId = Environment.GetEnvironmentVariable("MTL_S_BOT_ID");

        public static string MainApiUrl = ConfigRepository.GetApiUrl();
        public static string AuthApiUrl = $"{MainApiUrl}/GetST";
        public static string ServiceTasksApiUrl = $"{MainApiUrl}/ServiceEngineerRequests";
        public static string ServiceTaskApiUrl = MainApiUrl + "/ServiceEngineerRequestsAll?$filter=Request_No eq '{0}' and Task_No eq '{1}'";
        public static string SetTaskStatusApiUrl = $"{MainApiUrl}/SetStatus";
        public static string AddFileApiUrl = $"{MainApiUrl}/AddServiceFile";
        public static string AddNetworkFileApiUrl = $"{MainApiUrl}/AddTelegramFile";
        public static string GetTaskFilesListUrl = $"{MainApiUrl}/ServiceFilesList";
        public static string AddCommentApiUrl = $"{MainApiUrl}/AddRequestTaskComment";
        public static string OtpgenApiUrl = $"{MainApiUrl}/ChallengeOtp";

        public static string AcceptHeaderName = "Accept";
        public static string AcceptHeaderValue = "application/json";
        public static string AuthHeaderName = "Authorization";
    }
}
