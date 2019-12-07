using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Task")]
   public class ImportTaskDto
    {
        // <Task>
        //    <Name>Australian</Name>
        //    <OpenDate>19/08/2018</OpenDate>
        //    <DueDate>13/07/2019</DueDate>
        //    <ExecutionType>2</ExecutionType>
        //    <LabelType>0</LabelType>

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        [XmlElement("Name")]
        public string Name { get; set; }


        [Required]
        [XmlElement("OpenDate")]
        public string OpenDate { get; set; }


        [Required]
        [XmlElement("DueDate")]
        public string DueDate { get; set; }

        [Required]
        [XmlElement("ExecutionType")]
        public string ExecutionType { get; set; }

        [Required]
        [XmlElement("LabelType")]
        public string LabelType { get; set; }
    }
}
