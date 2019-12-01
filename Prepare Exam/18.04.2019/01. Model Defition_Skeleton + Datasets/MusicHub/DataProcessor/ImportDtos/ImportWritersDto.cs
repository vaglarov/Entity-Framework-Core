using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicHub.DataProcessor.ImportDtos
{
   public class ImportWritersDto
    {
        //      {
        //  "Name": "Mik Jonathan",
        //  "Pseudonym": "The Mik"
        //},
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }

        [RegularExpression("[A-z][a-z]+ [A-z][a-z]+")]
        public string Pseudonym { get; set; }
    }
}
