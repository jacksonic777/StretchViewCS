# StretchViewCS

StretchViewCS is a Windows application for viewing part of the screen with zoom, shrink, rotation, and flip controls.

## Main Features

- Select and mirror a target area on the desktop
- Change the display scale, including shrinking below the original size
- Flip horizontally, flip vertically, rotate left or right, and reset rotation
- Grid lines, center lines, and window size presets
- Color picker with position, HEX, RGB, and color preview
- Save the current view, copy it to the clipboard, and use quick print
- Japanese / English display language switching

## Requirements

- Windows 10 22H2 or later, or Windows 11 recommended
- .NET Framework 4.8
- Win32 API is used for screen capture, color picking, hotkeys, and related desktop features

## Usage

1. Run `StretchViewCS.exe`.
2. Use Selection Range when you want to mirror a specific screen area.
3. Use the toolbar or menus to zoom, shrink, flip, rotate, and show grid overlays.
4. Open the local HTML help from the Help menu.

## Settings

Open Settings from Tools > Settings... or from the Settings button in the main window header.

- Hotkeys are disabled by default.
- The default sampling rate is 400 ms.
- Selection Range is not restored on startup by default.
- Settings are saved to `%AppData%\StretchViewCS\StretchViewCS.ini`.

## Removed Features

The surface layer feature and the screen ruler feature have been removed. They are not part of the normal workflow in the current version.

## License

Before distribution, confirm the license information and bundled files in the repository.
