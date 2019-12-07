namespace TeisterMask.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var projects = context.Projects
               .Where(c => c.Tasks.Count >= 1)
               .OrderByDescending(p => p.Tasks.Count())
               .ThenBy(p => p.Name)
               .Select(c => new ExportProjectsDto
               {
                   TasksCount = c.Tasks.Count,
                   ProjectName = c.Name,
                   HasEndDate = (c.DueDate != null) ? "Yes" : "No",
                   Tasks = c.Tasks
                   .OrderBy(t => t.Name)
                   .Select(t => new ExportTaskDto
                   {
                       Name = t.Name,
                       Label = t.LabelType.ToString()
                   })
                   .ToArray()
               })
               .ToArray();


            var xmlSerializer = new XmlSerializer(typeof(ExportProjectsDto[]), new XmlRootAttribute("Projects"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            xmlSerializer.Serialize(new StringWriter(sb), projects, namespaces);

            var result = sb.ToString().TrimEnd();

            return result;
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            //        "Username": "mmcellen1",
            //"Tasks": [
            //  {
            //    "TaskName": "Pointed Gourd",
            //    "OpenDate": "10/08/2018",
            //    "DueDate": "10/24/2019",
            //    "LabelType": "Priority",
            //    "ExecutionType": "ProductBacklog"

            // TotalIncomes = x.Projections.SelectMany(p => p.Tickets).Sum(p => p.Price).ToString("f2"),

            var empl = context.Employees
                    .Select(x => new
                    {
                        Username = x.Username,
                        Tasks = x.EmployeesTasks.Select(y => y.Task).Select(t => new
                        {
                            TaskName = t.Name,
                            OpenDate = t.OpenDate.ToString(),
                            DueDate = t.DueDate.ToString(),
                            LabelType = t.LabelType.ToString(),
                            ExecutionType = t.ExecutionType.ToString()
                        }).ToArray()
                    })
                    .Take(10)
                    .ToArray();



            var jsonResult = JsonConvert.SerializeObject(empl, Newtonsoft.Json.Formatting.Indented);
            return jsonResult;
        }
    }
}