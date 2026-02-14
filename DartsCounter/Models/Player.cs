namespace DartsCounter.Models
{
    public class Player
    {
        public string Name { get; set; }
        public int CurrentScore { get; set; } = 501;
        public int LegsWon { get; set; } = 0;
        public List<int> ThrowHistory { get; set; } = new List<int>();

        public Player(string name)
        {
            Name = name;
        }
    }
}
