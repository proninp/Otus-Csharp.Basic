using System.Collections;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Numerics;

namespace HomeWork2
{
    internal class Program
    {
        delegate void ListAddDelegate(IList<int> list, int s);
        delegate void ArrayListAddDelegate(ArrayList list, int s);
        delegate void LinkedListAddDelegate(LinkedList<int> list, int s);

        static void Main(string[] args)
        {
            int size = 1_000_000;
            int seekElement = 496_753;
            int div = 777;
            string timeSpanFormat = @"m\:ss\.fff";
            var timer = new Stopwatch();

            #region Объявление текстовых переменных для хранения времени выполнения
            string undefListTimeText;
            string predefListTimeText;
            string undefArrayListTimeText;
            string predefArrayListTimeText;
            string addFirstLinkedListTimeText;
            string addLastLinkedListTimeText;
            string getElementListText;
            string getElementArrayListText;
            string getElementLinkedListText;
            string printElementListText;
            string printElementArrayListText;
            string printElementLinkedListText;
            #endregion

            #region Объявление и инициализация списков
            List<int> predefinedSizeList = new List<int>(size);
            List<int> undefinedSizeList = new List<int>();
            ArrayList predefinedSizeArrayList = new ArrayList(size);
            ArrayList undefinedSizeArrayList = new ArrayList();
            LinkedList<int> addFirstLinkedList = new LinkedList<int>();
            LinkedList<int> addLastLinkedList = new LinkedList<int>();
            #endregion

            #region Вычисление времени заполнения для списков
            ListAddDelegate listsFunc = AddListElements;
            undefListTimeText = GetFuncExecutionTime(listsFunc, undefinedSizeList, size, timer, timeSpanFormat);
            predefListTimeText = GetFuncExecutionTime(listsFunc, predefinedSizeList, size, timer, timeSpanFormat);

            ArrayListAddDelegate arrayListFunc = AddListElements;
            predefArrayListTimeText = GetFuncExecutionTime(arrayListFunc, predefinedSizeArrayList, size, timer, timeSpanFormat);
            undefArrayListTimeText = GetFuncExecutionTime(arrayListFunc, undefinedSizeArrayList, size, timer, timeSpanFormat);
            #endregion

            #region Вычисление времени заполнения для связанных списков
            LinkedListAddDelegate linkedListFunc = AddLinkedListElementsFirst;
            addFirstLinkedListTimeText = GetFuncExecutionTime(linkedListFunc, addLastLinkedList, size, timer, timeSpanFormat);
            
            linkedListFunc = AddLinkedListElementsLast;
            addLastLinkedListTimeText = GetFuncExecutionTime(linkedListFunc, addLastLinkedList, size, timer, timeSpanFormat);
            #endregion

            #region Вычисление времени получения элемента
            listsFunc = GetListElement;
            getElementListText = GetFuncExecutionTime(listsFunc, predefinedSizeList, seekElement, timer, timeSpanFormat);
            arrayListFunc = GetArrayListElement;
            getElementArrayListText = GetFuncExecutionTime(arrayListFunc, predefinedSizeArrayList, seekElement, timer, timeSpanFormat);
            linkedListFunc = GetLinkedListElement;
            getElementLinkedListText = GetFuncExecutionTime(linkedListFunc, addLastLinkedList, seekElement, timer, timeSpanFormat);
            #endregion

            #region Вычисление времени печати специальных элементов
            listsFunc = PrintListSpecialElemets;
            printElementListText = GetFuncExecutionTime(listsFunc, predefinedSizeList, div, timer, timeSpanFormat);
            arrayListFunc = PrintArrayListSpecialElemets;
            printElementArrayListText = GetFuncExecutionTime(arrayListFunc, predefinedSizeArrayList, div, timer, timeSpanFormat);
            linkedListFunc = PrintLinkedListSpecialElemets;
            printElementLinkedListText = GetFuncExecutionTime(linkedListFunc, addLastLinkedList, div, timer, timeSpanFormat);
            #endregion

            Console.WriteLine();
            var cColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;

            #region Вывод результата замеров
            string addTitle = $"Добавление {size.ToString("#,0")} элементов в";
            string getTitle = $"Получение значения элемента {seekElement.ToString("#,0")} из";
            string printTitle = $"Печать элементов, делящихся на {div} для структуры";

            Console.WriteLine($"{addTitle} List с предопределенной ёмкостью: {predefListTimeText}");
            Console.WriteLine($"{addTitle} List без предопределенной ёмкости: {undefListTimeText}");
            Console.WriteLine($"{addTitle} ArrayList с предопределенной ёмкостью: {predefArrayListTimeText}");
            Console.WriteLine($"{addTitle} ArrayList без предопределенной ёмкости: {undefArrayListTimeText}");

            Console.WriteLine($"{addTitle} LinkedList в начало: {addFirstLinkedListTimeText}");
            Console.WriteLine($"{addTitle} LinkedList в конец: {addLastLinkedListTimeText}");
            Console.WriteLine();
            Console.WriteLine($"{getTitle} List: {getElementListText}");
            Console.WriteLine($"{getTitle} ArrayList: {getElementArrayListText}");
            Console.WriteLine($"{getTitle} LinkedList: {getElementLinkedListText}");
            Console.WriteLine();
            Console.WriteLine($"{printTitle} List: {printElementListText}");
            Console.WriteLine($"{printTitle} ArrayList: {printElementArrayListText}");
            Console.WriteLine($"{printTitle} LinkedList: {printElementLinkedListText}");
            #endregion

            Console.ForegroundColor = cColor;
        }
        /// <summary>
        /// Добавление элементов в список
        /// </summary>
        /// <param name="list">Список</param>
        /// <param name="size">Количество элементов</param>
        static void AddListElements(IList<int> list, int size)
        {
            for (int i = 0; i < size; i++)
                list.Add(i);
        }
        /// <summary>
        /// Добавление элементов в список
        /// </summary>
        /// <param name="list">Список</param>
        /// <param name="size">Количество элементов</param>
        static void AddListElements(ArrayList list, int size)
        {
            for (int i = 0; i < size; i++)
                list.Add(i);
        }
        /// <summary>
        /// Добавление элементов в конец связанного списка
        /// </summary>
        /// <param name="linkedList">Связанный список</param>
        /// <param name="size">Количество элементов</param>
        static void AddLinkedListElementsLast(LinkedList<int> linkedList, int size)
        {
            for (int i = 0; i < size; i++)
                linkedList.AddLast(i);
        }
        /// <summary>
        /// Добавление элементов в начало связанного списка
        /// </summary>
        /// <param name="linkedList">Связанный список</param>
        /// <param name="size">Количество элементов</param>
        static void AddLinkedListElementsFirst(LinkedList<int> linkedList, int size)
        {
            for (int i = 0; i < size; i++)
                linkedList.AddFirst(i);
        }
        static int FindElement(IList<int> list, int elementInd)
        {
            return list[elementInd];
        }
        /// <summary>
        /// Получение элемента
        /// </summary>
        /// <param name="list">Список</param>
        /// <param name="id">Номер элементы начиная с 0</param>
        static void GetListElement(IList<int> list, int id) 
        {
            int elem = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (i == id)
                {
                    elem = list[i];
                    break;
                }
            }
        }
        /// <summary>
        /// Получение элемента
        /// </summary>
        /// <param name="list">Список</param>
        /// <param name="id">Номер элементы начиная с 0</param>
        static void GetArrayListElement(ArrayList list, int id)
        {
            int elem = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (i == id)
                {
                    elem = (int) (list[i] ?? 0);
                    break;
                }
            }
        }
        /// <summary>
        /// Получение элемента
        /// </summary>
        /// <param name="list">Список</param>
        /// <param name="id">Номер элементы начиная с 0</param>
        static void GetLinkedListElement(LinkedList<int> list, int id) { int a = list.ElementAt(id); }
        /// <summary>
        /// Печать элементов List делящихся на div
        /// </summary>
        /// <param name="list">Список</param>
        /// <param name="div">Делитель</param>
        static void PrintListSpecialElemets(IList<int> list, int div)
        {
            Console.WriteLine($"Элементы List'a, нацело делящиеся на {div}:");
            foreach(var v in list)
                if (v % div == 0)
                    Console.Write($"[{v}] ");
            Console.WriteLine("\n");
        }
        /// <summary>
        /// Печать элементов ArrayList делящихся на div
        /// </summary>
        /// <param name="list">Список</param>
        /// <param name="div">Делитель</param>
        static void PrintArrayListSpecialElemets(ArrayList list, int div)
        {
            Console.WriteLine($"Элементы ArrayList'a, нацело делящиеся на {div}:");
            for (int i = 0; i < list.Count; i++)
            {
                int v = (int)(list[i] ?? 0);
                if (v % div == 0)
                    Console.Write($"[{v}] ");
            }
            Console.WriteLine("\n");
        }
        /// <summary>
        /// Печать элементов LinkedList делящихся на div
        /// </summary>
        /// <param name="list">Список</param>
        /// <param name="div">Делитель</param>
        static void PrintLinkedListSpecialElemets(LinkedList<int> list, int div)
        {
            Console.WriteLine($"Элементы LinkedList'a, нацело делящиеся на {div}:");
            foreach(var v in list)
                if (v % div == 0)
                    Console.Write($"[{v}] ");
            Console.WriteLine("\n");
        }

        #region Функции для замера времени выполнения
        /// <summary>
        /// Вычисление времени для функций добавления элементов в список
        /// </summary>
        /// <param name="func">Делегат функции</param>
        /// <param name="list">Список</param>
        /// <param name="size">Количество элементов</param>
        /// <param name="timer">Таймер</param>
        /// <param name="timeSpanFormat">Формат вывода времени</param>
        /// <returns>Строка времени</returns>
        static string GetFuncExecutionTime(ListAddDelegate func, IList<int> list, int size, Stopwatch timer, string timeSpanFormat)
        {
            timer.Start();
            func(list, size);
            return StopTimer(timer, timeSpanFormat);
        }
        /// <summary>
        /// Вычисление времени для функций добавления элементов в список
        /// </summary>
        /// <param name="func">Делегат функции</param>
        /// <param name="list">Список</param>
        /// <param name="size">Количество элементов</param>
        /// <param name="timer">Таймер</param>
        /// <param name="timeSpanFormat">Формат вывода времени</param>
        /// <returns>Строка времени</returns>
        static string GetFuncExecutionTime(ArrayListAddDelegate func, ArrayList list, int size, Stopwatch timer, string timeSpanFormat)
        {
            timer.Start();
            func(list, size);
            return StopTimer(timer, timeSpanFormat);
        }
        /// <summary>
        /// Вычисление времени для функций добавления элементов в связанный список
        /// </summary>
        /// <param name="func">Делегат функции</param>
        /// <param name="list">Список</param>
        /// <param name="size">Количество элементов</param>
        /// <param name="timer">Таймер</param>
        /// <param name="timeSpanFormat">Формат вывода времени</param>
        /// <returns>Строка времени</returns>
        static string GetFuncExecutionTime(LinkedListAddDelegate func, LinkedList<int> list, int size, Stopwatch timer, string timeSpanFormat)
        {
            timer.Start();
            func(list, size);
            return StopTimer(timer, timeSpanFormat);
        }
        /// <summary>
        /// Остановка замера и вывод времени
        /// </summary>
        /// <param name="timer">Экземпляр таймера</param>
        /// <param name="timeSpanFormat">Формат вывода времени</param>
        /// <returns>Строка времени</returns>
        static string StopTimer(Stopwatch timer, string timeSpanFormat)
        {
            timer.Stop();
            var timeTaken = timer.Elapsed;
            string timeText = timeTaken.ToString(timeSpanFormat);
            timer.Reset();
            return timeText;
        }
        #endregion
    }
}