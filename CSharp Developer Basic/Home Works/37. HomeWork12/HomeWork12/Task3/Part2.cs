using System.Collections.Immutable;

namespace Task3
{
    public class Part2
    {
        public ImmutableList<string> Poem { get; private set; }
        public ImmutableList<string> AddPart(ImmutableList<string> poem)
        {
            Poem = poem.Add("А это пшеница,\n" +
                            "Которая в темном чулане хранится\n" +
                            "В доме,\n" +
                            "Который построил Джек.");
            return Poem;
        }
    }
}
