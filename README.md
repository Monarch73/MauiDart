# DartsCounter 🎯

[![Build Status](https://img.shields.io/badge/status-work--in--progress-orange)](https://github.com/)
[![Framework](https://img.shields.io/badge/.NET-9.0-blue)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Android-green)](#)

**DartsCounter** is a cross-platform .NET MAUI application designed to manage and display scores for 501 Double-Out Darts. It features a unique dual-mode architecture optimized for local network play.

> ⚠️ **Note:** This project is currently **Work in Progress (WIP)**. Many features are still under development and the UI is subject to change.

---

## 🚀 Concept: Controller & Display

DartsCounter is designed to run on two distinct platforms simultaneously:

- **Windows (The Controller):** Serves as the primary input device. Typically used on a tablet or laptop near the dartboard to record scores, manage players, and control game flow.
- **Android/Mobile (The Display):** Serves as a high-visibility scoreboard. Designed to be run on a tablet or phone mounted near the board or held by spectators, updating in real-time via UDP broadcasting.

---

## ✨ Features

### Current Capabilities
- **501 Double-Out Logic:** Automated score calculation with full support for "Bust" rules and mandatory double-out validation.
- **Real-time Synchronization:** Uses `DartsSyncService` for low-latency UDP broadcasting across the local network (Port 50001).
- **Multi-Player Support:** Add and manage up to 8 players in a single game.
- **Dynamic UI:** Automatic role detection—Windows launches the Controller interface, while Android/iOS launches the Display interface.
- **Score Multipliers:** Quick-toggle inputs for Single, Double, and Triple scores.

### Planned Features 🛠️
- [ ] **Full State Synchronization:** Enhancing network protocols to sync the complete player list and historical throws.
- [ ] **Game Variations:** Support for 301, Cricket, and Around the Clock.
- [ ] **Statistics:** Track averages (PPR), checkout percentages, and high scores.
- [ ] **Custom Themes:** Dark/Light mode and customizable color schemes for high-visibility scoring.
- [ ] **Undo/Redo:** Ability to correct mis-entered scores easily.

---

## 🛠️ Technology Stack

- **Framework:** .NET MAUI (.NET 9.0)
- **Architecture:** MVVM (Model-View-ViewModel)
- **Networking:** UDP Broadcasting for local device discovery and sync
- **Language:** C# / XAML

---

## 📋 Prerequisites

- **.NET 9.0 SDK**
- **MAUI Workloads:** `maui-android`, `maui-windows` (and optionally `maui-ios`)
- **IDE:** Visual Studio 2022 (with MAUI workload) or VS Code with .NET MAUI extension.

---

## 🏃 How to Run

### 1. Windows (Controller)
```bash
cd DartsCounter
dotnet build -t:Run -f net9.0-windows10.0.19041.0
```

### 2. Android (Display)
Ensure your device/emulator is connected:
```bash
cd DartsCounter
dotnet build -t:Run -f net9.0-android
```

*Note: Both devices must be on the same local network for synchronization to work.*

---

## 🤝 Contributing

Since this is a WIP, feedback and contributions are welcome! Feel free to open issues for bug reports or feature suggestions.

---

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details (if applicable).
