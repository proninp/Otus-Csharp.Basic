namespace HomeWork16.DTO
{
    public class Order
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        public override string ToString() =>
            $"Order ID - {ID}, CustomerID - {CustomerID}, ProductID - {ProductID}, Quantity - {Quantity}";
    }
}
