namespace Program2
{
    internal class Planet
    {
        public string Name { get; set; }
        public short OrderNumber { get; set; }
        public int EquatorLength { get; set; }
        public Planet? PreviousPlanet { get; set; }
        public Planet(string name, short number, int equatorlength, Planet? previousPlatet) : this(name, number, equatorlength)
        {
            PreviousPlanet = previousPlatet;
        }
        public Planet(string name, short number, int equatorlength)
        {
            Name = name;
            OrderNumber = number;
            EquatorLength = equatorlength;
        }
        public override string ToString() => $"Name: {Name}, Number from Sun: {OrderNumber}, Equator Length: {EquatorLength}";
    }
}
