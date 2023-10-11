using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTLServiceBot.Assistants
{
    public class AssistLog
    {
        public static void ColoredPrint(string text, LogStatus logStatus = LogStatus.Information)
        {
            var consoleColor = logStatus switch
            {
                LogStatus.Attention => ConsoleColor.Cyan,
                LogStatus.Warning => ConsoleColor.Magenta,
                LogStatus.Error => ConsoleColor.Red,
                _ => Console.ForegroundColor
            };
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;
        }
    }
}
