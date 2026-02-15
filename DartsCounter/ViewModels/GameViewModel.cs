using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DartsCounter.Models;

namespace DartsCounter.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
        
        private Player? _currentPlayer;
        public Player? CurrentPlayer
        {
            get => _currentPlayer;
            set { _currentPlayer = value; OnPropertyChanged(); }
        }

        private int _multiplier = 1; // 1 = Single, 2 = Double, 3 = Triple
        public int Multiplier
        {
            get => _multiplier;
            set { _multiplier = value; OnPropertyChanged(); }
        }

        private string _statusMessage = "Welcome!";
        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public ICommand AddPlayerCommand { get; }
        public ICommand ScoreCommand { get; }
        public ICommand SetMultiplierCommand { get; }
        public ICommand ResetCommand { get; }

        private readonly Services.DartsSyncService _syncService = new Services.DartsSyncService();

        public GameViewModel()
        {
            AddPlayerCommand = new Command<string>(AddPlayer);
            ScoreCommand = new Command<int>(RecordScore);
            SetMultiplierCommand = new Command<string>(m => 
            {
                if (int.TryParse(m, out int multiplier))
                {
                    Multiplier = multiplier;
                    System.Diagnostics.Debug.WriteLine($"Multiplier set to: {multiplier}");
                }
            });
            ResetCommand = new Command(ResetGame);

            if (DeviceInfo.Platform != DevicePlatform.WinUI)
            {
                _syncService.StartListening(OnStateReceived);
            }
        }

        private void ResetGame()
        {
            Players.Clear();
            CurrentPlayer = null;
            StatusMessage = "Game Reset. Add players.";
            Broadcast();
        }

        private void OnStateReceived(string json)
        {
            try
            {
                var state = System.Text.Json.JsonSerializer.Deserialize<GameStateDto>(json);
                if (state == null) return;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    StatusMessage = state.StatusMessage;
                    // For simplicity, we just update the current player and score display
                    // In a full app, you would sync the entire list.
                    if (CurrentPlayer == null) CurrentPlayer = new Player(state.CurrentPlayerName);
                    CurrentPlayer.Name = state.CurrentPlayerName;
                    CurrentPlayer.CurrentScore = state.CurrentScore;
                });
            }
            catch { }
        }

        private async void Broadcast()
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                var dto = new GameStateDto
                {
                    CurrentPlayerName = CurrentPlayer?.Name ?? "None",
                    CurrentScore = CurrentPlayer?.CurrentScore ?? 501,
                    StatusMessage = StatusMessage
                };
                await _syncService.BroadcastState(dto);
            }
        }

        private void AddPlayer(string name)
        {
            if (Players.Count < 8 && !string.IsNullOrWhiteSpace(name))
            {
                Players.Add(new Player(name));
                if (CurrentPlayer == null) CurrentPlayer = Players[0];
                Broadcast();
            }
        }

        private void RecordScore(int value)
        {
            if (CurrentPlayer == null) return;

            int points = value * Multiplier;
            int newScore = CurrentPlayer.CurrentScore - points;

            if (newScore == 0)
            {
                if (Multiplier == 2)
                {
                    StatusMessage = $"{CurrentPlayer.Name} wins!";
                    CurrentPlayer.CurrentScore = 0;
                }
                else
                {
                    StatusMessage = "Bust! (Double needed)";
                    NextTurn();
                }
            }
            else if (newScore < 2)
            {
                StatusMessage = "Bust!";
                NextTurn();
            }
            else
            {
                CurrentPlayer.CurrentScore = newScore;
                StatusMessage = $"{CurrentPlayer.Name} scored {points}";
                NextTurn();
            }
            
            Multiplier = 1;
            Broadcast();
        }

        public class GameStateDto
        {
            public string CurrentPlayerName { get; set; } = string.Empty;
            public int CurrentScore { get; set; }
            public string StatusMessage { get; set; } = string.Empty;
        }

        private void NextTurn()
        {
            if (CurrentPlayer == null) return;
            int index = Players.IndexOf(CurrentPlayer);
            index = (index + 1) % Players.Count;
            CurrentPlayer = Players[index];
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
