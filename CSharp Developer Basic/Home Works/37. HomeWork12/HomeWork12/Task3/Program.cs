using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Task1;

namespace Task3
{
    public class Program
    {
        private static ImmutableList<string> _poem = ImmutableList.Create<string>();

        public static void Main(string[] args)
        {
            var part1 = new Part1();
            var part2 = new Part2();
            var part3 = new Part3();
            var part4 = new Part4();
            var part5 = new Part5();
            var part6 = new Part6();
            var part7 = new Part7();
            var part8 = new Part8();
            var part9 = new Part9();

            _ = part9.AddPart(
                    part8.AddPart(
                        part7.AddPart(
                            part6.AddPart(
                                part5.AddPart(
                                    part4.AddPart(
                                        part3.AddPart(
                                            part2.AddPart(
                                                part1.AddPart(_poem)))))))));

            Helper.ConsolePrint("********** Part 1: **********", ConsoleColor.Magenta);
            PrintList(part1.Poem);

            Helper.ConsolePrint("********** Part 2: **********", ConsoleColor.Magenta);
            PrintList(part2.Poem);

            Helper.ConsolePrint("********** Part 3: **********", ConsoleColor.Magenta);
            PrintList(part3.Poem);

            Helper.ConsolePrint("********** Part 4: **********", ConsoleColor.Magenta);
            PrintList(part4.Poem);

            Helper.ConsolePrint("********** Part 5: **********", ConsoleColor.Magenta);
            PrintList(part5.Poem);

            Helper.ConsolePrint("********** Part 6: **********", ConsoleColor.Magenta);
            PrintList(part6.Poem);

            Helper.ConsolePrint("********** Part 7: **********", ConsoleColor.Magenta);
            PrintList(part7.Poem);

            Helper.ConsolePrint("********** Part 8: **********", ConsoleColor.Magenta);
            PrintList(part8.Poem);

            Helper.ConsolePrint("********** Part 9: **********", ConsoleColor.Magenta);
            PrintList(part9.Poem);
        }
        private static void PrintList(ImmutableList<string> poem)
        {
            poem.ForEach(e => Helper.ConsolePrint(e, ConsoleColor.Cyan));
        }

    }
}