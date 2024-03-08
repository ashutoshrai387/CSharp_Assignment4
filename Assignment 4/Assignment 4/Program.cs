using System;
using System.Linq;
using System.Collections.Generic;

public class Program
{
    IList<Employee> employeeList;
    IList<Salary> salaryList;

    public Program()
    {
        employeeList = new List<Employee>() {
            new Employee(){ EmployeeID = 1, EmployeeFirstName = "Rajiv", EmployeeLastName = "Desai", Age = 49},
            new Employee(){ EmployeeID = 2, EmployeeFirstName = "Karan", EmployeeLastName = "Patel", Age = 32},
            new Employee(){ EmployeeID = 3, EmployeeFirstName = "Sujit", EmployeeLastName = "Dixit", Age = 28},
            new Employee(){ EmployeeID = 4, EmployeeFirstName = "Mahendra", EmployeeLastName = "Suri", Age = 26},
            new Employee(){ EmployeeID = 5, EmployeeFirstName = "Divya", EmployeeLastName = "Das", Age = 20},
            new Employee(){ EmployeeID = 6, EmployeeFirstName = "Ridhi", EmployeeLastName = "Shah", Age = 60},
            new Employee(){ EmployeeID = 7, EmployeeFirstName = "Dimple", EmployeeLastName = "Bhatt", Age = 53}
        };

        salaryList = new List<Salary>() {
            new Salary(){ EmployeeID = 1, Amount = 1000, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 1, Amount = 500, Type = SalaryType.Performance},
            new Salary(){ EmployeeID = 1, Amount = 100, Type = SalaryType.Bonus},
            new Salary(){ EmployeeID = 2, Amount = 3000, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 2, Amount = 1000, Type = SalaryType.Bonus},
            new Salary(){ EmployeeID = 3, Amount = 1500, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 4, Amount = 2100, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 5, Amount = 2800, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 5, Amount = 600, Type = SalaryType.Performance},
            new Salary(){ EmployeeID = 5, Amount = 500, Type = SalaryType.Bonus},
            new Salary(){ EmployeeID = 6, Amount = 3000, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 6, Amount = 400, Type = SalaryType.Performance},
            new Salary(){ EmployeeID = 7, Amount = 4700, Type = SalaryType.Monthly}
        };
    }

    public static void Main()
    {
        Program program = new Program();

        program.Task1();

        program.Task2();

        program.Task3();
    }

    public void Task1()
    {
        var totalSalaryByEmployee = from emp in employeeList
                                    join sal in salaryList on emp.EmployeeID equals sal.EmployeeID
                                    group sal by new { emp.EmployeeFirstName, emp.EmployeeLastName } into g
                                    select new
                                    {
                                        Name = g.Key.EmployeeFirstName + " " + g.Key.EmployeeLastName,
                                        TotalSalary = g.Sum(x => x.Amount)
                                    };

        var sortedSalaryByEmployee = totalSalaryByEmployee.OrderBy(x => x.TotalSalary);

        Console.WriteLine("Task 1: Total Salary of all employees with corresponding names in ascending order:");
        foreach (var item in sortedSalaryByEmployee)
        {
            Console.WriteLine($"{item.Name}: {item.TotalSalary}");
        }
        Console.WriteLine();
    }

    public void Task2()
    {
        var secondOldestEmployee = employeeList.OrderByDescending(x => x.Age).Skip(1).FirstOrDefault();
        var totalSalaryOfSecondOldestEmployee = salaryList.Where(x => x.EmployeeID == secondOldestEmployee.EmployeeID)
            .Sum(x => x.Amount);

        Console.WriteLine($"Task 2: Employee details of 2nd oldest employee ({secondOldestEmployee.EmployeeFirstName} {secondOldestEmployee.EmployeeLastName})" +
            $" including total monthly salary: Age: {secondOldestEmployee.Age}, Total Monthly Salary: {totalSalaryOfSecondOldestEmployee}");
        Console.WriteLine();
    }

    public void Task3()
    {
        var meanSalaries = employeeList.Where(x => x.Age > 30)
                                       .Join(salaryList, emp => emp.EmployeeID, sal => sal.EmployeeID,
                                             (emp, sal) => sal.Amount)
                                       .GroupBy(x => SalaryType.Monthly);

        Console.WriteLine("Task 3: Mean of Monthly, Performance, and Bonus salary of employees aged greater than 30:");
        foreach (var group in meanSalaries)
        {
            Console.WriteLine($"{group.Key} Salary: {group.Average()}");
        }
    }
}

public enum SalaryType
{
    Monthly,
    Performance,
    Bonus
}

public class Employee
{
    public int EmployeeID { get; set; }
    public string EmployeeFirstName { get; set; }
    public string EmployeeLastName { get; set; }
    public int Age { get; set; }
}

public class Salary
{
    public int EmployeeID { get; set; }
    public int Amount { get; set; }
    public SalaryType Type { get; set; }
}