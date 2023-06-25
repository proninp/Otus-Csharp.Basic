using Program2;

namespace Program3
{
    internal class Program
    {
        private static readonly string forbiddenPlanet = "Лимония";
        private static readonly string[] planets = { "Земля", forbiddenPlanet, "Марс" };
        private static int _count;
        public static void Main(string[] args)
        {
            var catalogue = new PlanetCatalogue();
            HomeWork01.Library.HomeWorkHelper.PrintConsole("********** Основное задание **********");
            var tuples = planets.Select(x => catalogue.GetPlanet(x, str => (IncreaseCounter() % 3 == 0) ? "Вы спрашиваете слишком часто" : null)).ToArray();
            PrintTuples(tuples, planets);
            
            HomeWork01.Library.HomeWorkHelper.PrintConsole("********** Дополнительное задание **********");
            var tuples2 = planets.Select(x => catalogue.GetPlanet(x, str => (x.Equals(forbiddenPlanet)) ? "Это запретная планета" : null)).ToArray();
            PrintTuples(tuples2, planets);
        }
        
        public static void PrintTuples((int numberFromSun, int equatorLength, string errorMessage)[] tuples, string[] planets)
        {
            for (int i = 0; i < tuples.Length; i++)
            {
                if (!string.IsNullOrEmpty(tuples[i].errorMessage))
                    Console.WriteLine(tuples[i].errorMessage);
                else
                    Console.WriteLine($"Название: {planets[i]}, Номер от Солнца: {tuples[i].numberFromSun}, Длина экватора: {tuples[i].equatorLength}");
            }
        }

        private static int IncreaseCounter()
        {
            _count++;
            return _count;
        }
    }
}