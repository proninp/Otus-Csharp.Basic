namespace HomeWork11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var elemsCount = 100;
            var map = new OtusDictionary();

            for (int i = 0; i < elemsCount; i++)
                map[i]= $"0x{i:X}";

            foreach (var item in map)
                Console.WriteLine(item);

            Console.WriteLine();
            map[32] = $"{map[32]} by Indexer";
            Console.WriteLine(map[32]);

            Console.ReadLine();
        }
    }
}