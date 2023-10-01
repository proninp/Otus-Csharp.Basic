using Dapper;
using HomeWork16.DTO;
using Npgsql;
using System.Text;

namespace HomeWork16.Repositories
{
    public class Customers
    {
        private readonly NpgsqlConnection _db;

        public Customers(NpgsqlConnection db) =>        
            _db = db;

        public List<Customer>? GetCustomers()
        {
            var query = new StringBuilder("SELECT * FROM \"Customers\"");
            return _db.Query<Customer>(query.ToString()).ToList();
        }

        public Customer? GetCustomerById(int id)
        {
            var query = new StringBuilder("SELECT * FROM \"Customers\" WHERE \"ID\" = @id");
            return _db.QueryFirstOrDefault<Customer?>(query.ToString(), new { id });
        }
    }
}
