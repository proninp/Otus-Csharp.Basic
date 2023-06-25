namespace Program2
{
    internal class PlanetCatalogue
    {
        private readonly List<Planet> _planets = new();
        private int _count = 0;
        public PlanetCatalogue(params Planet[] planets) : this() => _planets.AddRange(planets);
        public PlanetCatalogue()
        {
            _planets.Add(new Planet("Венера", 2, 38_025));
            _planets.Add(new Planet("Земля", 3, 40_075, _planets.Last()));
            _planets.Add(new Planet("Марс", 4, 21_344, _planets.Last()));
        }
        public (int numberFromSun, int equatorLength, string errorMessage) GetPlanet(string name, Func<string, string?> validator)
        {
            string? error = validator(name);
            if (!string.IsNullOrEmpty(error))
                return (0, 0, error);

            foreach (var planet in _planets)
            {   
                if (name.Equals(planet.Name))
                    return (planet.OrderNumber, planet.EquatorLength, "");
            }
            return (0, 0, "Не удалось найти планету");
        }
    }
}
