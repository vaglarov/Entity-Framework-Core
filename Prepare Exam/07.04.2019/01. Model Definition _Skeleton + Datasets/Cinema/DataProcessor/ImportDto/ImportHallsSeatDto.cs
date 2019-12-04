using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.DataProcessor.ImportDto
{
    public class ImportHallsSeatDto
    {
        //        {
        //  "Name": "Methocarbamol",
        //  "Is4Dx": false,
        //  "Is3D": true,
        //  "Seats": 52
        //},

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public string Is4Dx { get; set; }

        [Required]
        public string Is3D { get; set; }

        [Required]
        public string Seats { get; set; }

    }
}
