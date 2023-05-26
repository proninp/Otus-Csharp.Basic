using System.Text;

namespace HomeWork1
{
    internal class Program
    {
        const string InputTableRange = "Введите размерность таблицы:";
        const string InputSimpleText = "Введите производбный текст:";
        const string IncorrectNumberFormat = "Вы ввели не число - Попробуйте еще раз";
        const string IncorrectNumbrtValue = "Введеное число не должно быть меньше 1 и больше 6 - Попробуйте еще раз";
        const string TooLongText = "Слишком длинный текст";
        const char MatrixRangeSymbol = '+';
        const char MatrixSeparatorSymbol = ' ';

        const int NumberLowerLimit = 1;
        const int NumberUpperLimit = 6;
        const int MatrixMaxWidth = 40;

        static void Main(string[] args)
        {
            #region Пункт 1
            var n = GetSimpleNumber();
            #endregion

            #region Пункт 2
            var text = GetSimpleString(n);
            Console.WriteLine();
            #endregion

            #region Пункт 3
            PrintTable(n, text);
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
                PrintConsole(InputTableRange);
                if (int.TryParse(Console.ReadLine(), out int x))
                {
                    n = (x < NumberLowerLimit || x > NumberUpperLimit) ? 0 : x;
                    if (n == 0)
                        PrintConsoleErr(IncorrectNumbrtValue);
                }
                else
                    PrintConsoleErr(IncorrectNumberFormat);
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
            string inputText;
            bool isLoop;
            int maxStrLen = (MatrixMaxWidth - 2 * range); // 1 + (n - 1) + str.Len + (n - 1) + 1
            do
            {
                PrintConsole(InputSimpleText);
                inputText = Console.ReadLine();
                isLoop = string.IsNullOrEmpty(inputText);
                if (!isLoop && inputText?.Length > maxStrLen) // run this check if only not isLoop
                {
                    isLoop = true;
                    PrintConsoleErr(TooLongText);
                }
            }
            while (isLoop);
            return inputText;
        }
        #endregion
        #region Функции для печати таблицы
        /// <summary>
        /// Печать таблицы
        /// </summary>
        /// <param name="n">Введенное число n</param>
        /// <param name="text">Введенный текст</param>
        static void PrintTable(int n, string text)
        {
            int rowWidth = 2 * n + text.Length;
            int rowHight = 2 * n - 1;
            string tableBorder = GetBorderText(rowWidth);
            PrintBorder(tableBorder);
            foreach (TableRow row in (TableRow[])Enum.GetValues(typeof(TableRow)))
            {
                switch (row)
                {
                    case TableRow.First:
                        {
                            PrintFirstRow(rowWidth, rowHight, n, text);
                            PrintBorder(tableBorder);
                            break;
                        }
                    case TableRow.Second:
                        {
                            PrintSecondRow(rowWidth, rowHight);
                            PrintBorder(tableBorder);
                            break;
                        }
                    case TableRow.Last:
                        {
                            PrintThirdRow(rowWidth, rowWidth - 2); // -2 - т.к. 2 символа - это левая и правая границы ячейки
                            PrintBorder(tableBorder);
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Печать междустрочного разделителя
        /// </summary>
        /// <param name="w">Ширина строки</param>
        static string GetBorderText(int w)
        {
            /* "1 балл - если горизонтальная граница таблицы, будет собрана один раз, а выводится будет одной строкой" */
            StringBuilder border = new StringBuilder(w);
            for (int i = 0; i < w; i++)
                border.Append(MatrixRangeSymbol);
            return border.ToString();
        }
        /// <summary>
        /// Печать горизонтальной границы таблицы
        /// </summary>
        /// <param name="border">Текст границы таблицы</param>
        static void PrintBorder(string border) => Console.WriteLine(border);
        /// <summary>
        /// Печать первой строки
        /// </summary>
        /// <param name="w">Ширина строки</param>
        /// <param name="h">Высота строки</param>
        /// <param name="n">Количество символов от границы строки до текстового выражения</param>
        /// <param name="text">Текстовое выражение</param>
        static void PrintFirstRow(int w, int h, int n, string text)
        {
            int centerRow = h / 2; // Выносим выше, чтобы сэкономить на вычислениях внутри цикла
            bool isMidleHeight;
            string emptyRow = new StringBuilder(w)
                .Append(MatrixRangeSymbol)
                .Append(new string(MatrixSeparatorSymbol, w - 2))
                .Append(MatrixRangeSymbol)
                .ToString();
            string rowWithText = new StringBuilder(w)
                .Append(MatrixRangeSymbol)
                .Append(new string(MatrixSeparatorSymbol, (w - 2 - text.Length) / 2))
                .Append(text)
                .Append(new string(MatrixSeparatorSymbol, (w - 2 - text.Length) / 2))
                .Append(MatrixRangeSymbol)
                .ToString();
            for (int i = 0; i < h; i++)
            {
                isMidleHeight = i == centerRow;
                if (!isMidleHeight)
                    Console.WriteLine(emptyRow);
                else
                    Console.WriteLine(rowWithText);
            }
        }
        /// <summary>
        /// Печать второй строки 
        /// </summary>
        /// <param name="w">Ширина строки</param>
        /// <param name="h">Высота строки</param>
        static void PrintSecondRow(int w, int h)
        {
            string evenRowText = GetSecondRowText(w, true);
            string oddRowText = GetSecondRowText(w, false);
            for (int i = 0; i < h; i++)
            {
                if (i % 2 == 0)
                    Console.WriteLine(evenRowText);
                else
                    Console.WriteLine(oddRowText);
            }
        }
        /// <summary>
        /// Печать третей строки
        /// </summary>
        /// <param name="w">Ширина строки</param>
        /// <param name="h">Высота строки</param>
        static void PrintThirdRow(int w, int h)
        {
            StringBuilder sb = new StringBuilder(w);
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    bool isBorder = (j == 0) || (j == w - 1);
                    bool isMainDiagonal = j - 1 == i; // Диагональ слева направо
                    bool isSecondaryDiagonal = j - w + 2 + i == 0; // Диагональ справа налево
                    if (isBorder || isMainDiagonal || isSecondaryDiagonal)
                        sb.Append(MatrixRangeSymbol);
                    else
                        sb.Append(MatrixSeparatorSymbol);
                }
                Console.WriteLine(sb.ToString());
                sb.Clear();
            }
        }
        /// <summary>
        /// Получение текста каждой подстроки второй строки
        /// </summary>
        /// <param name="rowWidth">Ширина строки</param>
        /// <param name="rowNumber">Номер текущей подстроки</param>
        /// <returns>Подстрока</returns>
        static string GetSecondRowText(int rowWidth, bool isEven)
        {
            int shift = (isEven) ? 0 : 1;
            StringBuilder ans = new StringBuilder(rowWidth);
            ans.Append(MatrixRangeSymbol);
            for (int i = 1; i < rowWidth - 1; i++)
            {
                if ((i + shift) % 2 == 0)
                    ans.Append(MatrixRangeSymbol);
                else
                    ans.Append(MatrixSeparatorSymbol);
            }
            ans.Append(MatrixRangeSymbol);
            return ans.ToString();
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
    enum TableRow
    {
        First,
        Second,
        Last
    }
}