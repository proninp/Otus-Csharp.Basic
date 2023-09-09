using System.Collections.Immutable;

namespace Task3
{
    public class Part9
    {
        public ImmutableList<string> Poem { get; private set; }

        public ImmutableList<string> AddPart(ImmutableList<string> poem)
        {
            Poem = poem.Add("Вот два петуха,\n" +
                            "Которые будят того пастуха,\n" +
                            "Который бранится с коровницей строгою,\n" +
                            "Которая доит корову безрогую,\n" +
                            "Лягнувшую старого пса без хвоста,\n" +
                            "Который за шиворот треплет кота,\n" +
                            "Который пугает и ловит синицу,\n" +
                            "Которая часто ворует пшеницу,\n" +
                            "Которая в темном чулане хранится\n" +
                            "В доме,\n" +
                            "Который построил Джек.");
            return Poem;
        }
    }
}
