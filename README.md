# FluxPrompt
Easily launch applications without using the Windows Start Menu or touching your mouse. This is inspired by other popular launchers. My hope is to add addional capabilities such as enhanced clipboard functionality, basic C# scripting, and boilerplate text retrieval. 

Version: **Alpha**

## Usage

When run for the first time, FluxPrompt will scan your Start Menu for shortcuts.

Type Control + Space to open FluxPrompt then start typing letters of the name of the application that you would like to run. For instance typing "np" will find Notepad, Notepad++ and other programs that have these letters in the name of the shortcut.  The applications that you search for most frequently appear at the top of the results.

Type Up and Down keys to select the application that you would like to run, then type enter to launch it. Alternatively, click on a result to run an application. Typing Alt + Enter will launch the application with administrator priviledges if allowed at UAC prompt. 

Type Escape to minimize FluxPrompt to the notification tray.

You can type Control + Space  to show Flux Prompt again. Double clicking on the notification icon does the same thing.
+- Right-click on the notification icon and select 'Help' to view usage instructions.

## Issues

- Control + Space keyboard shortcut clashes with other common problems. It's been a while but I'm fairly certain it's actually Alt + Space.

## Roadmap

- Improve error handling and add basic logging.
- Running as administrator on the initial run is helpful since scanning the Start Menu without elevated privileges misses many common shortcuts due to permissions errors. Scanning shortcuts in a separate elevated proccess will improve the situation.
- Create cusomizable settings.
  - Change hot keys
  - Configure directories to be scanned for shortcuts.
  - Configure directories to be scanned for executables.
- Ability to re-scan shortcuts.
- Installer.
