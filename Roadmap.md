# Roadmap

## Planned Features

- Create customizable settings
  - Change hot keys
  - Configure directories to be scanned for shortcuts and executables
- Installer
- Improve error handling
- Add basic logging of errors and performance metrics
- Enhanced clipboard management and history
- Smart boilerplate text retrieval and management
- Custom automation workflows and shortcuts
- Enable saving of data in JSON format in addition to the custom binary format.

## Known Issues

1. **Start Menu Scanning Issues**:
   - Running without administrator privileges can miss many common shortcuts due to permission errors
   - The application scans shortcuts on every launch, which impacts performance
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
