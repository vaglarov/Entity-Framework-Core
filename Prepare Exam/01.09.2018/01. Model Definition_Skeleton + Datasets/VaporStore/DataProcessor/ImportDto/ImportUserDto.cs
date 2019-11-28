using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.DataProcessor.ImportDto
{
 //"FullName": "Anita Ruthven",
 //   "Username": "aruthven",
 //   "Email": "aruthven@gmail.com",
 //   "Age": 75,
 //   "Cards": [
 //     {
 //       "Number": "5208 8381 5687 8508",
 //       "CVC": "624",
 //       "Type": "Debit"
 //     }
    public class ImportUserDto
    {
        [Required]
        [RegularExpression("^[A-Z][a-z]+ [A-Z][a-z]+$")]
        public string FullName { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Range(3, 103)]
        public int Age { get; set; }

        public ImportCardDto[] Cards { get; set; }
    }
}




