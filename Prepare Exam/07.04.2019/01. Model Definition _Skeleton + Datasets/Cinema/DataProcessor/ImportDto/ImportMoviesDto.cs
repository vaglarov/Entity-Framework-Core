using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.DataProcessor.ImportDto
{
   public class ImportMoviesDto
    {

        //{
        //  "Title": "Little Big Man",-----
        //  "Genre": "Western", ---------
        //  "Duration": "01:58:00",
        //  "Rating": 28,
        //  "Director": "Duffie Abrahamson"
        //},

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Title { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public string Duration { get; set; }

        [Required]
        [Range(1, 10)]
        public double Rating { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Director { get; set; }

    }
}
