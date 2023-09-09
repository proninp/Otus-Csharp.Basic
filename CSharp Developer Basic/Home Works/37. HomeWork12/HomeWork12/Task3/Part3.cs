using System.Collections.Immutable;

namespace Task3
{
    public class Part3
    {
        public ImmutableList<string> Poem { get; private set; }

        public ImmutableList<string> AddPart(ImmutableList<string> poem)
        {
            Poem = poem.Add("А это веселая птица-синица,\n" +
                            "Которая часто ворует пшеницу,\n" +
                            "Которая в темном чулане хранится\n" +
                            "В доме,\n" +
                            "Который построил Джек.");
            return Poem;
        }
    }
}
