using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork08
{
    public class Settings
    {
        public const string AuxiliaryEmployeesRequest = "Введите \"Да\", если хотите использовать дополнительно предустановленный список сотрудников в итоговом выводе";
        public const string RestartDescription = "Введите сотрудников: имя в первой строке, зарплата в виде целого числа во второй строке\n" +
            "Чтобы закончить ввод, введите пустую строку в качестве имени сотрудника";
        public const string NameInputRequest = "Введите имя:";
        public const string SalaryInputRequest = "Введите зарплату:";
        public const string EmployeeListTraverseDescrption = "\nПолученный список сотрудников:";
        public const string EmployeeListIsEmptyDescription = "\nСписок сотрудников пуст\n";
        public const string SalarySearchRequest = "\nВведите зарплату для поиска сотрудника:";
        public const string EmployeeNotFoundDescription = "Такой сотрудник не найден";
        public const string NotANumberInputDescription = "Вы ввели не число. Попробуйте еще раз.";
        public const string RunModeSelectionText = "\nВведите 0 для перехода к началу работы программы или 1 для поиска зарплаты:";
    }
}
