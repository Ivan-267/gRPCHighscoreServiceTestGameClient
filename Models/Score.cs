using System;

namespace HighscoreServiceClient.Models
{
    public class Score
    {
        public int Id { get; set; }
        public int Points { get; set; }
        public string PlayerName { get; set; }
        public DateTime TimeCreated { get; set; }

        public Score(int id, int points, string playerName, DateTime timeCreated)
        {
            Id = id;
            Points = points;
            PlayerName = playerName;
            TimeCreated = timeCreated;
        }
    }
}
