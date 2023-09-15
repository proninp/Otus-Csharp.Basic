﻿using Dapper;
using Microsoft.Data.SqlClient;
using MTLServiceBot.Users;
using System.Text;

namespace MTLServiceBot.SQL
{
    public static class SessionRepository
    {
        public static int GetActiveSessionsQty()
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = "SELECT COUNT(*) FROM [dbo].[Tg User Sessions] WHERE [Logout Datetime] = '1753-01-01 00:00:00.000'";
            return db.QueryFirstOrDefault<int>(query);
        }
        public static bool CheckActiveSessionExists(this Session userSession)
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = new StringBuilder();
            query.Append("SELECT COUNT(*) FROM [dbo].[Tg User Sessions] WHERE");
            query.Append($" [User Id] = {userSession.User.Id}");
            query.Append($" AND [Chat Id] = {userSession.ChatId}");
            query.Append(" AND [Logout Datetime] = '1753-01-01 00:00:00.000'");
            return db.QueryFirstOrDefault<int>(query.ToString()) > 0;
        }
        public static void GetActiveSessionCredentials(this Session userSession)
        {
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = new StringBuilder();
            query.Append("SELECT TOP (1) [User Id] id, [Chat Id] chatId, [Login] login, [Password Cipher] password, [Login Datetime] loginDatetime WHERE");
            query.Append($" [User Id] = {userSession.User.Id}");
            query.Append($" AND [Chat Id] = {userSession.ChatId}");
            query.Append(" AND [Logout Datetime] = '1753-01-01 00:00:00.000'");
            query.Append(" ORDER BY [Login Datetime] DESC");
            var session = db.QueryFirstOrDefault<Session>(query.ToString());
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
            using var db = new SqlConnection(AppConfig.ConnectionString);
            var query = new StringBuilder();
            query.Append("INSERT INTO[dbo].[Tg User Sessions]");
            query.Append(" ([User Id], [Chat Id], [Login], [Password Cipher], [Login Datetime], [Logout Datetime])");
            query.Append(" VALUES (@id, @chatId, @login, @password, @loginDatetime, @logoutDatetime)");
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
            using var db = new SqlConnection(AppConfig.ConnectionString);
            StringBuilder query = new StringBuilder();
            query.Append("UPDATE [dbo].[Tg User Sessions] SET");
            query.Append($" [Logout Datetime] = @logoutDatetime");
            query.Append($" WHERE [User Id] = @id AND [Chat Id] = @chatId AND [Logout Datetime] = '1753-01-01 00:00:00.000'");
            db.Execute(query.ToString(), new
            {
                id = userSession.User.Id,
                chatId = userSession.ChatId,
                logoutDatetime = userSession.LogoutDatetime
            });
        }
    }
}