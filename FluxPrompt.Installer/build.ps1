# Set the working directory to the repository root
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location -Path $scriptPath\..

# Build the installer using wix.exe
& "C:\Program Files\WiX Toolset v6.0\bin\wix.exe" build $scriptPath\Product.wxs -ext WixToolset.UI.wixext

# Move the output files to bin/Release
Move-Item -Path $scriptPath\Product.msi -Destination $scriptPath\bin\Release\FluxPrompt-Setup-0.1.0-alpha.msi -Force
Move-Item -Path $scriptPath\Product.wixpdb -Destination $scriptPath\bin\Release\FluxPrompt-Setup-0.1.0-alpha.wixpdb -Force

Write-Host "Installer built and moved to bin/Release." 