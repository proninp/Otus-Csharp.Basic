using Dapper;
using Microsoft.Data.SqlClient;
using MTLServiceBot.Assistants;
using MTLServiceBot.Users;
using System.Text;

namespace MTLServiceBot.SQL
{
    public static class SessionRepository
    {
        public static int GetActiveSessionsQty()
        {
            var query = "SELECT COUNT(*) FROM [dbo].[Tg User Sessions] WHERE [Logout Datetime] = '1753-01-01 00:00:00.000'";
            using var db = new SqlConnection(AppConfig.ConnectionString);
            return db.QueryFirstOrDefault<int>(query);
        }
        public static bool CheckActiveSessionExists(this Session userSession)
        {
            var query = new StringBuilder();
            query.Append("SELECT COUNT(*) FROM [dbo].[Tg User Sessions] WHERE");
            query.Append(" [User Id] = @userID");
            query.Append(" AND [Chat Id] = @chatId");
            query.Append(" AND [Logout Datetime] = '1753-01-01 00:00:00.000'");
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var cnt = db.QueryFirstOrDefault<int>(query.ToString(), new
            {
                userId = userSession.User.Id,
                chatId = userSession.ChatId
            });
            return cnt > 0;
        }
        public static void GetActiveSessionCredentials(this Session userSession)
        {
            var query = new StringBuilder();
            query.Append("SELECT TOP (1) [User Id] id, [Chat Id] chatId, [Login] login, [Password Cipher] password, [Login Datetime] loginDatetime");
            query.Append(" FROM [dbo].[Tg User Sessions]");
            query.Append(" WHERE [User Id] = @userId");
            query.Append(" AND [Chat Id] = @chatId");
            query.Append(" AND [Logout Datetime] = '1753-01-01 00:00:00.000'");
            query.Append(" ORDER BY [Login Datetime] DESC");
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var session = db.QueryFirstOrDefault<Session>(query.ToString(), new
            {
                userId = userSession.User.Id,
                chatId = userSession.ChatId
            });
            userSession.SetCredentials(session);
        }
        public static List<Session> GetActiveSessions()
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [User Id] id, [Chat Id] chatId, [Login] login, [Password Cipher] password, [Login Datetime] loginDatetime");
            query.Append(" FROM [dbo].[Tg User Sessions]");
            query.Append(" WHERE [Logout Datetime] = '1753-01-01 00:00:00.000'");
            return db.Query<Session>(query.ToString()).ToList();
        }
        public static void Save(this Session userSession)
        {
            var query = new StringBuilder();
            query.Append("INSERT INTO[dbo].[Tg User Sessions]");
            query.Append(" ([User Id], [Chat Id], [Login], [Password Cipher], [Login Datetime], [Logout Datetime])");
            query.Append(" VALUES (@id, @chatId, @login, @password, @loginDatetime, @logoutDatetime)");
            using var db = new SqlConnection(AppConfig.ConnectionString);
            db.Execute(query.ToString(), new
            {
                id = userSession.User.Id,
                chatId = userSession.ChatId,
                login = userSession.User.Login,
                password = EncryptionHelper.Encrypt(userSession.User.Password, userSession.User.Id.ToString(), userSession.ChatId.ToString()),
                loginDatetime = userSession.LoginDatetime,
                logoutDatetime = userSession.LogoutDatetime
            });
        }
        public static void LogoutSession(this Session userSession)
        {
            if (!CheckActiveSessionExists(userSession))
                return;
            StringBuilder query = new StringBuilder();
            query.Append("UPDATE [dbo].[Tg User Sessions] SET");
            query.Append($" [Logout Datetime] = @logoutDatetime");
            query.Append($" WHERE [User Id] = @id AND [Chat Id] = @chatId AND [Logout Datetime] = '1753-01-01 00:00:00.000'");
            using var db = new SqlConnection(AppConfig.ConnectionString);
            db.Execute(query.ToString(), new
            {
                id = userSession.User.Id,
                chatId = userSession.ChatId,
                logoutDatetime = userSession.LogoutDatetime
            });
        }
    }
}
