namespace Program2
{
    internal class Program
    {
        private static readonly string[] planets = { "Земля", "Лимония", "Марс" };

        static void Main(string[] args)
        {
            var catalogue = new PlanetCatalogue();
            var tuples = planets.Select(x => catalogue.GetPlanet(x)).ToArray();
            for (int i = 0; i < tuples.Length; i++)
            {   
                if (!string.IsNullOrEmpty(tuples[i].errorMessage))
                    Console.WriteLine(tuples[i].errorMessage); 
                else
                    Console.WriteLine($"Название: {planets[i]}, Номер от Солнца: {tuples[i].numberFromSun}, Длина экватора: {tuples[i].equatorLength}");
            }
        }
    }
}