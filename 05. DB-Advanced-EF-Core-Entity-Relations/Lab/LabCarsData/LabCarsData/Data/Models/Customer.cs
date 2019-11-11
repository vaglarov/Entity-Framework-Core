
namespace LabCarsData.Data.Models
{
    using static DataValidations.Customer;
using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxFirstNameLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(MaxLastNameLength)]
        public string LastName { get; set; }

        public int Age { get; set; }

        public Address Address { get; set; }

        public ICollection<CarPurchase> Purcheses { get; set; } = new HashSet<CarPurchase>();
    }
}
