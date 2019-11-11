namespace LabCarsData.Data.Models
{
    using static DataValidations.Car;

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Car
    {
        public int CarId { get; set; }

        public DateTime ProductionDate { get; set; }

        [Required]
        [MaxLength(MaxVINLength)]
        public string VIN { get; set; }

        public decimal Price { get; set; }


        [Required]
        [MaxLength(MaxColorLength)]
        public string Color { get; set; }

        public int ModelId { get; set; }
        public Model Model { get; set; }

        public ICollection<CarPurchase> Owners { get; set; } = new HashSet<CarPurchase>();

    }
}
