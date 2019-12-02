using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace MusicHub.DataProcessor.ImportDtos
{
    //  Performers>
    //<Performer>
    //  <FirstName>Peter</FirstName>
    //  <LastName>Bree</LastName>
    //  <Age>25</Age>
    //  <NetWorth>3243</NetWorth>
    //  <PerformersSongs>
    //    <Song id = "2" />
    //    < Song id="1" />
    //  </PerformersSongs
    [XmlType("Performer")]
    public class ImportPerformerDto
    {

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [XmlElement("LastName")]
        public string LastName { get; set; }

        [Required]
        [Range(18, 70)]
        [XmlElement("Age")]
        public int Age { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [XmlElement("NetWorth")]
        public decimal NetWorth { get; set; }

        [Required]
        [XmlArray("PerformersSongs")]
        public ImportSongsMappingDto[] MappingTable { get; set; }
    }
}
