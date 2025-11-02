# Advanced Screen Ruler

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A versatile, on-screen measurement tool for Windows, built with C# and Windows Forms. What started as a simple ruler has evolved into a powerful 2D measurement and annotation tool, perfect for designers, engineers, and game developers.

It allows you to measure pixels, calibrate to real-world units, and analyze geometry on a static, pannable canvas captured from your screen.

*It is highly recommended to replace the screenshot below with an animated GIF showcasing the new drawing modes and features!*

![Advanced Screen Ruler v3.0 in action](images/screenshotv3.png)

## ✨ Features

-   **Comprehensive Drawing Toolkit**: Switch between multiple tools to measure anything you need.
    -   **Lines**: Measure distance between two points.
    -   **Angles**: Three-point angle measurement with inner/outer angle toggle.
    -   **Circles**: Define by center and radius, with automatic calculations.
    -   **Rectangles**: Define by diagonal corners.
    -   **Grids**: Create rectangular grids with adjustable cell size (in calibrated units).
    -   **Markers**: Place markers to highlight points of interest and display their coordinates.

-   **Advanced Precision Tools**: Draw with CAD-like accuracy.
    -   **Snap to Points (S)**: Automatically snap the cursor to the nearest point of any existing shape. Snap radius is adjustable.
    -   **Guides (D)**: Display alignment guides when the cursor is horizontally or vertically aligned with other snap points.
    -   **Axis Lock (Shift)**: Restrict drawing to horizontal or vertical axes.
    -   **Cursor Lock (Ctrl)**: Hold to lock the cursor's position for precise clicks.

-   **Canvas & Background Capture**: Work on a static image of your screen.
    -   **Capture (C)**: Take a screenshot of the current monitor to use as a static background. The ruler becomes fully opaque for clarity.
    -   **Pannable Canvas**: Pan the background and all drawn shapes using the **Arrow Keys**.
    -   **Two Drag Modes**:
        -   **Default Drag**: Moves the window and the canvas together.
        -   **Viewport Drag (Shift + Drag)**: Moves only the window over the static canvas, like a camera viewport.
    -   **Adjustable Overlay**: Use **Shift + Mouse Wheel** to change the opacity of the color overlay for perfect contrast.
    -   **Clear (X)**: Remove the background and return to live mode.

-   **Interactive Recalibration (R)**: Enter Recalibration mode, click on any drawn line, and enter its real-world length to instantly recalibrate the entire canvas.

-   **Full Session Management**: Never lose your work.
    -   **Save/Load Session**: Save all your drawn shapes, window position, size, canvas pan, and calibration settings to a single `.sez` (Session Zipped) file. The captured background is included in the save file.
    -   **File Association**: Opens `.sez` files passed as a command-line argument.

-   **In-App Help & UI Enhancements**:
    -   Press **H** to toggle an on-screen help panel with all hotkeys.
    -   Context-sensitive information appears next to the cursor (snap radius, grid size, etc.).
    -   All text is rendered with a high-contrast shadow for excellent readability on any background.

## 🚀 Getting Started

### For Users (Pre-compiled)

1.  Go to the [**Releases**](https://github.com/hardway777/ScreenRuler/releases) page of this repository.
2.  Download the latest `.zip` file.
3.  Extract the contents and run `ScreenRuler.exe`.
4.  (Optional) To associate `.sez` files, right-click a `.sez` file, choose "Open with" > "Choose another app", find and select `ScreenRuler.exe`, and check "Always use this app".

### For Developers (Building from Source)
*Requires .NET 6.0+ and Visual Studio 2022.*

1.  Clone the repository: `git clone https://github.com/hardway777/ScreenRuler.git`
2.  Open `ScreenRuler.sln` in Visual Studio and build the project.

## 📖 How to Use

### Mouse Controls
-   **Left Click**: Place a point for the current drawing mode.
-   **Left Drag**: Pans the window and canvas together.
-   **Shift + Left Drag**: Pans only the window (viewport mode).
-   **Middle Click**:
    -   *While drawing*: Cancels the current shape.
    -   *On empty area*: Clears all drawn shapes.
-   **Mouse Wheel**: Adjusts the context-sensitive value (Snap Radius, Grid Cell Size, or Angle Type).
-   **Shift + Mouse Wheel**: Adjusts the background overlay opacity in capture mode.
-   **Right Click**: Opens the context menu.

### Keyboard Hotkeys
-   **Drawing Modes**:
    -   `1`: Lines
    -   `2`: Angles
    -   `3`: Circles
    -   `4`: Rectangles
    -   `5`: Grid
    -   `*`: Markers
    -   `R`: Recalibrate by Line
-   **Precision Modifiers**:
    -   `S`: Toggle Snap to Points.
    -   `D`: Toggle Guides.
    -   `Shift`: Hold while drawing to lock to an axis.
    -   `Ctrl`: Hold to lock the cursor's current position.
-   **Canvas & Background**:
    -   `C`: Capture the current monitor's screen as a background.
    -   `X`: Clear the captured background.
    -   `Arrow Keys`: Pan the canvas.
    -   `Home`: Reset the canvas pan.
-   **General**:
    -   `H`: Toggle the help screen.

## Changelog

### v4.0
-   **New Features:**
    -   Added interactive recalibration mode (`R`) to set scale based on a drawn line.
    -   Added session saving/loading to a compressed `.sez` file, including the captured background image.
    -   The application can now open `.sez` files passed as command-line arguments.
-   **Improvements & Fixes:**
    -   Reworked mouse drag logic for intuitive canvas vs. viewport panning.
    -   Improved grid tool with smart defaults and smoother, proportional resizing.
    -   Improved text positioning for all shapes for better readability.
    -   Fixed a UI bug where status text could overlap with the ruler scale.
	
### v3.1
-   **New Features:**
    -   Added cursor lock by holding `Ctrl` for precise point placement.

### v3.0 - The Precision CAD Update
This version transformed the ruler into a full-fledged 2D measurement and annotation tool with a pannable canvas, multiple drawing modes, and advanced precision features.

-   **New Features:**
    -   **Multiple Drawing Modes**: Added tools for measuring Angles, Circles, Rectangles, Grids, and placing Markers.
    -   **Background Capture System**: Added the ability to capture the screen and use it as a static, pannable canvas.
    -   **Precision Modifiers**: Implemented Snap-to-Point (`S`), Guides (`D`), and Axis Lock (`Shift`).
    -   **In-App Help Screen**: A new help panel (`H`) displays all hotkeys.
-   **Improvements & Fixes:**
    -   **Advanced Mouse Controls**: Reworked mouse logic to distinguish between clicks and drags.
    -   **Contextual Adjustments**: The mouse wheel now dynamically adjusts values.
    -   **Enhanced Readability**: All text now has a high-contrast shadow.
    -   **UX Refinements**: Added physical cursor snapping.
    -   Fixed a critical `OutOfMemoryException` related to angle drawing.

### v2.0 - The Annotation Update
This major update transformed the ruler from a passive measurement grid into an active annotation tool.

-   **New Features:**
    -   Implemented an interactive multi-line measurement tool.
    -   Added a dynamic preview line that follows the cursor.
    -   Introduced an adaptive UI for the central info hub.
    -   Added a toggleable "Always on Top" option.
-   **Improvements & Fixes:**
    -   Added high-contrast outlines to measurement lines and text.
    -   Color-coded the horizontal and vertical axes.
    -   Improved measurement precision.

## 🤝 Contributing

Contributions are welcome! If you have ideas for new features, find a bug, or want to improve the code, please feel free to:

-   Open an issue to discuss the change.
-   Fork the repository and submit a pull request.

## 📜 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.