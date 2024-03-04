using Microsoft.Extensions.Configuration;
using MTLServiceBot.SQL;
using System.Reflection;

namespace MTLServiceBot;

public class AppConfig
{
    private static readonly Lazy<AppConfig> instance = new Lazy<AppConfig>(() => new AppConfig());

    public static AppConfig Instance { get => instance.Value; }

#if DEBUG
    private static readonly string? DbHost = Environment.GetEnvironmentVariable("DB_HOST");
    private static readonly string? DbName = Environment.GetEnvironmentVariable("DB_NAME");
    private static readonly string? DbUser = Environment.GetEnvironmentVariable("DB_USER");
    private static readonly string? DbPass = Environment.GetEnvironmentVariable("DB_PASS");

    public static readonly string ImportantLogsFile = "ImportantLogs.Dev.json";
    public static readonly string RegularLogsFile = "All.Dev.logs";
#else
    private static readonly string? DbHost = Environment.GetEnvironmentVariable("DB_HOST_PROD");
    private static readonly string? DbName = Environment.GetEnvironmentVariable("DB_NAME_PROD");
    private static readonly string? DbUser = Environment.GetEnvironmentVariable("DB_USER_PROD");
    private static readonly string? DbPass = Environment.GetEnvironmentVariable("DB_PASS_PROD");

    public static readonly string ImportantLogsFile = "ImportantLogs.Prod.json";
    public static readonly string RegularLogsFile = "All.Prod.logs";
#endif

    public static readonly string ConnectionString = $"Data Source={DbHost};Initial Catalog={DbName};User ID={DbUser};" +
        $"Password={DbPass};Integrated Security=SSPI;TrustServerCertificate=True;";

    public static readonly string? SetupId = Environment.GetEnvironmentVariable("MTL_S_BOT_ID");

    #region public const
    public const string ConfigRepoTokenError = "Необходимо указать токен в таблице настроек Telegram бота";
    public const string ConfigRepoApiLinkError = "Необходимо указать адрес API в таблице настроек Telegram бота";
    public const string ConfigRepoTgFilesError = "Необходимо указать директорию для скачивания файлов в таблице настроек Telegram бота";
    public const string ConfigRepoSharedNetworkError = "Необходимо указать сетевую директорию для размещения файлов в таблице настроек Telegram бота";
    public const string ConfigRepoAvailAuthCountError = "Необходимо указать допустимое количество попыток авторизации в таблице настроек Telegram бота";
    public const string ConfigRepoNetworkLoginError = "Необходимо указать логин для авторизации на сетевом ресурсе в таблице настроек Telegram бота";
    public const string ConfigRepoNetworkPswError = "Необходимо указать пароль для авторизации на сетевом ресурсе в таблице настроек Telegram бота";
    public const string ConfigRepoAppSetupIdError = "Необходимо указать Id настройки бота";
    #endregion


    #region Public Fields
    public string BotId { get; }
    
    public string EncryptionKey { get; }
    public string EncryptionSalt { get; }
    public string SingleTaskNumberFormatSeparator { get; }
    public string CBCmdDataSeparator { get; }
    public string CBCmdChangeStatus { get; }
    public string CBCmdAddFile { get; }

    public string MainApiUrl { get; }
    public string AuthApiUrl { get; }
    public string ServiceTasksApiUrl { get; }
    public string ServiceTaskApiUrl { get; }
    public string SetTaskStatusApiUrl { get; }
    public string AddFileApiUrl { get; }
    public string AddNetworkFileApiUrl { get; }
    public string GetTaskFilesListUrl { get; }
    public string AddCommentApiUrl { get; }    
    public string OtpgenApiUrl { get; }
    public string AcceptHeaderName { get; }
    public string AcceptHeaderValue { get; }
    public string AuthHeaderName { get; }

