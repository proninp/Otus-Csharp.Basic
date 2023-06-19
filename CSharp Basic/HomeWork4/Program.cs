using System.Globalization;

namespace HomeWork4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string lineSeparator = new string('-', 50);

            #region Демонстрация базового задания: п.1, п.2, п.3
            HomeWork1.Program.PrintConsole("Демонстрация базового задания");
            var s = new MyStack("a", "b", "c");
            Console.WriteLine($"Size = {s.Size}, Top = '{s.Top}'");
            s.PrintStackInOrder();
            Console.WriteLine();
            // ------------------------------------------------
            var elem = s.Pop();
            // Извлек верхний элемент 'c' Size = 2
            Console.WriteLine($"Извлек верхний элемент '{elem}' Size = {s.Size}");
            s.PrintStackInOrder();
            Console.WriteLine();
            // ------------------------------------------------
            s.Add("d");
            // Size = 3, Top = 'd'
            Console.WriteLine($"Добавлен элемент '{s.Top}' Size = {s.Size}");
            s.PrintStackInOrder();
            Console.WriteLine();
            // ------------------------------------------------
            s.Pop();
            s.Pop();
            s.Pop();
            Console.WriteLine("3 раза выполнен s.Pop():");
            // Size = 0, Top = null
            Console.WriteLine($"Size = {s.Size}, Top = {(s.Top == null ? "null" : s.Top)}");
            Console.WriteLine();
            // ------------------------------------------------
            try
            {
                s.Pop();
            }
            catch(EmptyStackException e)
            {
                Console.WriteLine($"s.Pop() - Exception: {e.Message}");
                
            }
            // ------------------------------------------------
            HomeWork1.Program.PrintConsole(lineSeparator);
            #endregion

            #region Демонстрация работы метода Merge: Доп. задание 1
            HomeWork1.Program.PrintConsole("Демонстрация работы метода Merge: Доп. задание 1");
            var s1 = new MyStack("a", "b", "c");
            Console.WriteLine("Стэк s1:");
            s1.PrintStackInOrder();

            var s2 = new MyStack("1", "2", "3");
            Console.WriteLine("Стэк s2:");
            s2.PrintStackInOrder();

            s1.Merge(s2);
            Console.WriteLine("Merge двух стэков:");
            // в стеке s теперь элементы - "a", "b", "c", "3", "2", "1" <- верхний
            Console.WriteLine($"Size = {s1.Size}, Top = '{s1.Top}'");
            s1.PrintStackInOrder();
            HomeWork1.Program.PrintConsole(lineSeparator);
            // -------------------------------
            #endregion

            #region Демонстрация работы метода Concat: Доп. задание 2
            HomeWork1.Program.PrintConsole("Демонстрация работы метода Concat: Доп. задание 2");
            s1 = new MyStack("a", "b", "c");
            Console.WriteLine("Стэк s1:");
            s1.PrintStackInOrder();

            Console.WriteLine("Стэк s2:");
            s2.PrintStackInOrder();

            var s3 = new MyStack("А", "Б", "В");
            Console.WriteLine("Стэк s3:");
            s3.PrintStackInOrder();

            var s4 = MyStack.Concat(s1, s2, s3);
            Console.WriteLine("Concat стэков s1, s2 и s3:");
            s4.PrintStackInOrder();
            HomeWork1.Program.PrintConsole(lineSeparator);
            // -------------------------------
            #endregion
        }
    }
}