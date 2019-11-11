namespace LabCarsData.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Models;
    using static DataValidations.Address;
    public class Address
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxTextLength)]
        public string Text { get; set; }

        [Required]
        [MaxLength(MaxTownLength)]
        public string Town { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
     }
}
