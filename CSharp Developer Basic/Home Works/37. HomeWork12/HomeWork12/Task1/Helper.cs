namespace Task1
{
    public class Helper
    {
        public static void ConsolePrint(string text, ConsoleColor color = ConsoleColor.DarkCyan, bool withLineBreak = true)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            if (withLineBreak)
                Console.WriteLine(text);
            else
                Console.Write(text);
            Console.ForegroundColor = currentColor;
        }
    }
}
