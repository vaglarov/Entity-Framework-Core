using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace MusicHub.DataProcessor.ImportDtos
{    //<PerformersSongs>
    //    <Song id = "2" />
    [XmlType("Song")]
    public class ImportSongsMappingDto
    {
        [XmlAttribute("id")]
        [Required]
        public int DtoId { get; set; }
    }
}
