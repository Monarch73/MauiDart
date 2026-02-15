# DartsCounter Project

## Overview

**DartsCounter** is a cross-platform .NET MAUI application designed to manage and display scores for a game of 501 Double-Out Darts. The system is architected to run on two distinct platforms with specialized roles:

*   **Windows Application:** Acts as the **Controller**. It provides a detailed interface for inputting scores, managing players, and controlling the game flow.
*   **Android Application:** Acts as the **Display**. It serves as a passive, high-visibility scoreboard optimized for viewing by players and spectators.

The two applications synchronize game state in real-time using UDP broadcasting over the local network.

## Key Features

*   **501 Double-Out Logic:** Implements standard game rules, including three-dart turns, bust detection (reverting to start-of-turn score), and double-out validation.
*   **Real-time Synchronization:** Uses `DartsSyncService` to broadcast game state (current player, score, status, and darts thrown) via UDP from the Windows controller to Android displays.
*   **Multi-Platform UI:**
    *   **Controller (Windows):** Inputs for single, double, triple scores (1-20, Bullseye), player management, turn progress tracking, real-time player list with scores, and game reset. Optimized for full-height display.
    *   **Display (Android):** Large, clear typography for current score, active player, and darts thrown in current turn, plus a summary list of all players.

## Project Structure

The solution follows a standard MVVM (Model-View-ViewModel) architecture:

*   **`DartsCounter/`**: Root project directory.
    *   **`Models/`**: Contains data entities like `Player.cs` (implements `INotifyPropertyChanged`).
    *   **`ViewModels/`**: Contains `GameViewModel.cs`, which holds the game logic, state, 3-dart turn management, and commands.
    *   **`Views/`**: Contains the UI definitions:
        *   `ControllerPage.xaml`: Input interface for Windows with turn tracking and player list.
        *   `DisplayPage.xaml`: Scoreboard interface for Android with dart counter.
    *   **`Services/`**: Contains `DartsSyncService.cs` for UDP network communication.
    *   **`Converters/`**: UI value converters (e.g., `MultiplierToColorConverter.cs`).
    *   **`App.xaml.cs`**: Handles application startup, platform routing, and Windows window sizing.
    *   **`MauiProgram.cs`**: Application entry point and dependency configuration.

## Building and Running

Ensure you have the .NET 9.0 SDK and the necessary MAUI workloads installed (`maui-android`, `maui-windows`).

### Windows (Controller)

To build and run the Windows application:

```bash
cd DartsCounter
dotnet build -t:Run -f net9.0-windows10.0.26100.0
```

### Android (Display)

To build and run the Android application (requires an emulator or connected device):

```bash
cd DartsCounter
dotnet build -t:Run -f net9.0-android
```

## Development Conventions

*   **Platform-Specific Logic:** The application uses `DeviceInfo.Platform` checks to switch behavior. Windows acts as the controller and sets initial window height to screen resolution.
*   **Turn Logic:** Each player has 3 darts per turn. A "Bust" occurs if a player scores more than their remaining points or reaches 1 without a double-out. On bust, the score reverts to the value at the start of that turn.
*   **Networking:** The `DartsSyncService` uses port `50001` for UDP broadcasting. The state DTO includes `DartsThrown` to keep displays in sync with the turn progress.
*   **State Management:** `GameViewModel` is the source of truth. `Player` models notify the UI of property changes.
