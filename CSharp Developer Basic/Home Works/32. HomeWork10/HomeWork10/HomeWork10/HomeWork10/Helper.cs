namespace HomeWork10
{
    public class Helper
    {
        public static void ConsolePrint(string text, ConsoleColor color = ConsoleColor.DarkCyan)
        {
            ConsolePrint(text, true, color);
        }
        public static void ConsolePrint(string text, bool withLineBreak = true, ConsoleColor color = ConsoleColor.DarkCyan)
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
