namespace HomeWork13
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var listPart = list.Top(30);
            Console.WriteLine(string.Join(", ", listPart));

            var persons = new List<Person>()
            {
                new Person("Bjarne", 72),
                new Person("James", 68),
                new Person("Bill", 67),
                new Person("Linus", 53),
                new Person("Guido", 67),
                new Person("Brendan", 62)
            };
            var personsPart = persons.Top(50, p => p.Age);
            Console.WriteLine(string.Join("\n", personsPart));
        }
    }
}