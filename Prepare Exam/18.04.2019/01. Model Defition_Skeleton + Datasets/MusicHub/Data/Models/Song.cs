using MusicHub.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models
{
    public class Song
    {
        //•	Id – integer, Primary Key
        //•	Name – text with min length 3 and max length 20 (required)
        //•	Duration – TimeSpan(required)
        //•	CreatedOn – Date(required)
        //•	Genre ¬– Genre enumeration with possible values: "Blues, Rap, PopMusic, Rock, Jazz" (required)
        //•	AlbumId– integer foreign key
        //•	Album– the song’s album
        //•	WriterId - integer, foreign key(required)
        //•	Writer – the song’s writer
        //•	Price – decimal (non-negative, minimum value: 0) (required)
        //•	SongPerformers - collection of type SongPerformer
        public Song()
        {
            this.SongPerformers = new HashSet<SongPerformer>();
        }
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public Genre Genre { get; set; }

        public int? AlbumId { get; set; }

        public Album Album { get; set; }

        [Required]
        public int WriterId { get; set; }

        public Writer Writer { get; set; }


        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        public ICollection<SongPerformer> SongPerformers { get; set; }

    }
}
