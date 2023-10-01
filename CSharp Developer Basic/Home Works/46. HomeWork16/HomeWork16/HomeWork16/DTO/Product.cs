namespace HomeWork16.DTO
{
    public class Product
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        
        public override string ToString() =>
            $"Product ID - {ID}, Name - {Name}, Description - {Description}, StockQuantity - {StockQuantity}, Price - {Price}";
    }
}
