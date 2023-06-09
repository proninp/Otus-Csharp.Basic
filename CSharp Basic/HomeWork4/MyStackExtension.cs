using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork4
{
    public static class MyStackExtension
    {
        /// <summary>
        /// метод расширения Merge добавляет элементы s2 в экзмепляр объекта, вызвавший метод
        /// </summary>
        /// <param name="s1">Стэк, в который добавятся элементы</param>
        /// <param name="s2">Стэк, элементы которого добавятся в стэк s1</param>
        public static void Merge(this MyStack s1, MyStack s2)
        {
            var reverted = s2.GetInOrderArray();
            foreach(var item in reverted)
                s1.Add(item);
        }
    }
}
