namespace Task1
{
    public class Customer: IObserve
    {
        public string? Name { get; set; }

        public Customer(string name)
        {
            Name = name;
        }

        public void OnItemChanged(string message)
        {
            Console.WriteLine($"Событие магазина {Name}. {message}");
        }
    }
}
