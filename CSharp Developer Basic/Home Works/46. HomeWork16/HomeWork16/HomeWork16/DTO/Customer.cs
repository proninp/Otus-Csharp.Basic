namespace HomeWork16.DTO
{
    public class Customer
    {
        public int ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }

        public override string ToString() =>
            $"Customer ID - {ID}, FirstName - {FirstName}, LastName - {LastName}, Age - {Age}";
    }
}
