using System.Text;

namespace HomeWork1
{
    internal class Program
    {
        const string INPUT_TABLE_RANGE = "Введите размерность таблицы:";
        const string INPUT_SIMPLE_TEXT = "Введите производбный текст:";
        const string INCORRECT_NUMBER_FORMAT = "Вы ввели не число - Попробуйте еще раз";
        const string INCORRECT_NUMBER_VALUE = "Введеное число не должно быть меньше 1 и больше 6 - Попробуйте еще раз";
        const string TOO_LONG_TEXT = "Слишком длинный текст";
        const char MATRIX_RANGE_SYMBOL = '+';
        const char MATRIX_SEPARATOR_SYMBOL = ' ';

        const int NUMBER_LOWER_LIMIT = 1;
        const int NUMBER_UPPER_LIMIT = 6;
        const int MATRIX_MAX_WIDTH = 40;

        static void Main(string[] args)
        {
            #region Пункт 1
            int n = GetSimpleNumber();
            #endregion

            #region Пункт 2
            string text = GetSimpleString(n);
            Console.WriteLine();
            #endregion
            
            #region Пункт 3
            int rowWidth = 2 * n + text.Length;
            int rowHight = 2 * n - 1;
            PrintBorder(rowWidth);
            PrintFirstRow(rowWidth, rowHight, n, text);
            PrintBorder(rowWidth);
            PrintSecondRow(rowWidth, rowHight);
            PrintBorder(rowWidth);
            PrintThirdRow(rowWidth, rowWidth - 2); // -2 - т.к. 2 символа - это левая и правая границы ячейки
            PrintBorder(rowWidth);
            #endregion
        }
        #region Функции для получения значений из консоли
        /// <summary>
        /// Функция для получения введенного с клавиатуры числа n
        /// </summary>
        /// <returns>Число 1 < n < 6</returns>
        static int GetSimpleNumber()
        {
            int n = 0;
            do
            {
                PrintConsole(INPUT_TABLE_RANGE);
                if (int.TryParse(Console.ReadLine(), out int x))
                {
                    n = (x < NUMBER_LOWER_LIMIT || x > NUMBER_UPPER_LIMIT) ? 0 : x;
                    if (n == 0)
                        PrintConsoleErr(INCORRECT_NUMBER_VALUE);
                }
                else
                    PrintConsoleErr(INCORRECT_NUMBER_FORMAT);
            }
            while (n == 0);
            return n;
        }
        /// <summary>
        /// Функция для получения текста подхоядщей длинны
        /// </summary>
        /// <param name="range">Полученное ранее число, ограничивающее размерность матрицы</param>
        /// <returns>Текст определенной длинны</returns>
        static string GetSimpleString(int range)
        {
            string? inputText;
            bool isLoop;
            int maxStrLen = (MATRIX_MAX_WIDTH - 2 * range); // 1 + (n - 1) + str.Len + (n - 1) + 1
            do
            {
                PrintConsole(INPUT_SIMPLE_TEXT);
                inputText = Console.ReadLine();
                isLoop = string.IsNullOrEmpty(inputText);
                if (!isLoop && inputText?.Length > maxStrLen) // run this check if only not isLoop
                {
                    isLoop = true;
                    PrintConsoleErr(TOO_LONG_TEXT);
                }
            }
            while (isLoop);
            return inputText;
        }
        #endregion
        #region Функции для печати таблицы
        /// <summary>
        /// Печать междустрочного разделителя
        /// </summary>
        /// <param name="w">Ширина строки</param>
        static void PrintBorder(int w)
        {
            while(w-- > 0)
                Console.Write(MATRIX_RANGE_SYMBOL);
            Console.WriteLine();
        }
        /// <summary>
        /// Печать первой строки
        /// </summary>
        /// <param name="w">Ширина строки</param>
        /// <param name="h">Высота строки</param>
        /// <param name="n">Количество символов от границы строки до текстового выражения</param>
        /// <param name="text">Текстовое выражение</param>
        static void PrintFirstRow(int w, int h, int n, string text)
        {
            int hMid = h/2; // Выносим выше, чтобы сэкономить на вычислениях внутри цикла
            bool isMidleHeight;
            for (int i = 0; i < h; i++)
            {
                isMidleHeight = i == hMid;
                for (int j = 0; j < w; j++)
                {
                    if ((j == 0) || (j == w - 1)) // Позиция = граница ?
                        Console.Write(MATRIX_RANGE_SYMBOL);
                    else if ((isMidleHeight) && j >= n && j < n + text.Length) // Позиция начала и конца печати текстового выражения
                        Console.Write(text[j - n]);
                    else
                        Console.Write(MATRIX_SEPARATOR_SYMBOL);
                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Печать второй строки 
        /// </summary>
        /// <param name="w">Ширина строки</param>
        /// <param name="h">Высота строки</param>
        static void PrintSecondRow(int w, int h)
        {
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if ((j == 0 || j == w - 1) || ((i + j) % 2 == 0))
                        Console.Write(MATRIX_RANGE_SYMBOL);
                    else
                        Console.Write(MATRIX_SEPARATOR_SYMBOL);
                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Печать третей строки
        /// </summary>
        /// <param name="w">Ширина строки</param>
        /// <param name="h">Высота строки</param>
        static void PrintThirdRow(int w, int h)
        {
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    bool isBorder = (j == 0) || (j == w - 1);
                    bool isMainDiagonal = j - 1 == i; // Диагональ слева направо
                    bool isSecondaryDiagonal = j - w + 2 + i == 0; // Диагональ справа налево
                    if (isBorder || isMainDiagonal || isSecondaryDiagonal)
                        Console.Write(MATRIX_RANGE_SYMBOL);
                    else
                        Console.Write(MATRIX_SEPARATOR_SYMBOL);
                }
                Console.WriteLine();
            }
        }

        #endregion
        #region Вспомогательные функции для цветной печати в консоли
        /// <summary>
        /// Печать в консоль текста для указания действия
        /// </summary>
        /// <param name="text">Текст, который будет выведен на консоль</param>
        static void PrintConsole(string text) => PrintConsole(text, ConsoleColor.Magenta);
        /// <summary>
        /// Печать в консоль текста для обозначения ошибки ввода
        /// </summary>
        /// <param name="text">Текст, который будет выведен на консоль</param>
        static void PrintConsoleErr(string text) => PrintConsole(text, ConsoleColor.Red);
        /// <summary>
        /// Печать в консоль произвольного текста
        /// </summary>
        /// <param name="text">Текст, который будет выведен на консоль</param>
        /// <param name="color">Цвет, которым в консоли будет напечатано сообщение</param>
        static void PrintConsole(string text, ConsoleColor color)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;
        }
        #endregion
    }
}