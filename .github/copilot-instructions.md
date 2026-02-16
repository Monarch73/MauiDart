# DartsCounter - AI Copilot Instructions

## Project Overview

**DartsCounter** is a cross-platform .NET MAUI application for managing 501 Double-Out darts scores with a dual-mode architecture:
- **Windows (Controller):** Input device for managing players and recording scores
- **Android/Mobile (Display):** Real-time scoreboard synced via UDP broadcasting on port 50001
- **Framework:** .NET MAUI 9.0 targeting Windows, Android, and macCatalyst

## Architecture & Data Flow

### Key Components

1. **GameViewModel** (`ViewModels/GameViewModel.cs`)
   - Central state management using MVVM pattern (implements `INotifyPropertyChanged`)
   - Manages player collection, current player, multiplier state, and game logic
   - Exposes commands: `AddPlayerCommand`, `ScoreCommand`, `SetMultiplierCommand`, `ResetCommand`
   - Contains scoring logic for 501 Double-Out rules (bust detection, double-out validation)

2. **DartsSyncService** (`Services/DartsSyncService.cs`)
   - UDP-based synchronization service on port 50001
   - **Server side (Windows):** `BroadcastState()` broadcasts `GameStateDto` as JSON
   - **Client side (Android):** `StartListening()` listens for state updates
   - Platform-aware: initialized only on non-WinUI platforms to avoid dual broadcasting

3. **Player Model** (`Models/Player.cs`)
   - Data model with observable properties (implements `INotifyPropertyChanged`)
   - Core properties: `Name`, `CurrentScore` (starts at 501), `TotalPointsScored`, `TurnsPlayed`
   - Calculated property: `AverageScore` (PPR = TotalPointsScored / TurnsPlayed)
   - Tracks `ThrowHistory` and `LegsWon`

4. **Views**
   - **SetupPlayers (`Views/SetupPlayers.xaml`):** Initial startup page for Windows (Controller).
     - Manages the player list (max 8 players).
     - Transitions to `ControllerPage` when the match starts.
   - **ControllerPage (`Views/ControllerPage.xaml`):** Windows controller interface (Match Phase) with:
     - Score input buttons (1-20, Bullseye with single/double/triple multipliers)
     - Turn progress tracking (darts thrown counter)
     - Real-time player list with current scores
     - Game reset functionality
   - **DisplayPage (`Views/DisplayPage.xaml`):** Android display interface optimized for viewing with:
     - Large typography for current player name and score
     - Darts thrown counter for current turn
     - Summary list of all players and their scores
   - **App.xaml.cs:** Platform detection routes to appropriate view (Windows starts with `SetupPlayers` within a `NavigationPage`); handles Windows window sizing to screen resolution.

## Darts Game Logic (501 Double-Out)

### Scoring Rules Implemented in GameViewModel.RecordScore()

1. **Multipliers:** Single (1x), Double (2x), Triple (3x) applied to score value
2. **Bust Conditions:**
   - Final score = 0 without a double → **Bust** (revert to start-of-turn score)
   - Final score < 2 → **Bust** (insufficient points for finish)
   - Final score = 1 → **Bust** (impossible to finish with double)
3. **Valid Finish:** Score exactly 0 on a double multiplier
4. **Turn Management:**
   - 3 darts per turn (tracked by `DartsThrown`)
   - After 3 darts or bust: advance to next player via `NextTurn()`
   - Turn stats (points + turn count) recorded before switching

### State Management During Turn

- `_scoreAtStartOfTurn`: Snapshot before first dart (for bust reversion)
- `_pointsThisTurn`: Cumulative points in current turn
- `CurrentPlayer.CurrentScore`: Live score (decremented after each dart)
- `Multiplier`: Reset to 1 after each dart recorded

## Data Synchronization Pattern

