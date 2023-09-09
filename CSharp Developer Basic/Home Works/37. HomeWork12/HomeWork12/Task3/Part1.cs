using System.Collections.Immutable;

namespace Task3
{
    public class Part1
    {
        public ImmutableList<string> Poem { get; private set; }
        public ImmutableList<string> AddPart(ImmutableList<string> poem)
        {
            Poem = poem.Add("Вот дом,\n" +
                            "Который построил Джек.");
            return Poem;
        }
    }
}