    public string StartCommandName { get; }
    public string StopCommandName { get; }
    public string LoginCommandName { get; }
    public string LogoutCommandName { get; }
    public string ServiceTasksCommandName { get; }
    public string OtpgenCommandName { get; }
    public string HelpCommandName { get; }
    public string UnknownCommandName { get; }

    public string StartCommandDescription { get; }
    public string StopCommandDescription { get; }
    public string LoginCommandDescription { get; }
    public string LogoutCommandDescription { get; }
    public string ServiceCommandDescription { get; }
    public string OtpgenCommandDescription { get; }
    public string HelpCommandDescription { get; }
    public string UnknownCommandDescription { get; }
    public string UnknownCommandFullDescription { get; }
    public string StopCommandMsg { get; }

    public string EnterLogin { get; }
    public string EnterPassword { get; }
    public string LoginEmptyError { get; }
    public string PasswordEmptyError { get; }
    public string LoginSuccessMsg { get; }
    public string LoginAlreadyAuthorized { get; }
    public string AvailableAuthAttemptsPhrase { get; }
    public string LoginNoAuthorizedAttempts { get; }
    public string AuthorizationRequired { get; }
    public string LogoutUnathorized { get; }
    public string LogoutSuccess { get; }

    public string UpdateFailed { get; }
    public string UpdateTypeUnknownLog { get; }
    public string UpdateFromUnknown { get; }
    public string UpdateTypeDataUnknown { get; }
    public string UpdateMessageTypeError { get; }
    public string UpdateMessageFromBotError { get; }
    public string UpdateNewReceivingLog { get; }
    public string UpdateCommandExceptionTemplate { get; }

    public string ServerConnectionError { get; }
    public string UnauthorizedError { get; }
    public string DeserializeJsonError { get; }

    public string ServiceTasksListEmpty { get; }
    public string ChooseServiceRequestBtn { get; }
    public string SingleServiceRequestUpdateFailureMsg { get; }
    public string ServiceTasksWorkflowIncorrectFormat { get; }
    public string ServiceTasksWorkflowNotFound { get; }

    public string CBCmdAddFileDescription { get; }
    public string CBCmdAddFileCallMsgDescription { get; }
    public string CBCmdAddFileCallHandlerError { get; }
    public string CBCmdAddFileCallHandlerFileError { get; }
    public string AddFileHandleIdError { get; }
    public string AddFileHandleReceiveError { get; }
    public string AddFileHandleCopyError { get; }
    public string AddFileHandleAddedMsg { get; }

    public string CBCmdDataEmpty { get; }
    public string CBCmdDataUndefined { get; }
    public string CBCmdDataNotRecognized { get; }
    public string CBCmdServiceTaskNotFound { get; }

    public string LogNotificationDescription { get; }
    public string LogDescription { get; }
    public string EncryptionKeyRequiredError { get; }
    public string EncryptionSaltRequiredError { get; }
    #endregion

