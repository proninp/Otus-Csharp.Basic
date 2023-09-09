using System.Collections.Concurrent;
using Task1;

namespace Task2
{
    internal class Program
    {
        private static ConcurrentDictionary<string, int> _library = new();
        public static void Main(string[] args)
        {
            var task = CalcLibraryPercentage();
            while (RunMenu()) ;
        }
        private static bool RunMenu()
        {
            Helper.ConsolePrint("Выберите действие:", ConsoleColor.Magenta);
            Helper.ConsolePrint("1 - Добавить книгу;", ConsoleColor.Magenta);
            Helper.ConsolePrint("2 - Вывести список непрочитанного;", ConsoleColor.Magenta);
            Helper.ConsolePrint("3 - Выйти.", ConsoleColor.Magenta);
            int choise;
            while (true)
            {
                if (!int.TryParse(Console.ReadLine(), out choise))
                    Helper.ConsolePrint("Вы ввели не число. Попробуйте еще раз.", ConsoleColor.DarkRed);
                else if (choise < 1 || choise > 3)
                    Helper.ConsolePrint("Недопустимая опция. Попробуйте еще раз.", ConsoleColor.DarkRed);
                else
                    break;
            }
            switch (choise)
            {
                case 1:
                    Helper.ConsolePrint("Введите название книги:", ConsoleColor.Cyan);
                    var bookName = Console.ReadLine();
                    if (!string.IsNullOrEmpty(bookName))
                        _library.TryAdd(bookName, 0);
                    break;
                case 2:
                    PrintLibrary();
                    break;
                case 3:
                    return false;
                    break;
            }
            return true;
        }
        private static void PrintLibrary()
        {
            foreach (var book in _library)
                Console.WriteLine($"{book.Key} - {book.Value}%");
        }
        private static async Task CalcLibraryPercentage()
        {
            while (true)
            {
                Parallel.ForEach(_library, CalcBookPercentage);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
        private static void CalcBookPercentage(KeyValuePair<string, int> kvp)
        {
            if (_library[kvp.Key] < 100)
                _library[kvp.Key]++; 
            else
                _library.TryRemove(kvp.Key, out int _);
        }
    }
}