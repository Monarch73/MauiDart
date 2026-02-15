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