    private AppConfig()
    {
        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        string settingsPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\Settings"));

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(settingsPath)
            .AddJsonFile("AppSettings.json")
            .AddJsonFile("ApiSettings.json")
            .AddJsonFile("CommandsSettings.json")
            .AddJsonFile("AuthInteractionSettings.json")
            .AddJsonFile("UserInteractionSettings.json")
            .Build();

        #region Fields Initialization
        BotId = configuration["BotId"] ?? "";
        EncryptionKey = configuration["EncryptionKey"] ?? "";
        EncryptionSalt = configuration["EncryptionSalt"] ?? "";
        SingleTaskNumberFormatSeparator = configuration["SingleTaskNumberFormatSeparator"] ?? "";
        CBCmdDataSeparator = configuration["CBCmdDataSeparator"] ?? "";
        CBCmdChangeStatus = configuration["CBCmdChangeStatus"] ?? "";
        CBCmdAddFile = configuration["CBCmdAddFile"] ?? "";

        MainApiUrl = ConfigRepository.GetApiUrl();
        AuthApiUrl = string.Format(configuration["AuthApiUrl"] ?? "", MainApiUrl);
        ServiceTasksApiUrl = string.Format(configuration["ServiceTasksApiUrl"] ?? "", MainApiUrl);
        ServiceTaskApiUrl = MainApiUrl + configuration["ServiceTaskApiUrl"] ?? "";
        SetTaskStatusApiUrl = string.Format(configuration["SetTaskStatusApiUrl"] ?? "", MainApiUrl);
        AddFileApiUrl = string.Format(configuration["AddFileApiUrl"] ?? "", MainApiUrl);
        AddNetworkFileApiUrl = string.Format(configuration["AddNetworkFileApiUrl"] ?? "", MainApiUrl);
        GetTaskFilesListUrl = string.Format(configuration["GetTaskFilesListUrl"] ?? "", MainApiUrl);
        AddCommentApiUrl = string.Format(configuration["AddCommentApiUrl"] ?? "", MainApiUrl);
        OtpgenApiUrl = string.Format(configuration["OtpgenApiUrl"] ?? "", MainApiUrl);
        AcceptHeaderName = configuration["AcceptHeaderName"] ?? "";
        AcceptHeaderValue = configuration["AcceptHeaderValue"] ?? "";
        AuthHeaderName = configuration["AuthHeaderName"] ?? "";

        StartCommandName = configuration["StartCommandName"] ?? "";
        StopCommandName = configuration["StopCommandName"] ?? "";
        LoginCommandName = configuration["LoginCommandName"] ?? "";
        LogoutCommandName = configuration["LogoutCommandName"] ?? "";
        ServiceTasksCommandName = configuration["ServiceTasksCommandName"] ?? "";
        OtpgenCommandName = configuration["OtpgenCommandName"] ?? "";
        HelpCommandName = configuration["HelpCommandName"] ?? "";
        UnknownCommandName = configuration["UnknownCommandName"] ?? "";

        StartCommandDescription = configuration["StartCommandDescription"] ?? "";
        StopCommandDescription = configuration["StopCommandDescription"] ?? "";
        LoginCommandDescription = configuration["LoginCommandDescription"] ?? "";
        LogoutCommandDescription = configuration["LogoutCommandDescription"] ?? "";
        ServiceCommandDescription = configuration["ServiceCommandDescription"] ?? "";
        OtpgenCommandDescription = configuration["OtpgenCommandDescription"] ?? "";
        HelpCommandDescription = configuration["HelpCommandDescription"] ?? "";
        UnknownCommandDescription = configuration["UnknownCommandDescription"] ?? "";
        UnknownCommandFullDescription = string.Format(configuration["UnknownCommandFullDescription"] ?? "", HelpCommandName);
        StopCommandMsg = configuration["StopCommandMsg"] ?? "";

        LoginEmptyError = configuration["LoginEmptyError"] ?? "";
        PasswordEmptyError = configuration["PasswordEmptyError"] ?? "";
        LoginSuccessMsg = configuration["LoginSuccessMsg"] ?? "";
        LoginAlreadyAuthorized = configuration["LoginAlreadyAuthorized"] ?? "";
        AvailableAuthAttemptsPhrase = configuration["AvailableAuthAttemptsPhrase"] ?? "";
        LoginNoAuthorizedAttempts = configuration["LoginNoAuthorizedAttempts"] ?? "";
        AuthorizationRequired = configuration["AuthorizationRequired"] ?? "";
        LogoutUnathorized = configuration["LogoutUnathorized"] ?? "";
        LogoutSuccess = configuration["LogoutSuccess"] ?? "";
        EnterLogin = string.Format(configuration["EnterLogin"] ?? "", AvailableAuthAttemptsPhrase);
        EnterPassword = configuration["EnterPassword"] ?? "";

        UpdateFailed = configuration["UpdateFailed"] ?? "";
        UpdateTypeUnknownLog = configuration["UpdateTypeUnknownLog"] ?? "";
        UpdateFromUnknown = configuration["UpdateFromUnknown"] ?? "";
        UpdateTypeDataUnknown = configuration["UpdateTypeDataUnknown"] ?? "";
        UpdateMessageTypeError = configuration["UpdateMessageTypeError"] ?? "";
        UpdateMessageFromBotError = configuration["UpdateMessageFromBotError"] ?? "";
        UpdateNewReceivingLog = configuration["UpdateNewReceivingLog"] ?? "";
        UpdateCommandExceptionTemplate = configuration["UpdateCommandExceptionTemplate"] ?? "";

        ServerConnectionError = configuration["ServerConnectionError"] ?? "";
        UnauthorizedError = configuration["UnauthorizedError"] ?? "";
        DeserializeJsonError = configuration["DeserializeJsonError"] ?? "";

        ServiceTasksListEmpty = configuration["ServiceTasksListEmpty"] ?? "";
        ChooseServiceRequestBtn = configuration["ChooseServiceRequestBtn"] ?? "";
        SingleServiceRequestUpdateFailureMsg = configuration["SingleServiceRequestUpdateFailureMsg"] ?? "";
        ServiceTasksWorkflowIncorrectFormat = string.Format(configuration["ServiceTasksWorkflowIncorrectFormat"] ?? "",
            configuration["STWFIncorrectFormatEx1"],
            configuration["STWFIncorrectFormatEx2"],
            string.Format(configuration["STWFIncorrectFormatEx3"] ?? "", SingleTaskNumberFormatSeparator),
            configuration["STWFIncorrectFormatEx4"]);
        ServiceTasksWorkflowNotFound = string.Format(configuration["ServiceTasksWorkflowNotFound"] ?? "",
            configuration["ServiceTasksWorkflowNotFoundEx1"],
            configuration["ServiceTasksWorkflowNotFoundEx2"],
            configuration["ServiceTasksWorkflowNotFoundEx3"]);

        CBCmdAddFileDescription = configuration["CBCmdAddFileDescription"] ?? "";
        CBCmdAddFileCallMsgDescription = configuration["CBCmdAddFileCallMsgDescription"] ?? "";
        CBCmdAddFileCallHandlerError = configuration["CBCmdAddFileCallHandlerError"] ?? "";
        CBCmdAddFileCallHandlerFileError = string.Format(configuration["CBCmdAddFileCallHandlerFileError"] ?? "",
            configuration["CBCmdAddFileCallHandlerFileErrorEx1"],
            configuration["CBCmdAddFileCallHandlerFileErrorEx2"]);
        AddFileHandleIdError = configuration["AddFileHandleIdError"] ?? "";
        AddFileHandleReceiveError = configuration["AddFileHandleReceiveError"] ?? "";
        AddFileHandleCopyError = configuration["AddFileHandleCopyError"] ?? "";
        AddFileHandleAddedMsg = configuration["AddFileHandleAddedMsg"] ?? "";

        CBCmdDataEmpty = configuration["CBCmdDataEmpty"] ?? "";
        CBCmdDataUndefined = configuration["CBCmdDataUndefined"] ?? "";
        CBCmdDataNotRecognized = configuration["CBCmdDataNotRecognized"] ?? "";
        CBCmdServiceTaskNotFound = string.Format(configuration["CBCmdServiceTaskNotFound"] ?? "",
            configuration["CBCmdServiceTaskNotFoundEx1"],
            string.Format(configuration["CBCmdServiceTaskNotFoundEx2"] ?? "", ServiceTasksCommandName));

        LogNotificationDescription = configuration["LogNotificationDescription"] ?? "";
        LogDescription = configuration["LogDescription"] ?? "";
        EncryptionKeyRequiredError = configuration["EncryptionKeyRequiredError"] ?? "";
        EncryptionSaltRequiredError = configuration["EncryptionSaltRequiredError"] ?? "";
        #endregion
    }
    
}
