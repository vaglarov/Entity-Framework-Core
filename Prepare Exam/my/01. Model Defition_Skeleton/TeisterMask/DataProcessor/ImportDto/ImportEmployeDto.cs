using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class ImportEmployeDto
    {
        //        "Username": "dnapton4",
        //"Email": "asprey4@businessinsider.com",
        //"Phone": "235-815-6395",
        //"Tasks": [
        //  35,
        //  75,
        //  26,
        //  12,
        //  57,
        //  11,
        //  63
        //]
        //}
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        [RegularExpression("[A-z0-9]+")]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [RegularExpression("[0-9]{3}-[0-9]{3}-[0-9]{4}")]
        public string Phone { get; set; }

        [Required]
        public int[] TaskId { get; set; }
    }
}
