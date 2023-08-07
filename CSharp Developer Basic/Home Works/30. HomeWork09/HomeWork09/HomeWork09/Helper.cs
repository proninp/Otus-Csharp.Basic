namespace HomeWork09
{
    public class Helper
    {
        public static void PrintLog(string text, ConsoleColor color = ConsoleColor.DarkCyan)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;
        }
    }
}
