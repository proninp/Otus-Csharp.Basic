using Dapper;
using Microsoft.Data.SqlClient;

namespace MTLServiceBot.SQL
{
    public static class ConfigRepository
    {
        public static string GetBotToken()
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = $"SELECT [Bot Token] FROM [dbo].[Tg Application Setup] WHERE [Bot Id] = '{GetSetupId()}'";
            var token = db.QueryFirstOrDefault<string>(query);
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("Необходимо указать токен в таблице настроек Telegram бота.");
            return token;
        }
        public static string GetApiUrl()
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = $"SELECT [API Url] FROM [dbo].[Tg Application Setup] WHERE [Bot Id] = '{GetSetupId()}'";
            var token = db.QueryFirstOrDefault<string>(query);
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("Необходимо указать адрес API в таблице настроек Telegram бота.");
            return token;
        }
        private static string GetSetupId()
        {
            if (string.IsNullOrEmpty(AppConfig.SetupId))
                throw new InvalidOperationException("Необходимо указать Id настройки бота");
            return AppConfig.SetupId;
        }
    }
}
