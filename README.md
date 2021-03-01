# FluxPrompt
Easily launch applications without using the Windows Start Menu. This is inspired by other popular launchers. My hope is to add addional capabilities such as enhanced clipboard functionality, basic C# scripting, and boilerplate text retrieval. 

Version: **Alpha**

## Usage

When launched, FluxPrompt will scan your Start Menu for shortcuts.

Type Control + Space to open FluxPrompt then start typing letters of the application name that you would like to launch. For instance typing "np" will find Notepad, Notepad++ and other programs that have these letters in the name of the shortcut. The applications that you search for most frequently appear at the top of the results.

Type Up and Down keys to select the application that you would like to launch. Type enter to launch it.

Type Escape to minimize FluxPrompt to the notification tray. You may double click on the notification icon to show Flux Prompt again or type Control + Space.

## Roadmap

- Persist history of launches across runs.
- Improve error handling.
- Currently requrires administrator access on every run since scanning the Start Menu requires elevated priviledges. Scanning shortcuts in a separate elevated proccess will aleviate this requirement.
- Installer.
- Add ability to launch an application as administrator.
- Create cusomizable settings.
  - Change hot keys
  - Configure directories to be scanned for shortcuts.
  - Configure directories to be scanned for executables.
- Ability to re-scan shortcuts.
