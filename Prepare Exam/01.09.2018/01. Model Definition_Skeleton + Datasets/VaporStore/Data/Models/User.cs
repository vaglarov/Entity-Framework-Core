namespace VaporStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class User
    {
        public User()
        {
            this.Cards = new HashSet<Card>();
        }
        //•	Id – integer, Primary Key
        [Required]
        public int Id { get; set; }

        //•	Username – text with length[3, 20] (required)
        [Required]
        [MinLength(3), MaxLength(20)]
        public string Username { get; set; }

        //•	FullName – text, which has two words, consisting of Latin letters.Both start with an upper letter and are separated by a single space(ex. "John Smith") (required)
        [Required]
        [RegularExpression("^[A-Z][a-z]+ [A-Z][a-z]+$")]
        public string FullName { get; set; }

        //•	Email – text(required)
        [Required]
        public string Email { get; set; }

        //•	Age – integer in the range[3, 103] (required)
        [Required]
        [Range(3, 103)]
        public int Age { get; set; }

        //•	Cards – collection of type Card
        public ICollection<Card> Cards { get; set; }

    }
}
