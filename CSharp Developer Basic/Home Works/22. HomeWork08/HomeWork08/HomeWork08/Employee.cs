namespace HomeWork08
{
    public class Employee: IComparable<Employee>
    {
        private static string IncorrectSalaryFormat = "Введено не целое число";
        public string Name { get; set; }
        public int Salary { get; set; }
        public Employee(string name, string? salaryStr)
        {
            if (string.IsNullOrEmpty(salaryStr) || !int.TryParse(salaryStr, out int salary))
                throw new ArgumentException(IncorrectSalaryFormat);
            Name = name;
            Salary = salary;
        }
        public static bool operator >(Employee left, Employee right) => left.Salary > right.Salary;
        public static bool operator <(Employee left, Employee right) => left.Salary < right.Salary;
        public static bool operator >=(Employee left, Employee right) => left.Salary >= right.Salary;
        public static bool operator <=(Employee left, Employee right) => left.Salary <= right.Salary;
        public static bool operator >(Employee left, int value) => left.Salary > value;
        public static bool operator <(Employee left, int value) => left.Salary < value;
        public static bool operator >=(Employee left, int value) => left.Salary >= value;
        public static bool operator <=(Employee left, int value) => left.Salary <= value;
        public static bool operator ==(Employee left, Employee right) => left.Equals(right);
        public static bool operator !=(Employee left, Employee right) => !left.Equals(right);
        public static bool operator ==(Employee left, int value) => left.Salary == value;
        public static bool operator !=(Employee left, int value) => left.Salary != value;
        public override bool Equals(object? obj)
        {
            if (obj is not Employee employee)
                return false;
            return employee.Name == Name && employee.Salary == Salary;
        }
        public override int GetHashCode() => Name.GetHashCode() ^ Salary.GetHashCode();
        public override string ToString() => $"{Name.PadRight(20)}: {Salary}";
        public int CompareTo(Employee? employee)
        {
            if (employee is null)
                return 1;
            if (Salary < employee.Salary)
                return -1;
            else if (Salary > employee.Salary)
                return 1;
            else
                return string.Compare(Name, employee.Name, StringComparison.InvariantCulture);
        }        
    }
}
