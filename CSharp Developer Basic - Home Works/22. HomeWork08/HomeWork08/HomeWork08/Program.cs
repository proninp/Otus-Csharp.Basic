using HomeWork01.Library;
namespace HomeWork08
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var runMode = RunMode.Restart;

            while(runMode == RunMode.Restart)
            {
                BinaryTree tree = new BinaryTree();

                AuxiliaryEmployeesInsert(tree);
                EmployeesManualInsert(tree);
                PrintEmployees(tree);
                
                if (tree.Count > 0)
                {
                    do
                    {
                        FindEmployeeBySalary(tree);
                        runMode = GetUserRunMode();
                    }
                    while (runMode == RunMode.SalarySearch);
                }
                if (runMode == RunMode.Restart)
                    Console.WriteLine();
            }
        }
        
        private static void AuxiliaryEmployeesInsert(BinaryTree tree)
        {
            HomeWorkHelper.PrintConsole(Settings.AuxiliaryEmployeesRequest);
            if (IsUseAdditionalEmployeeList())
            {
                var employees = new List<Employee>()
                {
                    new Employee("Александр Друзь", "10000"),
                    new Employee("Максим Поташев", "8500"),
                    new Employee("Борис Левин", "7000"),
                    new Employee("Леонид Тимофеев", "5000"),
                    new Employee("Михаил Дюба", "5000"),
                    new Employee("Виктор Сиднев", "8000")
                };
                foreach (var e in employees)
                    tree.Insert(e);
            }
        }
        private static bool IsUseAdditionalEmployeeList() => Console.ReadLine()?.ToLower().Equals("да") ?? false;
        private static void EmployeesManualInsert(BinaryTree tree)
        {
            HomeWorkHelper.PrintConsole(Settings.RestartDescription);
            string? name;
            string salary;
            do
            {
                Console.WriteLine(Settings.NameInputRequest);
                name = Console.ReadLine();
                if (!string.IsNullOrEmpty(name))
                {
                    salary = IntegerNumberInputRequest(Settings.SalaryInputRequest, false);
                    tree.Insert(new Employee(name, salary));
                }
            } while (!string.IsNullOrEmpty(name));
        }
        private static void PrintEmployees(BinaryTree tree)
        {
            if (tree.Count != 0)
            {
                HomeWorkHelper.PrintConsole(Settings.EmployeeListTraverseDescrption, ConsoleColor.Cyan);
                tree.InOrderTraversal();
            }
            else
                HomeWorkHelper.PrintConsole(Settings.EmployeeListIsEmptyDescription, ConsoleColor.Cyan);
        }
        private static void FindEmployeeBySalary(BinaryTree tree)
        {
            string salaryText = IntegerNumberInputRequest(Settings.SalarySearchRequest, true);
            int.TryParse(salaryText, out int salary);
            Console.WriteLine();
            Console.WriteLine(tree.Find(salary)?.Data.Name ?? Settings.EmployeeNotFoundDescription);
        }
        private static string IntegerNumberInputRequest(string requestText, bool isColored)
        {
            bool isContinue = true;
            string input = string.Empty;
            int dummy;
            while(isContinue)
            {
                if (isColored)
                    HomeWorkHelper.PrintConsole(requestText);
                else
                    Console.WriteLine(requestText);

                input = Console.ReadLine() ?? string.Empty;
                isContinue = string.IsNullOrEmpty(input) || (!int.TryParse(input, out dummy));
                if (isContinue)    
                    HomeWorkHelper.PrintConsoleErr(Settings.NotANumberInputDescription);
            }
            return input;
        }
        private static RunMode GetUserRunMode()
        {
            HomeWorkHelper.PrintConsole(Settings.RunModeSelectionText);
            string mode = Console.ReadLine()?.Trim() ?? string.Empty;
            return mode switch
            {
                "1" => RunMode.SalarySearch,
                "0" => RunMode.Restart,
                _ => RunMode.Exit
            };
        }
        
    }
}