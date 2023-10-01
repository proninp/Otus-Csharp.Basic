using HomeWork16.Repositories;
using Npgsql;

namespace HomeWork16
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var db = new NpgsqlConnection(AppConfig.DbConnectionString);

            ColoredPrint("********** Customers **********");
            var customers = new Customers(db);
            var customersList = customers.GetCustomers();
            customersList?.ForEach(Console.WriteLine);
            Console.WriteLine(customers.GetCustomerById(1));

            ColoredPrint("********** Products **********");
            var products = new Products(db);
            var productsList = products.GetProducts();
            productsList?.ForEach(Console.WriteLine);
            Console.WriteLine(products.GetProductById(2));

            ColoredPrint("********** Orders **********");
            var orders = new Products(db);
            var ordersList = products.GetProducts();
            ordersList?.ForEach(Console.WriteLine);
            Console.WriteLine(orders.GetProductById(3));
        }

        public static void ColoredPrint(string text, ConsoleColor color = ConsoleColor.DarkCyan)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;
        }
    }
}