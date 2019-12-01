using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MusicHub.Data.Models
{
    public class Album
    {
        //•	Id – integer, Primary Key
        //•	Name – text with min length 3 and max length 40 (required)
        //•	ReleaseDate – Date(required)
        //•	Price – calculated property(the sum of all song prices in the album)
        //•	ProducerId – integer foreign key
        //•	Producer – the album’s producer
        //•	Songs – collection of all songs in the album
        private decimal price;

        public Album()
        {
            this.Songs = new HashSet<Song>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public decimal Price
          => this.Songs.Sum(p => p.Price);

        //TODO: can br nulable
        public int? ProducerId { get; set; }

        public Producer Producer { get; set; }

        public ICollection<Song> Songs { get; set; }
    }
}
