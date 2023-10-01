using Dapper;
using HomeWork16.DTO;
using Npgsql;
using System.Text;

namespace HomeWork16.Repositories
{
    public class Products
    {
        private readonly NpgsqlConnection _db;

        public Products(NpgsqlConnection db) =>
            _db = db;

        public List<Product>? GetProducts()
        {
            var query = new StringBuilder("SELECT * FROM \"Products\"");
            return _db.Query<Product>(query.ToString()).ToList();
        }

        public Product? GetProductById(int id)
        {
            var query = new StringBuilder("SELECT * FROM \"Products\" WHERE \"ID\" = @id");
            return _db.QueryFirstOrDefault<Product?>(query.ToString(), new { id });
        }
    }
}
