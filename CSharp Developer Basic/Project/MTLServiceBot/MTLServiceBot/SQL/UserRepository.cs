using Dapper;
using Microsoft.Data.SqlClient;
using MTLServiceBot.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTLServiceBot.SQL
{
    public class UserRepository
    {
        public static int GetUsersQty()
        {
            var query = "SELECT COUNT(*) FROM [dbo].[Telegram Users]";
            using var db = new SqlConnection(AppConfig.ConnectionString);    
            return db.QueryFirstOrDefault<int>(query);
        }
        public static List<User> GetUsers(bool onlyActive = true)
        {
            StringBuilder query = new StringBuilder("SELECT [Id] id, [Name] name, [Login] login, [Password] password, [Is Active] isActive, [Is Admin] isAdmin " +
                "FROM [dbo].[Telegram Users]");
            if (onlyActive)
                query.Append(" WHERE [Is Active] = 1");
            using var db = new SqlConnection(AppConfig.ConnectionString);
            return db.Query<User>(query.ToString()).ToList();
        }
    }
}
