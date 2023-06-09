using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HomeWork4
{
    public class MyStack
    {
        readonly string EmptyStackExceptionText = "Стэк пустой";
        StackItem? CurrentItem { get; set; }
        public string? Top
        {
            get { return CurrentItem?.ItemValue; }
        }
        public int Size { get; set; }

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        public MyStack()
        {
            Size = 0;
        }
        /// <summary>
        /// Конструктор, принимающий неограниченное число параметров
        /// </summary>
        /// <param name="items">Параметры типа string</param>
        public MyStack(params string[] items)
        {
            foreach(var item in items)
                Add(item);
        }
        /// <summary>
        /// Статический метод Concat, объединяющий массив стэков
        /// </summary>
        /// <param name="stacks">Неограниченное количество параметров типа MyStack</param>
        /// <returns>новый стек с элементами каждого стека в порядке параметров, но сами элементы записаны в обратном порядке</returns>
        public static MyStack Concat(params MyStack[] stacks)
        {
            MyStack concatedStack = new MyStack();            
            foreach(var stack in stacks)
            {
                concatedStack.Add(stack.GetInOrderArray());
            }
            return concatedStack;
        }
        /// <summary>
        /// Возвращает массив строк в обратном плрядке - верхний элемент стэка будет последним в массиве
        /// </summary>
        /// <returns>Массив строк</returns>
        public string[] GetPostOrderArray()
        {
            var root = CurrentItem;
            var array = new string[Size];
            int i = Size - 1;
            while(root != null)
            {
                array[i--] = root.ItemValue;
                root = root.PreviousItem;
            }
            return array;
        }
        /// <summary>
        /// Возвращает массив строк в прямом плрядке - верхний элемент стэка будет первым в массиве
        /// </summary>
        /// <returns>Массив строк</returns>
        public string[] GetInOrderArray()
        {
            var root = CurrentItem;
            var array = new string[Size];
            int i = 0;
            while(root != null)
            {
                array[i++] = root.ItemValue;
                root = root.PreviousItem;
            }
            return array;
        }
        /// <summary>
        /// Добавить элементы массива в стек
        /// </summary>
        /// <param name="items">Массив строк для добавления в стэк</param>
        public void Add(string[] items)
        {
            foreach (var item in items)
                Add(item);
        }
        /// <summary>
        /// Добавить элемент в стек
        /// </summary>
        /// <param name="item">Строка для добавления в стэк</param>
        public void Add(string item)
        {
            if (item == null)
                return;
            var prev = CurrentItem;
            CurrentItem = new StackItem(item);
            CurrentItem.PreviousItem = prev;
            Size++;
        }
        /// <summary>
        /// Извлекает верхний элемент и удаляет его из стека
        /// </summary>
        /// <returns>Извлеченный верхний элемент стэка</returns>
        /// <exception cref="EmptyStackException">При попытке вызова метода Pop у пустого стека - выбрасывается исключение с сообщением "Стек пустой"</exception>
        public string Pop()
        {
            if (Size == 0 || CurrentItem == null)
                throw new EmptyStackException(EmptyStackExceptionText);
            string item = CurrentItem.ItemValue;
            CurrentItem = CurrentItem.PreviousItem;
            Size--;
            return item;
        }
        /// <summary>
        /// Печать стэка в прямом порядке
        /// </summary>
        public void PrintStackInOrder()
        {
            if (CurrentItem == null)
            {
                Console.WriteLine(EmptyStackExceptionText);
                return;
            }
            var root = CurrentItem;
            root.InOrderTraversalPrint();
            Console.WriteLine();
        }
        

        private class StackItem
        {
            public string? ItemValue { get; set; }
            public StackItem? PreviousItem { get; set; }
            public StackItem(string v) => ItemValue = v;
            /// <summary>
            /// Печать элементов в прямом порядке
            /// </summary>
            public void InOrderTraversalPrint()
            {
                if (PreviousItem != null)
                    PreviousItem.InOrderTraversalPrint();
                Console.Write($"[{ItemValue}] ");
            }
            /// <summary>
            /// Печать элементов в обратном порядке
            /// </summary>
            public void PostOrderTraversalPrint()
            {
                Console.Write($"[{ItemValue}] ");
                if (PreviousItem != null)
                    PreviousItem.PostOrderTraversalPrint();
            }
        }
    }

}
