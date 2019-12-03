﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.Data.Models
{
    public class Hall
    {
        //•	Id – integer, Primary Key
        //•	Name – text with length[3, 20] (required)
        //•	Is4Dx - bool
        //•	Is3D - bool
        //•	Projections - collection of type Projection
        //•	Seats - collection of type Seat


         public Hall()
        {
            this.Seats = new HashSet<Seat>();
            this.Projections = new HashSet<Projection>();
        }
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }

        public bool Is4Dx { get; set; }

        public bool Is3D { get; set; }

        public ICollection<Projection> Projections { get; set; }

        public ICollection<Seat> Seats { get; set; }


    }
}
