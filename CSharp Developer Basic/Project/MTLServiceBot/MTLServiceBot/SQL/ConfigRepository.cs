using Dapper;
using Microsoft.Data.SqlClient;
using MTLServiceBot.Assistants;

namespace MTLServiceBot.SQL
{
    public static class ConfigRepository
    {
        public static string GetBotToken()
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = $"SELECT [Bot Token] FROM [dbo].[Tg Application Setup] WHERE [Bot Id] = @botId";
            var token = db.QueryFirstOrDefault<string>(query, new
            {
                botId = GetSetupId()
            });
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException(TextConsts.ConfigRepoTokenError);
            return token;
        }

        public static string GetApiUrl()
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = $"SELECT [API Url] FROM [dbo].[Tg Application Setup] WHERE [Bot Id] = @botId";
            var token = db.QueryFirstOrDefault<string>(query, new
            {
                botId = GetSetupId()
            });
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException(TextConsts.ConfigRepoApiLinkError);
            return token;
        }

        public static string GetDownloadedFilesDirectory()
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = $"SELECT [Tg Files Download Path] FROM [dbo].[Tg Application Setup] WHERE [Bot Id] = @botId";
            var token = db.QueryFirstOrDefault<string>(query, new
            {
                botId = GetSetupId()
            });
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException(TextConsts.ConfigRepoTgFilesError);
            return token;
        }

        public static string GetSharedNetworkDirectory()
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = $"SELECT [Service Files Network Path] FROM [dbo].[Tg Application Setup] WHERE [Bot Id] = @botId";
            var token = db.QueryFirstOrDefault<string>(query, new
            {
                botId = GetSetupId()
            });
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException(TextConsts.ConfigRepoSharedNetworkError);
            return token;
        }

        private static string GetSetupId()
        {
            if (string.IsNullOrEmpty(AppConfig.SetupId))
                throw new InvalidOperationException(TextConsts.ConfigRepoAppSetupIdError);
            return AppConfig.SetupId;
        }
    }
}
