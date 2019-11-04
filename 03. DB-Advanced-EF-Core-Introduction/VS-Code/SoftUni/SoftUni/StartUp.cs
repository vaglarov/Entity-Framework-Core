using System;
using System.Globalization;
using System.Linq;
using System.Text;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                Console.WriteLine(GetEmployee147(context));
            }
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary,
                    e.EmployeeId
                })
                .OrderBy(e => e.EmployeeId)
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine(
                    $"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }

            return result.ToString().TrimEnd();
        }
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var emploees = context.Employees
                        .Select(e => new
                        {
                            e.FirstName,
                            e.Salary
                        })
                        .Where(e => e.Salary > 50000)
                        .OrderBy(e => e.FirstName)
                        .ToList();

            foreach (var empl in emploees)
            {
                result.AppendLine($"{empl.FirstName} - {empl.Salary:f2}");
            }

            return result.ToString().TrimEnd();
        }
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context) 
        {
            StringBuilder result = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    FirstName=e.FirstName,
                    LastName=e.LastName,
                    Department=e.Department.Name, 
                    Salary=e.Salary
                })
               .OrderBy(e => e.Salary)
               .ThenByDescending(e => e.FirstName)
               .ToList();

            foreach (var e in employees)
            {
                result.AppendLine($"{e.FirstName} {e.LastName} from {e.Department} - ${e.Salary:f2}");
            }
            return result.ToString().TrimEnd();
        }
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var employee = context.Employees
               .FirstOrDefault(e => e.LastName == "Nakov");

            employee.Address = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.SaveChanges();


            var employees = context.Employees
                .Select(e => new
                {
                   Adddress= e.Address
                })
                .OrderByDescending(x => x.Adddress)
                .ToList()
                .Take(10)
                .ToList();

            foreach (var address in employees)
            {
                result.AppendLine($"{address.Adddress.AddressText}");
            }

            return result.ToString().TrimEnd();
        }
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var employees = context.Employees
              .Where(e => e.EmployeesProjects.
                        Any(p => p.Project.StartDate.Year >= 2001 &&
                        p.Project.StartDate.Year <= 2003))
              .Select(e => new
              {
                  EmployeeFullName = e.FirstName + " " + e.LastName
                  ,ManagerFullName = e.Manager.FirstName +" "+ e.Manager.LastName
                  ,Projects = e.EmployeesProjects.Select(p => new
                  {
                      p.Project.Name,
                      p.Project.StartDate,
                      p.Project.EndDate
                  })
              })
              .Take(10)
              .ToList();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.EmployeeFullName} - Manager: {employee.ManagerFullName}");

                foreach (var project in employee.Projects)
                {
                    var startDate = project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                    var endDate = project.EndDate.HasValue
                        ? project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                        : "not finished";

                    result.AppendLine($"--{project.Name} - {startDate} - {endDate}");
                }
            }

            return result.ToString().TrimEnd();

        }
        public static string GetAddressesByTown(SoftUniContext context) 
        {
            StringBuilder result = new StringBuilder();

            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = a.Employees.Count
                })
                .Take(10)
                .ToList();

            foreach (var address in addresses)
            {
                result.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeeCount} employees");
            }

            return result.ToString().TrimEnd();
        }
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                //first name, last name, job title 
                .Select(e => new
                {
                    EmployeeFullName = e.FirstName + " " + e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                                .Select(p => new
                                {
                                    ProjectName = p.Project.Name
                                })
                                .OrderBy(p=>p.ProjectName)
                })
                .ToList();
            foreach (var item in employee)
            {
             result.AppendLine($"{item.EmployeeFullName} - {item.JobTitle}");
                foreach (var projects in item.Projects)
                {
                    result.AppendLine($"{projects.ProjectName}");
                }

            }

            return result.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            return result.ToString().TrimEnd();
        }
    }
}
