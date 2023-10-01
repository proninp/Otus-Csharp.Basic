using Dapper;
using HomeWork16.DTO;
using Npgsql;
using System.Text;

namespace HomeWork16.Repositories
{
    public class Orders
    {
        private readonly NpgsqlConnection _db;

        public Orders(NpgsqlConnection db) =>
            _db = db;

        public List<Order>? GetOrders()
        {
            var query = new StringBuilder("SELECT * FROM \"Orders\"");
            return _db.Query<Order>(query.ToString()).ToList();
        }

        public Order? GetOrderById(int id)
        {
            var query = new StringBuilder("SELECT * FROM \"Orders\" WHERE \"ID\" = @id");
            return _db.QueryFirstOrDefault<Order?>(query.ToString(), new { id });
        }
    }
}
