
namespace LabCarsData.Data.Models
{
    using static DataValidations.Customer;
using System.ComponentModel.DataAnnotations;
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxFirstNameLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(MaxLastNameLength)]
        public string LastName { get; set; }

        public int Year { get; set; }


    }
}
