namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;
    using TeisterMask.DataProcessor.ImportDto;
    using TeisterMask.Data.Models;
    using System.IO;
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;
    using System.Linq;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            StringBuilder stBuilder = new StringBuilder();

            var xmlSeserializer = new XmlSerializer(typeof(ImportProjectDto[]),
                                                          new XmlRootAttribute("Projects"));

            var projecsDto = (ImportProjectDto[])xmlSeserializer.Deserialize(new StringReader(xmlString));

            List<Project> projectsList = new List<Project>();

            foreach (var projectDto in projecsDto)
            {
                if (!IsValid(projectDto))
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var dateOpenDateResult = new DateTime();
                var openDateChecker = DateTime.TryParseExact(projectDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOpenDateResult);
                if (!openDateChecker)
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var dueDateResult = new DateTime();
                var dueDateChecker = DateTime.TryParseExact(projectDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dueDateResult);
                if (!dueDateChecker)
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var taskList = new List<Task>();
                foreach (var taskDto in projectDto.Tasks)
                {
                    if (!IsValid(taskDto))
                    {
                        stBuilder.AppendLine(ErrorMessage);
                        continue;
                    }
                    var openDateTask = new DateTime();
                    var openDateTaskChecker = DateTime.TryParseExact(projectDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out openDateTask);
                    if (!openDateTaskChecker)
                    {
                        stBuilder.AppendLine(ErrorMessage);
                        continue;
                    }

                    var dueDateTask = new DateTime();
                    var dueDateTaskChecker = DateTime.TryParseExact(projectDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dueDateTask);
                    if (!dueDateTaskChecker)
                    {
                        stBuilder.AppendLine(ErrorMessage);
                        continue;
                    }

                    var executionType = Enum.TryParse<ExecutionType>(taskDto.ExecutionType, out ExecutionType resultExecutionType);
                    var labelTypeType = Enum.TryParse<LabelType>(taskDto.LabelType, out LabelType resultLabelType);

                    if (!executionType || !labelTypeType)
                    {
                        stBuilder.AppendLine(ErrorMessage);
                        continue;
                    }

                    //if (dueDateTask >= openDateTask)
                    //{
                    //    stBuilder.AppendLine(ErrorMessage);
                    //    continue;
                    //}
                    var task = new Task
                    {
                        Name = taskDto.Name,
                        OpenDate = openDateTask,
                        DueDate = dueDateTask,
                        ExecutionType = resultExecutionType,
                        LabelType = resultLabelType
                    };
                    taskList.Add(task);
                }
                context.Tasks.AddRange(taskList);
                //context.SaveChanges();
                var project = new Project
                {
                    Name = projectDto.Name,
                    OpenDate = dateOpenDateResult,
                    DueDate = dueDateResult,
                    Tasks = taskList
                };

                projectsList.Add(project);
                foreach (var task1 in taskList)
                {
                    task1.Project = project;
                }
                //"Successfully imported project - {0} with {1} tasks.";
                stBuilder.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, taskList.Count));
            }

            context.Projects.AddRange(projectsList);
            context.SaveChanges();

            string resultBuilder = stBuilder.ToString().TrimEnd();

            return resultBuilder;
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {

            var employesDTO = JsonConvert.DeserializeObject<ImportEmployeDto[]>(jsonString);

            StringBuilder stBuilder = new StringBuilder();
            var employe = new List<Employee>();
            foreach (var emplDto in employesDTO)
            {
                if (!IsValid(emplDto))
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var taskList = new List<Task>();
                foreach (var taskObj in emplDto.TaskId)
                {
                    if (context.Tasks.Any(x => x.Id == taskObj))
                    {
                        stBuilder.AppendLine(ErrorMessage);
                        continue;
                    }
                    var task = context.Tasks.Where(x => x.Id == taskObj).FirstOrDefault();
                    taskList.Add(task);
                }


                var employye = new Employee
                {
                    Username = emplDto.Username,
                    Email = emplDto.Email,
                    Phone = emplDto.Phone
                };

                var mappingTable = new List<EmployeeTask>();
                foreach (var taskEmpl in taskList)
                {
                    var tskEmp = new EmployeeTask
                    {
                        Employee = employye,
                        Task = taskEmpl
                    };
                    mappingTable.Add(tskEmp);
                }
                context.EmployeesTasks.AddRange(mappingTable);

                employe.Add(employye);
                //SuccessfullyImportedEmployee
                //= "Successfully imported employee - {0} with {1} tasks.";
                stBuilder.AppendLine(string.Format(SuccessfullyImportedEmployee, employye.Username, taskList.Count));

            }



            context.Employees.AddRange(employe);
            context.SaveChanges();
            string result = stBuilder.ToString().TrimEnd();

            return result;

        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}