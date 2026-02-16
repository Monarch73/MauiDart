using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DartsCounter.Models
{
    public class Player : INotifyPropertyChanged
    {
        private string _name;
        public string Name 
        { 
            get => _name; 
            set { _name = value; OnPropertyChanged(); } 
        }

        private int _currentScore = 501;
        public int CurrentScore 
        { 
            get => _currentScore; 
            set { _currentScore = value; OnPropertyChanged(); } 
        }

        private int _totalPointsScored = 0;
        public int TotalPointsScored
        {
            get => _totalPointsScored;
            set 
            { 
                _totalPointsScored = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(AverageScore)); 
            }
        }

        private int _turnsPlayed = 0;
        public int TurnsPlayed
        {
            get => _turnsPlayed;
            set 
            { 
                _turnsPlayed = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(AverageScore)); 
            }
        }

        public double AverageScore => TurnsPlayed == 0 ? 0 : Math.Round((double)TotalPointsScored / TurnsPlayed, 2);

        public int LegsWon { get; set; } = 0;
        public List<int> ThrowHistory { get; set; } = new List<int>();

        public Player(string name)
        {
            _name = name;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
