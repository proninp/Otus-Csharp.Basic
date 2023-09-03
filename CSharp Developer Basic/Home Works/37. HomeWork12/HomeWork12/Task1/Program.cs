namespace Task1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var shop = new Shop();
            var customer1 = new Customer("Магазин 1");
            var customer2 = new Customer("Магазин 2");
            shop.RegisterObserver(customer1);
            shop.RegisterObserver(customer2);

            ConsoleKeyInfo key;

            Helper.ConsolePrint($"*** Инструкции ***" +
                $"\nДля добавления нового товара нажмите \"A\";" +
                $"\nДля удаления товара нажмите \"D\" и введите номер идентификатора товара;" +
                $"\nДля завершения работы с программой нажмите Х\n", ConsoleColor.Magenta);
            
            bool isLoop = true;
            while (isLoop)
            {
                Helper.ConsolePrint("Введите инструкцию:");
                key = Console.ReadKey();
                Console.WriteLine();
                switch (key.KeyChar)
                {
                    case 'A':
                    case 'А':
                        shop.Add($"Товар от {DateTime.Now}");
                        break;
                    case 'D':
                        Helper.ConsolePrint("Введите номер идентификатора товара:");
                        int id;
                        while (!int.TryParse(Console.ReadLine(), out id))
                            Helper.ConsolePrint("Вы ввели не число. Попробуйте еще раз.", ConsoleColor.DarkRed);
                        shop.Remove(id);
                        break;
                    case 'X':
                    case 'Х':
                        isLoop = false;
                        break;
                    default:
                        Helper.ConsolePrint("\nДействие нераспознано. Допустимые команды: 'A', 'D', 'X'.\n", ConsoleColor.DarkRed);
                        break;
                }
            }
            Console.ReadLine();
        }
    }
}