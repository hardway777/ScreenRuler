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

-   **Canvas & Background Capture**: Work on a static image of your screen.
    -   **Capture (C)**: Take a screenshot of the current monitor to use as a static background. The ruler becomes fully opaque for clarity.
    -   **Pannable Canvas**: Pan the background and all drawn shapes using the **Arrow Keys** or by dragging with the mouse.
    -   **Viewport Drag (Shift + Drag)**: Move the ruler window independently over the static canvas, like a camera viewport.
    -   **Adjustable Overlay**: Use **Shift + Mouse Wheel** to change the opacity of the color overlay for perfect contrast.
    -   **Clear (X)**: Remove the background and return to live mode.

-   **Full Session Management**: Never lose your work.
    -   **Save/Load Session**: Save all your drawn shapes, window position, size, and calibration settings to a JSON file. Load it back anytime.

-   **In-App Help & UI Enhancements**:
    -   Press **H** to toggle an on-screen help panel with all hotkeys.
    -   Context-sensitive information appears next to the cursor (snap radius, grid size, etc.).
    -   All text is rendered with a high-contrast shadow for excellent readability on any background.

## 🚀 Getting Started

### For Users (Pre-compiled)

1.  Go to the [**Releases**](https://github.com/hardway777/ScreenRuler/releases) page of this repository.
2.  Download the latest `.zip` file.
3.  Extract the contents and run `ScreenRuler.exe`.

### For Developers (Building from Source)
*Requires .NET 6.0+ and Visual Studio 2022.*

1.  Clone the repository: `git clone https://github.com/hardway777/ScreenRuler.git`
2.  Open `ScreenRuler.sln` in Visual Studio and build the project.

## 📖 How to Use

### Mouse Controls
-   **Left Click**: Place a point for the current drawing mode.
-   **Left Drag**: Pan the window and canvas together (or just the window with `Shift` in capture mode).
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
-   **Precision Modifiers**:
    -   `S`: Toggle Snap to Points.
    -   `D`: Toggle Guides.
    -   `Shift`: Hold while drawing to lock to an axis.
-   **Canvas & Background**:
    -   `C`: Capture the current monitor's screen as a background.
    -   `X`: Clear the captured background.
    -   `Arrow Keys`: Pan the canvas.
    -   `Home`: Reset the canvas pan.
-   **General**:
    -   `H`: Toggle the help screen.

## Changelog

### v3.0 - The Precision CAD Update
This version transforms the ruler into a full-fledged 2D measurement and annotation tool with a pannable canvas, multiple drawing modes, and advanced precision features.

-   **New Features:**
    -   **Multiple Drawing Modes**: Added tools for measuring Angles, Circles, Rectangles, Grids, and placing Markers.
    -   **Background Capture System**: Added the ability to capture the screen and use it as a static, pannable canvas.
    -   **Precision Modifiers**: Implemented Snap-to-Point (`S`), Guides (`D`), and Axis Lock (`Shift`).
    -   **Full Session Management**: Added Save/Load functionality for all settings and drawn shapes.
    -   **In-App Help Screen**: A new help panel (`H`) displays all hotkeys.
-   **Improvements & Fixes:**
    -   **Advanced Mouse Controls**: Reworked mouse logic to distinguish between clicks and drags. Implemented `Shift`+Drag for viewport panning.
    -   **Contextual Adjustments**: The mouse wheel now dynamically adjusts Snap Radius, Grid Cell Size, or Angle Type.
    -   **Enhanced Readability**: All text now has a high-contrast shadow.
    -   **UX Refinements**: Added physical cursor snapping, improved angle text positioning, and implemented a smart default for grid size.
    -   Fixed a critical `OutOfMemoryException` related to angle drawing.
    -   Fixed numerous bugs related to UI overlap and coordinate calculations.

### v2.0 - The Annotation Update
This major update transformed the ruler from a passive measurement grid into an active annotation tool.

-   **New Features:**
    -   Implemented an interactive multi-line measurement tool.
    -   Added a dynamic preview line that follows the cursor during measurement.
    -   Introduced an adaptive UI for the central info hub.
    -   Added a toggleable "Always on Top" option in the context menu.
-   **Improvements & Fixes:**
    -   Added high-contrast outlines to all measurement lines and text.
    -   Color-coded the horizontal and vertical axes.
    -   Improved measurement precision by fixing `double`/`int` type conversions.

## 🤝 Contributing

Contributions are welcome! If you have ideas for new features, find a bug, or want to improve the code, please feel free to:

-   Open an issue to discuss the change.
-   Fork the repository and submit a pull request.

## 📜 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.