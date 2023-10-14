using Dapper;
using Microsoft.Data.SqlClient;
using MTLServiceBot.Assistants;
using System.Text;

namespace MTLServiceBot.SQL
{
    public static class ConfigRepository
    {
        public static string GetBotToken()
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = $"SELECT [Bot Token] FROM [dbo].[Tg Bot Application Setup] WHERE [Bot Id] = @botId";
            var token = db.QueryFirstOrDefault<string>(query, new
            {
                botId = GetSetupId()
            });
            return GetCheckedSetupFieldValue(token, TextConsts.ConfigRepoTokenError);
        }

        public static string GetApiUrl()
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = $"SELECT [API Url] FROM [dbo].[Tg Bot Application Setup] WHERE [Bot Id] = @botId";
            var apiUrl = db.QueryFirstOrDefault<string>(query, new
            {
                botId = GetSetupId()
            });
            return GetCheckedSetupFieldValue(apiUrl, TextConsts.ConfigRepoApiLinkError);
        }

        public static string GetDownloadedFilesDirectory()
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = $"SELECT [Telegram Files Download Path] FROM [dbo].[Tg Bot Application Setup] WHERE [Bot Id] = @botId";
            var localPath = db.QueryFirstOrDefault<string>(query, new
            {
                botId = GetSetupId()
            });
            return GetCheckedSetupFieldValue(localPath, TextConsts.ConfigRepoTgFilesError);
        }

        public static string GetSharedNetworkDirectory()
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = $"SELECT [Service Files Network Path] FROM [dbo].[Tg Bot Application Setup] WHERE [Bot Id] = @botId";
            var networkPath = db.QueryFirstOrDefault<string>(query, new
            {
                botId = GetSetupId()
            });
            return GetCheckedSetupFieldValue(networkPath, TextConsts.ConfigRepoSharedNetworkError);
        }

        public static int GetAvailabelAuthorizationCount()
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = $"SELECT [User Authorization Attempts] FROM [dbo].[Tg Bot Application Setup] WHERE [Bot Id] = @botId";
            var attemptsCount = db.QueryFirstOrDefault<int>(query, new
            {
                botId = GetSetupId()
            });
            if (attemptsCount == 0)
                throw new InvalidOperationException(TextConsts.ConfigRepoAvailAuthCountError);
            return attemptsCount;
        }

        public static (string login, string pswCipher) GetNetworkAccessCredentials()
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = $"SELECT [Network Access User], [Network Access Password Cipher] FROM [dbo].[Tg Bot Application Setup] WHERE [Bot Id] = @botId";
            var credentials = db.QueryFirstOrDefault<(string login, string pswCipher)>(query, new
            {
                botId = GetSetupId()
            });
            return (GetCheckedSetupFieldValue(credentials.login, TextConsts.ConfigRepoNetworkLoginError), 
                GetCheckedSetupFieldValue(credentials.pswCipher, TextConsts.ConfigRepoNetworkPswError));
        }

        public static bool GetSendAsFileSetup()
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = "SELECT [Send Files as Base64 API] FROM [dbo].[Tg Bot Application Setup] WHERE [Bot Id] = @botId";

            var isSendAsFile = db.QueryFirstOrDefault<bool>(query, new
            {
                botId = GetSetupId()
            });
            return isSendAsFile;
        }

        private static string GetSetupId()
        {
            return GetCheckedSetupFieldValue(AppConfig.SetupId ?? "", TextConsts.ConfigRepoAppSetupIdError);
        }

        private static string GetCheckedSetupFieldValue(string value, string errorMsg)
        {
            if (string.IsNullOrEmpty(value))
                throw new InvalidOperationException(errorMsg);
            return value!;
        }
    }
}
