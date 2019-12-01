using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicHub.DataProcessor.ImportDtos
{
    public class ImportProducers
    {

        //        "Name": "Georgi Milkov",
        //"Pseudonym": "Gosho Goshev",
        //"PhoneNumber": "+359 899 345 045",
        //"Albums": [

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }

        [RegularExpression(@"^[A-z][a-z]+ [A-z][a-z]+$")]
        public string Pseudonym { get; set; }

        [RegularExpression(@"^\+359 [0-9]{3} [0-9]{3} [0-9]{3}$")]
        public string PhoneNumber { get; set; }

        public ImportAlbums[] Albums { get; set; }
    }
}
