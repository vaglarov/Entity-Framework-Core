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
                Console.WriteLine(RemoveTown(context));
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
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Department = e.Department.Name,
                    Salary = e.Salary
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
                    Adddress = e.Address
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
                  ,
                  ManagerFullName = e.Manager.FirstName + " " + e.Manager.LastName
                  ,
                  Projects = e.EmployeesProjects.Select(p => new
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
                                .OrderBy(p => p.ProjectName)
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

            var departments = context.Departments
                            .Where(d => d.Employees.Count > 5)
                            .OrderBy(d => d.Employees.Count)
                            .ThenBy(d => d.Name)
                            .Select(d => new
                            {
                                d.Name,
                                ManagerFullName = d.Manager.FirstName + " " + d.Manager.LastName,
                                DepartmantEmployee = d.Employees
                                         .OrderBy(e => e.FirstName)
                                         .ThenBy(e => e.LastName)
                                         .Select(e => new
                                         {
                                             EmpFullName = e.FirstName + " " + e.LastName,
                                             e.JobTitle
                                         }).ToList()
                            }).ToList();


            foreach (var dep in departments)
            {
                result.AppendLine($"{dep.Name} – {dep.ManagerFullName}");
                foreach (var emp in dep.DepartmantEmployee)
                {
                    result.AppendLine($"{emp.EmpFullName} - {emp.JobTitle}");
                }

            }

            return result.ToString().TrimEnd();
        }
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var projects = context.Projects
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .ToList();

            foreach (var project in projects)
            {
                var startDate = project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                result.AppendLine(project.Name);
                result.AppendLine(project.Description);
                result.AppendLine(startDate);
            }
            return result.ToString().TrimEnd();
        }
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Engineering" ||
                            e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" ||
                            e.Department.Name == "Information Services")
                .ToList();

            foreach (var employee in employees)
            {
                employee.Salary += (employee.Salary * 0.12m);
            }

            var promotedЕmployees = employees.Select(e => new
            {
                e.FirstName,
                e.LastName,
                Salary = e.Salary.ToString("F2")
            })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();


            foreach (var employee in promotedЕmployees)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary})");
            }



            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    FullName = e.FirstName + " " + e.LastName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach (var emp in employees)
            {
                result.AppendLine($"{emp.FullName} - {emp.JobTitle} - (${emp.Salary:f2})");
            }

            return result.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var projectToRemove = context.Projects
             .FirstOrDefault(p => p.ProjectId == 2);

            var employeesProjectsToRemove = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2)
                .ToList();

            context.EmployeesProjects.RemoveRange(employeesProjectsToRemove);

            context.Projects.Remove(projectToRemove);

            context.SaveChanges();

            var projects = context.Projects
                .Select(p => p.Name)
                .Take(10)
                .ToList();

            foreach (var project in projects)
            {
                result.AppendLine(project);
            }

            return result.ToString().TrimEnd();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var employees = context.Employees
                  .Where(e => e.Address.Town.Name == "Seattle")
                  .ToList();

            foreach (var employee in employees)
            {
                employee.AddressId = null;
            }

            var addressesToRemove = context.Addresses
                .Where(a => a.Town.Name == "Seattle")
                .ToList();

            context.Addresses.RemoveRange(addressesToRemove);

            var townToRemove = context.Towns
                .FirstOrDefault(t => t.Name == "Seattle");

            context.Towns.Remove(townToRemove);

            context.SaveChanges();

            return $"{addressesToRemove.Count} addresses in Seattle were deleted";
        }
    }
}
