using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork08
{
    public class Settings
    {
        public static string AuxiliaryEmployeesRequest = "Введите \"Да\", если хотите использовать дополнительно предустановленный список сотрудников в итоговом выводе";
        public static string RestartDescription = "Введите сотрудников: имя в первой строке, зарплата в виде целого числа во второй строке\n" +
            "Чтобы закончить ввод, введите пустую строку в качестве имени сотрудника";
        public static string NameInputRequest = "Введите имя:";
        public static string SalaryInputRequest = "Введите зарплату:";
        public static string EmployeeListTraverseDescrption = "\nПолученный список сотрудников:";
        public static string EmployeeListIsEmptyDescription = "\nСписок сотрудников пуст\n";
        public static string SalarySearchRequest = "\nВведите зарплату для поиска сотрудника:";
        public static string EmployeeNotFoundDescription = "Такой сотрудник не найден";
        public static string NotANumberInputDescription = "Вы ввели не число. Попробуйте еще раз.";
        public static string RunModeSelectionText = "\nВведите 0 для перехода к началу работы программы или 1 для поиска зарплаты:";
    }
}
