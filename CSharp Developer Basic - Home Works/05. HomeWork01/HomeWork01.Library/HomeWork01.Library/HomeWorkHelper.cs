namespace HomeWork01.Library
{
    public class HomeWorkHelper
    {
        #region Вспомогательные функции для цветной печати в консоли
        /// <summary>
        /// Печать в консоль текста для указания действия
        /// </summary>
        /// <param name="text">Текст, который будет выведен на консоль</param>
        public static void PrintConsole(string text) => PrintConsole(text, ConsoleColor.Magenta);
        /// <summary>
        /// Печать в консоль текста для обозначения ошибки ввода
        /// </summary>
        /// <param name="text">Текст, который будет выведен на консоль</param>
        public static void PrintConsoleErr(string text) => PrintConsole(text, ConsoleColor.Red);
        /// <summary>
        /// Печать в консоль произвольного текста
        /// </summary>
        /// <param name="text">Текст, который будет выведен на консоль</param>
        /// <param name="color">Цвет, которым в консоли будет напечатано сообщение</param>
        public static void PrintConsole(string text, ConsoleColor color)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;
        }
        #endregion
    }
}