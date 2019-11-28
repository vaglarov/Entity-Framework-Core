using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportCardDto
    {
        //   "Number": "5208 8381 5687 8508",
        //       "CVC": "624",
        //       "Type": "Debit"

        [Required]
        [RegularExpression("[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}")]
        public string Number { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{3}")]
        public string CVC { get; set; }

        [Required]
        public string Type { get; set; }

    }
}