### Windows → Android Flow
1. `RecordScore()` or `NextTurn()` updates local state
2. `Broadcast()` called if `DeviceInfo.Platform == DevicePlatform.WinUI`
3. `DartsSyncService.BroadcastState()` serializes `GameStateDto` as JSON → UDP broadcast
4. Android listener receives JSON via `StartListening()` callback
5. `OnStateReceived()` deserializes and updates UI on main thread

### GameStateDto Structure
```csharp
CurrentPlayerName, CurrentScore, StatusMessage, DartsThrown
```

**Note:** Full player list sync is pending (marked as "Planned Features" in README). Currently only critical turn info syncs.

## Development Conventions

### Code Style & Patterns

- **MVVM:** All ViewModels implement `INotifyPropertyChanged` with `OnPropertyChanged()` helper
- **Commands:** Use `Command` and `Command<T>` from MAUI for button bindings
- **Observable Collections:** Used for dynamic player lists in UI bindings
- **Value Converters:** Implement `IValueConverter` for XAML binding transformations (e.g., `MultiplierToColorConverter` highlights active multiplier)

### Platform-Specific Code

- Use `DeviceInfo.Platform` checks to branch behavior (e.g., Controller vs. Display)
- UDP broadcast uses `IPAddress.Broadcast` (network-dependent; real deployments may need specific IP)
- Networking runs on background task via `Task.Run()` to avoid blocking UI thread

### Async Patterns

- `BroadcastState()` is async; called via `await` in `Broadcast()`
- `ReceiveAsync()` in listening loop to prevent thread blocking
- UI updates from background threads must use `MainThread.BeginInvokeOnMainThread()`

## Key Files Reference

| File | Purpose | Key Responsibility |
|------|---------|-------------------|
| `GameViewModel.cs` | Main game logic | Scoring, turn management, command handling |
| `DartsSyncService.cs` | Network sync | UDP broadcast/listen coordination |
| `Player.cs` | Data model | Player state + calculated statistics |
| `SetupPlayers.xaml` | Windows UI (Setup) | Initial player entry and game initialization |
| `ControllerPage.xaml(.cs)` | Windows UI (Match) | Input interface (score buttons, multiplier toggles) |
| `DisplayPage.xaml(.cs)` | Android UI | Live scoreboard display |
| `App.xaml.cs` | App entry | Platform routing logic |
| `MauiProgram.cs` | DI container | MAUI service setup |

## Common Workflows

### Adding a New Game Feature (e.g., 301 variant)

1. Extend `Player.cs` with variant-specific properties (e.g., `GameType` enum)
2. Update `GameViewModel.RecordScore()` with variant logic (conditioned by `CurrentGame?.GameType`)
3. Broadcast variant state in `GameStateDto`
4. Adjust UI validation (e.g., finish conditions differ for 301)

### Debugging Networking Issues

- Check `DartsSyncService` error messages in Debug console
- Verify port 50001 is accessible on local network (firewall rules)
- Network may block broadcast; fallback to multicast or specific IP needed
- Add verbose logging to `BroadcastState()` and `StartListening()` for diagnostics

### Adding Persistent Storage

- Player profiles/history would require serialization of `ThrowHistory` and `LegsWon`
- Consider JSON file or local SQLite database per platform
- Initialize in `App` constructor before loading game state

## Build & Test Commands

Ensure .NET 9.0 SDK and MAUI workloads are installed (`maui-android`, `maui-windows`).

```bash
# Build & run Windows (Controller)
dotnet build -t:Run -f net9.0-windows10.0.26100.0

# Build & run Android (Display) - requires emulator or connected device
dotnet build -t:Run -f net9.0-android

# Build only (no run)
dotnet build -f net9.0-windows10.0.26100.0
dotnet build -f net9.0-android
```

## Current Limitations & Planned Work

- **WIP Status:** Many features incomplete (see README "Planned Features")
- **Partial Sync:** Only critical turn state synced; full player list sync pending
- **No Undo/Redo:** Mis-entered scores require manual game reset
- **Network Fragility:** Broadcast-based discovery not reliable on restricted networks
- **Single Game Instance:** No support for multiple simultaneous games
