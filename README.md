# FluxPrompt

A lightweight Windows application launcher that helps you launch applications quickly without using the Start Menu or mouse. Inspired by popular launchers like Wox and PowerToys Run, FluxPrompt provides a simple and efficient way to access your applications.

## Features

- 🔍 Quick application search and launch
- ⌨️ Keyboard-first navigation
- 📊 Smart result ranking based on usage frequency
- 🔄 System tray integration
- 🚀 Automatic startup with Windows

## Status

**Work In Progress** - See our [Roadmap](Roadmap.md) for planned features and improvements in addition to known issues.

Future development plans include:
- Enhanced clipboard management and history
- Smart boilerplate text retrieval and management
- Custom automation workflows and shortcuts

## Installation

1. Download the latest release from the [Releases](https://github.com/yorkrj/FluxPrompt/releases) page
2. Run the installer (FluxPrompt-Setup.exe)
3. Follow the installation wizard
4. FluxPrompt will automatically start with Windows

### System Requirements

- Windows 10 version 1809 (build 17763) or later
- .NET 9.0 Runtime

### Uninstallation

1. Open Windows Settings
2. Go to Apps & Features
3. Find FluxPrompt in the list
4. Click Uninstall

## Usage

### Basic Usage

1. Press `Alt + Space` to open FluxPrompt
2. Start typing the name of the application you want to launch
3. Use `Up` and `Down` arrow keys to navigate results
4. Press `Enter` to launch the selected application
5. Press `Escape` to minimize to system tray

### Advanced Features

- `Alt + Enter`: Launch application with administrator privileges
- Double-click system tray icon: Show FluxPrompt
- Right-click system tray icon: Access help and settings

## Development

### Building from Source

1. Clone the repository
2. Open the solution in Visual Studio
3. Restore NuGet packages
4. Build the solution

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Inspired by [Wox](https://github.com/Wox-launcher/Wox) and [PowerToys Run](https://github.com/microsoft/PowerToys)
