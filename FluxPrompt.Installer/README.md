# FluxPrompt Installer

This directory contains the installer configuration for FluxPrompt using Inno Setup.

## Prerequisites

1. Install Inno Setup from https://jrsoftware.org/isdl.php
2. Build the main application in Release mode:
   ```powershell
   dotnet build ..\FluxPrompt\FluxPrompt.csproj -c Release
   ```

## Building the Installer

Run the build script:
```powershell
.\build.ps1
```

The installer will be created in `bin\Release\FluxPrompt-Setup.exe`.

## Installation

1. Run `FluxPrompt-Setup.exe`
2. Choose whether to:
   - Create a desktop shortcut
   - Run at Windows startup
3. Complete the installation

## Data Storage

FluxPrompt stores its data files in the user's AppData folder:
- `%APPDATA%\FluxPrompt\Links.dat` - Stores shortcut information
- `%APPDATA%\FluxPrompt\History.dat` - Stores launch history

This ensures the application can write data without requiring elevated permissions. 