namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmesDto = JsonConvert.DeserializeObject<ImportDepartmentsDto[]>(jsonString);

            StringBuilder stBuilder = new StringBuilder();
            List<Department> departmentList = new List<Department>();
            foreach (var departmetDto in departmesDto)
            {
                if (!IsValid(departmetDto))
                {
                    stBuilder.AppendLine("Invalid Data");
                    continue;
                }

                var cellList = new List<Cell>();

                foreach (var cellsDto in departmetDto.Cells)
                {
                    if (!IsValid(cellsDto))
                    {
                        stBuilder.AppendLine("Invalid Data");
                        cellList.Clear();
                        break;
                    }

                    var cell = new Cell { CellNumber = cellsDto.CellNumber, HasWindow = cellsDto.HasWindow };
                    cellList.Add(cell);
                }


                if (cellList.Count == 0)
                {
                    continue;
                }
                else
                {
                    context.Cells.AddRange(cellList);

                }



                var department = new Department
                {
                    Name = departmetDto.Name,
                    Cells = cellList.ToArray()

                };


                departmentList.Add(department);
                stBuilder.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.Departments.AddRange(departmentList);
            context.SaveChanges();
            string result = stBuilder.ToString().TrimEnd();

            return result;
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder stBuilder = new StringBuilder();
           
            var prisonersDto = JsonConvert.DeserializeObject<ImortPrisonerDto[]>(jsonString);

            List<Prisoner> prisonerList = new List<Prisoner>();
            foreach (var prisonDto in prisonersDto)
            {
                if (!IsValid(prisonDto))
                {
                    stBuilder.AppendLine("Invalid Data");
                    continue;
                }

                var mailsList = new List<Mail>();
                bool isCorrectMail = true;

                foreach (var mailsDto in prisonDto.Mails)
                {
                    if (!IsValid(mailsDto))
                    {
                        stBuilder.AppendLine("Invalid Data");
                        mailsList.Clear();
                        isCorrectMail = false;
                        break;
                    }

                    var mails = new Mail { Description = mailsDto.Description, Sender = mailsDto.Sender,Address=mailsDto.Address };
                    mailsList.Add(mails);
                }


                var cell = context.Cells.FirstOrDefault(c => c.Id == prisonDto.CellId);
                if (cell==null)
                {
                    stBuilder.AppendLine("Invalid Data");
                    continue;
                }

                if (!isCorrectMail)
                {
                    continue;
                }
                else
                {
                    context.Mails.AddRange(mailsList);

                }


                var prisoner = new Prisoner
                {
                    FullName = prisonDto.FullName,
                    Nickname = prisonDto.Nickname,
                    Age=prisonDto.Age,
                    IncarcerationDate=DateTime.ParseExact(prisonDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate= DateTime.ParseExact(prisonDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Bail=prisonDto.Bail,
                    CellId=prisonDto.CellId,
                   Mails= mailsList
                };


                prisonerList.Add(prisoner);
                stBuilder.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisonerList);
            context.SaveChanges();

            string result = stBuilder.ToString().TrimEnd();

            return result;
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder stBuilder = new StringBuilder();

            var xmlSeserializer = new XmlSerializer(typeof(ImportOfficerDto[]),
                                              new XmlRootAttribute("Officers"));

            var officersDto = (ImportOfficerDto[])xmlSeserializer.Deserialize(new StringReader(xmlString));
          
            List<Officer> officerList = new List<Officer>();

            foreach (var officerDto in officersDto)
            {
                if (!IsValid(officerDto))
                {
                    stBuilder.AppendLine("Invalid Data");
                    continue;
                }


                //Check for prisons
                var prisonsList = new List<Prisoner>();

                bool prisonerChecker = true;
                foreach (var prisoner in officerDto.Prisoners)
                {
                    var prisont = context.Prisoners.FirstOrDefault(p => p.Id == prisoner.Id);
                    if (prisoner==null)
                    {
                        prisonerChecker = false;
                        break;
                    }
                    else
                    {
                        prisonsList.Add(prisont);
                    }
                }

                if (!prisonerChecker)
                {
                    stBuilder.AppendLine("Invalid Data");
                    continue;
                }

                //Check Enum
                var enumPosition = new Position();
                var enumWeapon = new Weapon();


                var enumChackPosition = Enum.TryParse<Position>(officerDto.Position, out enumPosition);
                var enumChackWeapon = Enum.TryParse<Weapon>(officerDto.Weapon, out enumWeapon);

                if (!enumChackPosition || !enumChackWeapon)
                {
                    stBuilder.AppendLine("Invalid Data");
                    continue;
                }

                //Check Deparment
                var department = context.Departments.FirstOrDefault(d => d.Id == officerDto.DepartmentId);

                if (department==null)
                {
                    stBuilder.AppendLine("Invalid Data");
                    continue;
                }

                var officer = new Officer
                {
                    FullName = officerDto.FullName,
                    Salary = officerDto.Salary,
                    Position = enumPosition,
                    Weapon = enumWeapon,
                    DepartmentId = officerDto.DepartmentId,
                    Department = department,
                };

                foreach (var prisoner in prisonsList)
                {
                    officer.OfficerPrisoners.Add(new OfficerPrisoner {
                        Prisoner = prisoner,
                        PrisonerId=prisoner.Id,
                        OfficerId= officer.Id,
                        Officer= officer
                    });
                }

                officerList.Add(officer);

                stBuilder.AppendLine($"Imported {officer.FullName} ({prisonsList.Count} prisoners)");


            }

            context.Officers.AddRange(officerList);
            context.SaveChanges();

            string result = stBuilder.ToString().TrimEnd();
            return result;
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validatinResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validatinResult, true);

            return isValid;
        }
    }
}