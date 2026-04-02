# DartMaster Project

## Overview

**DartMaster** is a cross-platform .NET MAUI application designed to manage and display scores for a game of 501 Double-Out Darts. The system is architected to run on two distinct platforms with specialized roles:

*   **Windows Application:** Acts as the **Controller**. It provides a detailed interface for inputting scores, managing players, and controlling the game flow.
*   **Android Application:** Acts as the **Display**. It serves as a passive, high-visibility scoreboard optimized for viewing by players and spectators.

The two applications synchronize game state in real-time using UDP broadcasting over the local network.

## Key Features

*   **501 Double-Out Logic:** Implements standard game rules, including three-dart turns and validation:
    *   **Bust Conditions:** Occur if a player scores more than their remaining points, reaches a final score of 1, or reaches 0 without a double. On bust, the score reverts to the value at the start of that turn.
    *   **Valid Finish:** A player wins by reaching exactly 0 points with a **double multiplier**.
*   **Real-time Synchronization:** Uses `DartsSyncService` to broadcast game state via UDP (port 50001) from the Windows controller to Android displays.
*   **Screen Wake Lock:** Automatically keeps the device screen active while the application is in the foreground, ensuring the scoreboard remains visible throughout the match.
*   **Multi-Platform UI:**
    *   **Controller (Windows):** 
        *   **Setup Phase:** Starts on `SetupPlayers.xaml` to manage the player list (max 8).
        *   **Match Phase:** Transitions to `ControllerPage.xaml` for score input (1-20, Bullseye), multipliers (Single, Double, Triple), and turn tracking.
    *   **Display (Android):** Large, clear typography for current score, active player, and darts thrown, plus a summary list of all players.

## Project Structure

The solution follows a standard MVVM (Model-View-ViewModel) architecture:

*   **`src/DartMaster/`**: Root project directory.
    *   **`Models/`**: 
        *   `Player.cs`: Implements `INotifyPropertyChanged`. Tracks `Name`, `CurrentScore`, `TotalPointsScored`, `TurnsPlayed`, `LegsWon`, and `ThrowHistory`. Calculates `AverageScore` (PPR).
    *   **`ViewModels/`**: 
        *   `GameViewModel.cs`: Central game logic and state. Manages the player collection and 3-dart turns. Exposes `AddPlayerCommand`, `ScoreCommand`, `SetMultiplierCommand`, and `ResetCommand`.
    *   **`Views/`**: 
        *   `SetupPlayers.xaml`: **Startup page for Windows**. Handles player entry and initializes the game state.
        *   `ControllerPage.xaml`: Main input interface for the match. Receives the shared `GameViewModel` from the setup page.
        *   `DisplayPage.xaml`: Scoreboard interface for Android.
    *   **`Services/`**: 
        *   `DartsSyncService.cs`: Manages UDP communication. Windows broadcasts `GameStateDto` (JSON); Android listens and updates UI via `MainThread.BeginInvokeOnMainThread()`.
    *   **`Converters/`**: UI value converters (e.g., `MultiplierToColorConverter.cs`).
    *   **`App.xaml.cs`**: Handles application startup and platform-based routing. Windows is configured to start with `SetupPlayers` within a `NavigationPage`.

## Networking Details

*   **Broadcast Port:** 50001
*   **GameStateDto:** Includes `CurrentPlayerName`, `CurrentScore`, `StatusMessage`, and `DartsThrown`.
*   **Roles:** Windows acts as the server (broadcasts); Android acts as the client (listens). This prevents feedback loops and dual broadcasting.

## Building and Running

Ensure you have the .NET 10.0 SDK and the necessary MAUI workloads installed (`maui-android`, `maui-windows`).

### Windows (Controller)
```bash
dotnet build src/DartMaster/DartMaster.csproj -t:Run -f net10.0-windows10.0.26100.0
```

### Android (Display)
```bash
dotnet build src/DartMaster/DartMaster.csproj -t:Run -f net10.0-android
```

## Technical Notes

*   **Android Font Assets:** To ensure compatibility with physical Android devices, font assets in `Resources/Fonts` must use all-lowercase filenames with no special characters (e.g., `opensansregular.ttf`).

## Current Limitations & WIP

*   **Partial Sync:** Only critical turn state is currently synchronized; full player list synchronization is planned for a future update.
*   **No Undo:** There is currently no functionality to undo a mis-entered score; the game must be reset or managed manually.
*   **Network Stability:** UDP broadcasting is dependent on local network configuration and may be blocked by certain firewalls or router settings.
