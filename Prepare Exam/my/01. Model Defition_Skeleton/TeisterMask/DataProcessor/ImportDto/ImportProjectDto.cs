﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Project")]
    public class ImportProjectDto
    {
        //    <Project>
        //<Name>S</Name>
        //<OpenDate>25/01/2018</OpenDate>
        //<DueDate>16/08/2019</DueDate>
        //<Tasks>
        //  <Task>
        //    <Name>Australian</Name>
        //    <OpenDate>19/08/2018</OpenDate>
        //    <DueDate>13/07/2019</DueDate>
        //    <ExecutionType>2</ExecutionType>
        //    <LabelType>0</LabelType>
        //  </Task>

        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [XmlElement("OpenDate")]
        public string OpenDate { get; set; }

        [XmlElement("DueDate")]
        public string DueDate { get; set; }

        [XmlArray("Tasks")]
        public ImportTaskDto[] Tasks { get; set; }

    }
}
