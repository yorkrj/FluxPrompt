# FluxPrompt Installer

This directory contains the Windows Installer (MSI) setup for FluxPrompt.

## Prerequisites

1. Install WiX Toolset v6.0 or later from [https://wixtoolset.org/releases/](https://wixtoolset.org/releases/)
2. Ensure the WiX Toolset bin directory is in your system PATH (typically `C:\Program Files\WiX Toolset v6.0\bin`)

## Building the Installer

### Option 1: Using the PowerShell Build Script

1. Open a PowerShell terminal in the root directory of the project.
2. Run the build script:
   ```powershell
   .\FluxPrompt.Installer\build.ps1
   ```
   This will build the installer and move the output files to `FluxPrompt.Installer\bin\Release`.

### Option 2: Manual Build

1. Open a PowerShell terminal in the root directory of the project.
2. Run the following command:
   ```powershell
   "C:\Program Files\WiX Toolset v6.0\bin\wix.exe" build FluxPrompt.Installer\Product.wxs -ext WixToolset.UI.wixext
   ```
3. The installer will be created in the `FluxPrompt.Installer` directory as `Product.msi`.

## File Structure

- `Product.wxs` - Main WiX source file containing installer configuration
- `bin/Release/` - Output directory for the installer
  - `FluxPrompt-Setup-v1.0.0.msi` - The Windows Installer package
  - `FluxPrompt-Setup-v1.0.0.wixpdb` - Debug database (can be deleted)

## Customizing the Installer

### Changing the Version

1. Update the `Version` attribute in `Product.wxs`
2. Update the `Name` attribute to match the new version
3. Rebuild the installer

### Adding Files

1. Add new `<File>` elements inside the `ApplicationFiles` component
2. Ensure the source paths are relative to the repository root
3. Rebuild the installer

### Modifying Installation Options

The installer uses the `WixUI_InstallDir` dialog set, which provides:
- Installation directory selection
- Feature selection
- Progress display
- Completion dialog

To modify these options, edit the `<UI>` element in `Product.wxs`.

## Troubleshooting

If you encounter build errors:
1. Verify all source file paths are correct
2. Ensure the WiX Toolset is properly installed
3. Check that the main application has been built in Release mode
4. Verify the WiX Toolset bin directory is in your PATH

## Notes

- The installer automatically adds FluxPrompt to Windows startup
- Uninstallation is available through Windows Settings > Apps
- The installer creates shortcuts in the Start Menu 