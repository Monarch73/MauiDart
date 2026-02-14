# DartsCounter Project

## Overview

**DartsCounter** is a cross-platform .NET MAUI application designed to manage and display scores for a game of 501 Double-Out Darts. The system is architected to run on two distinct platforms with specialized roles:

*   **Windows Application:** Acts as the **Controller**. It provides a detailed interface for inputting scores, managing players, and controlling the game flow.
*   **Android Application:** Acts as the **Display**. It serves as a passive, high-visibility scoreboard optimized for viewing by players and spectators.

The two applications synchronize game state in real-time using UDP broadcasting over the local network.

## Key Features

*   **501 Double-Out Logic:** Implements standard game rules, including bust detection and double-out validation.
*   **Real-time Synchronization:** Uses `DartsSyncService` to broadcast game state (current player, score, status) via UDP from the Windows controller to Android displays.
*   **Multi-Platform UI:**
    *   **Controller (Windows):** Inputs for single, double, triple scores (1-20, Bullseye), player management, and game reset.
    *   **Display (Android):** Large, clear typography for current score and active player, plus a summary list of all players.

## Project Structure

The solution follows a standard MVVM (Model-View-ViewModel) architecture:

*   **`DartsCounter/`**: Root project directory.
    *   **`Models/`**: Contains data entities like `Player.cs`.
    *   **`ViewModels/`**: Contains `GameViewModel.cs`, which holds the game logic, state, and commands.
    *   **`Views/`**: Contains the UI definitions:
        *   `ControllerPage.xaml`: Input interface for Windows.
        *   `DisplayPage.xaml`: Scoreboard interface for Android.
    *   **`Services/`**: Contains `DartsSyncService.cs` for UDP network communication.
    *   **`Converters/`**: UI value converters (e.g., `MultiplierToColorConverter.cs`).
    *   **`App.xaml.cs`**: Handles application startup and determines which page to show based on the runtime platform (`DevicePlatform.WinUI` vs. others).
    *   **`MauiProgram.cs`**: Application entry point and dependency configuration.

## Building and Running

Ensure you have the .NET 9.0 SDK and the necessary MAUI workloads installed (`maui-android`, `maui-windows`).

### Windows (Controller)

To build and run the Windows application:

```bash
cd DartsCounter
dotnet build -t:Run -f net9.0-windows10.0.19041.0
```

### Android (Display)

To build and run the Android application (requires an emulator or connected device):

```bash
cd DartsCounter
dotnet build -t:Run -f net9.0-android
```

## Development Conventions

*   **Platform-Specific Logic:** The application uses `DeviceInfo.Platform` checks (primarily in `App.xaml.cs` and `GameViewModel.cs`) to switch behavior between the Controller (Windows) and Display (Android/Mobile) roles.
*   **Networking:** The `DartsSyncService` uses port `50001` for UDP broadcasting. Ensure firewall rules allow local network traffic on this port.
*   **State Management:** `GameViewModel` is the source of truth. Changes here are broadcasted to listeners.
*   **Styling:** UI is defined in XAML.
