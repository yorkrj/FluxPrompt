# Set the working directory to the repository root
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location (Split-Path -Parent $scriptPath)

# Create output directory if it doesn't exist
New-Item -ItemType Directory -Force -Path "$scriptPath\bin\Release"

# Build the installer using Inno Setup
& "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" "$scriptPath\setup.iss"

Write-Host "Installer built in bin/Release." 