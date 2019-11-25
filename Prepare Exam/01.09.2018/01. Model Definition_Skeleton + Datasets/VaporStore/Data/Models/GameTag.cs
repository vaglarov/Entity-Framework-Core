namespace VaporStore.Data.Models
{
    public class GameTag
    {
        //•	GameId – integer, Primary Key, foreign key(required)
        //•	TagId – integer, Primary Key, foreign key(required)
        //•	Game – Game
        //•	Tag – Tag
        //TODO add composite primary key
        public int GameId { get; set; }
        public Game Game { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
