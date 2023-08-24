namespace MTLServiceBot
{
    public class AppConfig
    {
        public const string AcceptHeader = "application/json";
        public static readonly string BotToken = Environment.GetEnvironmentVariable("MTL_TG_BOT_TOKEN");
        private static readonly string? mainApiUrl = Environment.GetEnvironmentVariable("MTL_MAIN_API_URL");
        public static readonly string? AuthApiUrl = $"{mainApiUrl}/GetST";
        public static readonly string? ServiceTasksApiUrl = $"{mainApiUrl}/ServiceEngineerRequests";
        public static readonly string? SetTaskStatusApiUrl = $"{mainApiUrl}/SetStatus";
        public static readonly string? SetTaskFilesListUrl = $"{mainApiUrl}/ServiceFilesList";
        public static readonly string? AddCommentApiUrl = $"{mainApiUrl}/AddRequestTaskComment";
    }
}
