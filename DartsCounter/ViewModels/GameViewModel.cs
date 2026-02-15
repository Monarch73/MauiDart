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

        private int _dartsThrown = 0;
        public int DartsThrown
        {
            get => _dartsThrown;
            set { _dartsThrown = value; OnPropertyChanged(); }
        }

        private int _scoreAtStartOfTurn;

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
            ScoreCommand = new Command<string>(s => 
            {
                if (int.TryParse(s, out int score))
                {
                    RecordScore(score);
                }
            });
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
            DartsThrown = 0;
            _scoreAtStartOfTurn = 501;
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
                    DartsThrown = state.DartsThrown;
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
                    StatusMessage = StatusMessage,
                    DartsThrown = DartsThrown
                };
                await _syncService.BroadcastState(dto);
            }
        }

        private void AddPlayer(string name)
        {
            if (Players.Count < 8 && !string.IsNullOrWhiteSpace(name))
            {
                var player = new Player(name);
                Players.Add(player);
                if (CurrentPlayer == null) 
                {
                    CurrentPlayer = player;
                    _scoreAtStartOfTurn = player.CurrentScore;
                }
                Broadcast();
            }
        }

        private void RecordScore(int value)
        {
            if (CurrentPlayer == null) return;

            DartsThrown++;
            int points = value * Multiplier;
            int newScore = CurrentPlayer.CurrentScore - points;

            bool isBust = false;
            bool isWin = false;

            if (newScore == 0)
            {
                if (Multiplier == 2)
                {
                    isWin = true;
                    StatusMessage = $"{CurrentPlayer.Name} wins!";
                    CurrentPlayer.CurrentScore = 0;
                }
                else
                {
                    isBust = true;
                    StatusMessage = "Bust! (Double needed)";
                }
            }
            else if (newScore < 2)
            {
                isBust = true;
                StatusMessage = "Bust!";
            }
            else
            {
                CurrentPlayer.CurrentScore = newScore;
                StatusMessage = $"{CurrentPlayer.Name} scored {points}";
            }

            if (isWin)
            {
                // For now, we stay on the winning player.
            }
            else if (isBust)
            {
                CurrentPlayer.CurrentScore = _scoreAtStartOfTurn;
                NextTurn();
            }
            else if (DartsThrown == 3)
            {
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
            public int DartsThrown { get; set; }
        }

        private void NextTurn()
        {
            if (CurrentPlayer == null) return;
            int index = Players.IndexOf(CurrentPlayer);
            index = (index + 1) % Players.Count;
            CurrentPlayer = Players[index];
            _scoreAtStartOfTurn = CurrentPlayer.CurrentScore;
            DartsThrown = 0;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
