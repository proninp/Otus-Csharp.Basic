namespace Program01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var venus1 = new
            {
                Name = "Венера",
                OrdrNumber = (short)2,
                EquatorLength = 38_025,
                PreviousPlanet = null as object
            };
            var earth = new
            {
                Name = "Земля",
                OrdrNumber = (short)3,
                EquatorLength = 40_075,
                PreviousPlanet = venus1
            };
            var mars = new
            {
                Name = "Марс",
                OrdrNumber = (short)4,
                EquatorLength = 21_344,
                PreviousPlanet = earth
            };
            var venus2 = new
            {
                Name = "Венера",
                OrdrNumber = (short)2,
                EquatorLength = 38_025,
                PreviousPlanet = mars
            };

            string separator = "\n" + new string('-', 100) + "\n";
            
            Console.WriteLine($"{venus1}. Экввалентна Венере = {venus1.Equals(venus1)}");
            Console.WriteLine(separator);
            Console.WriteLine($"{earth}. Экввалентна Венере = {earth.Equals(venus1)}");
            Console.WriteLine(separator);
            Console.WriteLine($"{mars}. Экввалентна Венере = {mars.Equals(venus1)}");
            Console.WriteLine(separator);
            Console.WriteLine($"{venus2}. Экввалентна Венере = {venus2.Equals(venus1)}");
        }
    }
}