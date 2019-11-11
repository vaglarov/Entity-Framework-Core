namespace P03_FootballBetting.Data.Models
{
   public class PlayerStatistic
    {
        //GameId, PlayerId, ScoredGoals, Assists, MinutesPlayed
        public int GameId { get; set; }
        //nav prop

        public int PlayerId { get; set; }
        //nav prop

        public int ScoredGoals { get; set; }
        public int Assists { get; set; }
        public int MinutesPlayed { get; set; }
    }
}
