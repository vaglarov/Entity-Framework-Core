namespace VaporStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Tag
    {
        //•	Id – integer, Primary Key
        public Tag()
        {
            this.GameTags = new HashSet<GameTag>();
        }

        public int Id { get; set; }

        //•	Name – text(required)
        [Required]
        public string  Name { get; set; }

        //•	GameTags - collection of type GameTag
        public ICollection<GameTag> GameTags{get;set;}
    }
}
