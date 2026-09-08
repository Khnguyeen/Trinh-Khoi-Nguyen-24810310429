using System;

namespace BaiTap2_EmployeeHierarchy
{
    public class Person
    {
        public string Id { get; init; }
        public string FullName { get; set; }
        public int BirthYear { get; set; }

        public Person(string id, string fullName, int birthYear)
        {
            Id = id;
            FullName = fullName;
            BirthYear = birthYear;
        }

        public int GetAge(int currentYear)
        {
            return currentYear - BirthYear;
        }
    }

    public class Employee : Person
    {
        public decimal BaseSalary { get; set; }

        public Employee(
            string id,
            string fullName,
            int birthYear,
            decimal baseSalary
        ) : base(id, fullName, birthYear)
        {
            BaseSalary = baseSalary;
        }

        public virtual decimal CalculateIncome()
        {
            return BaseSalary;
        }
    }

    public sealed class Manager : Employee
    {
        public decimal ResponsibilityAllowance { get; set; }

        public Manager(
            string id,
            string fullName,
            int birthYear,
            decimal baseSalary,
            decimal allowance
        ) : base(id, fullName, birthYear, baseSalary)
        {
            ResponsibilityAllowance = allowance;
        }

        public override decimal CalculateIncome()
        {
            return BaseSalary + ResponsibilityAllowance;
        }
    }

    class Program
    {
        static void PrintSalary(Person person, Employee employee)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine($"Tên: {person.FullName}");
            Console.WriteLine($"Tuổi: {person.GetAge(2026)}");
            Console.WriteLine($"Lương cơ bản: {employee.BaseSalary:N0} VNĐ");
            Console.WriteLine($"Thu nhập thực lĩnh: {employee.CalculateIncome():N0} VNĐ");
            Console.WriteLine("-----------------------------");
        }

        static void Main(string[] args)
        {
            Employee employee = new Employee(
                "NV01",
                "Nguyễn Văn A",
                2000,
                10000000
            );

            Manager manager = new Manager(
                "QL01",
                "Trần Văn B",
                1990,
                20000000,
                5000000
            );

            Console.WriteLine("=== PHIẾU LƯƠNG NHÂN VIÊN ===");
            PrintSalary(employee, employee);

            Console.WriteLine("\n=== PHIẾU LƯƠNG QUẢN LÝ ===");
            PrintSalary(manager, manager);
        }
    }
}