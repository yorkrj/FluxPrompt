## Roadmap

- Create cusomizable settings.
  - Change hot keys
  - Configure directories to be scanned for shortcuts.
  - Configure directories to be scanned for executables.
- Ability to re-scan shortcuts.
- Installer.
- Improve error handling.
- Add basic logging of errors and performance metrics.
- Running as administrator on the initial run can sometimes be helpful since scanning the Start Menu without elevated privileges misses many common shortcuts due to permissions errors. Scanning shortcuts in a separate elevated proccess will improve the situation.

## Known Issues

1. **Start Menu Scanning Issues**:
   - Running without administrator privileges can miss many common shortcuts due to permission errors
   - The application scans shortcuts on every launch, which impacts performance
   - There's no mechanism to refresh/rescan shortcuts when needed
   - No progress indicator during initial shortcut scanning

2. **Error Handling**:
   - Basic error handling exists but needs standardization
   - Some error messages are generic and not user-friendly
   - No logging system in place for errors and performance metrics

3. **Hot Key Management**:
   - Only one hot key (Alt+Space) is currently tracked and managed
   - The hot key handler needs to be improved to track multiple registered hot keys
   - No validation for hot key conflicts with other applications

4. **Configuration**:
   - Settings are stored in JSON format but there's no validation of the configuration structure
   - No default configuration fallback if the config file is corrupted
   - Limited configuration options available
