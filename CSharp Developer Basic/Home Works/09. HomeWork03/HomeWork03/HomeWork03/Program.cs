using HomeWork01.Library;
using System.Text;

namespace HomeWork03
{
    public class Program
    {
        static void Main(string[] args)
        {
            HomeWorkHelper.PrintConsole(Settings.InstructionsText, ConsoleColor.Magenta);
            ConsoleKeyInfo key;

            var equasion = new QuadEquasion("", "", "");
            var menu = new Menu(equasion);
            menu.Show();

            bool isEntePressed = false;

            while (true)
            {
                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        menu.Option++;
                        break;
                    case ConsoleKey.UpArrow:
                        menu.Option--;
                        break;
                    case ConsoleKey.Backspace:
                        menu.BackspaceSelected();
                        break;
                    case ConsoleKey.Enter:
                        isEntePressed = true;
                        break;
                    case ConsoleKey.Escape:
                        menu.Quit();
                        return;
                    default:
                        {
                            if (!char.IsControl(key.KeyChar))
                                menu.AddSelected(key.KeyChar);
                            break;
                        }
                }   
                if (!isEntePressed)
                    menu.Show();
                else
                {
                    menu.PrepareWindowOutput();
                    try
                    {
                        var result = equasion.TrySolve();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine(result);
                    }
                    catch(OutOfIntegerRangeException ex)
                    {
                        FormatData(ex.Message, Severity.Exception, menu.GetMenuParams());
                    }
                    catch (InvalidCastException ex)
                    {
                        FormatData(ex.Message, Severity.Error, menu.GetMenuParams());
                    }
                    catch(Exception ex)
                    {
                        FormatData(ex.Message, Severity.Error, menu.GetMenuParams());
                    }
                    menu.SetSelectedPosition();
                    isEntePressed = false;
                }
            }
        }
        static void FormatData(string message, Severity severity, IDictionary<int, string> data)
        {
            ConsoleColor color = Console.BackgroundColor;
            if (severity == Severity.Warning)
                color = ConsoleColor.Yellow;
            else if (severity == Severity.Error)
                color = ConsoleColor.DarkRed;
            else if (severity == Severity.Exception)
                color = ConsoleColor.DarkGreen;

            var separator = new string('-', 50);

            Console.BackgroundColor = color;
            
            var sb = new StringBuilder(separator)
                .Append(Environment.NewLine)
                .Append(message)
                .Append(Environment.NewLine)
                .Append(new string('-', 50));
            Console.WriteLine($"{sb}\n");
            for (int i = 0; i < data.Count; i++)
                Console.WriteLine(data[i]);
            Console.ResetColor();
        }

    }
}
