# Advanced Screen Ruler

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A versatile, on-screen measurement tool for Windows, built with C# and Windows Forms. What started as a simple ruler has evolved into a powerful 2D measurement and annotation tool, perfect for designers, engineers, and game developers.

It allows you to measure pixels, calibrate to real-world units, and analyze geometry on a static, pannable canvas captured from your screen, now with support for perspective correction.

![Advanced Screen Ruler v5.0 in action](https://github.com/hardway777/ScreenRuler/blob/main/images/screenshotv5.png)

## ✨ Features

-   **Perspective Measurement**: Define a 4-point perspective plane on an image and measure distances within it as if you were on a flat surface. The tool uses a robust geometric method to provide accurate, perspective-corrected measurements.

-   **Comprehensive Drawing Toolkit**: Switch between multiple tools to measure anything you need.
    -   **Lines**: Measure distance between two points, with or without perspective correction.
    -   **Angles**: Three-point angle measurement with inner/outer angle toggle.
    -   **Circles**: Define by center and radius.
    -   **Rectangles**: Define by diagonal corners, with length labels for each side.
    -   **Grids**: Create rectangular grids with adjustable cell size (in calibrated units).
    -   **Markers**: Place markers to highlight points of interest and display their coordinates.

-   **Advanced Precision Tools**: Draw with CAD-like accuracy.
    -   **Snap to Points (S)**: Automatically snap the cursor to the nearest point of any existing shape. Snap radius is adjustable.
    -   **Guides (D)**: Display alignment guides when the cursor is horizontally or vertically aligned with other snap points.
    -   **Axis Lock (Shift)**: Restrict drawing to horizontal or vertical axes.
    -   **Cursor Lock (Ctrl)**: Hold to lock the cursor's position for precise clicks.

-   **Canvas & Background Capture**: Work on a static image of your screen.
    -   **Capture (C)**: Take a screenshot of the current monitor to use as a static background.
    -   **Pannable Canvas**: Pan the background and all drawn shapes using the **Arrow Keys**.
    -   **Two Drag Modes**:
        -   **Default Drag**: Moves the window and the canvas together.
        -   **Viewport Drag (Shift + Drag)**: Moves only the window over the static canvas.
    -   **Adjustable Overlay**: Use **Shift + Mouse Wheel** to change the opacity of the color overlay.

-   **Interactive Recalibration (R)**: Enter Recalibration mode, click on any drawn line, and enter its real-world length to instantly recalibrate the entire canvas.

-   **Full Session Management**: Never lose your work.
    -   **Save/Load Session**: Save all your drawn shapes, window state, canvas pan, and calibration to a single `.sez` file, including the captured background.
    -   **File Association**: Opens `.sez` files passed as a command-line argument.

-   **In-App Help & UI Enhancements**:
    -   Press **H** to toggle an on-screen help panel with all hotkeys.
    -   Context-sensitive information appears next to the cursor.
    -   All shape labels are drawn in a rounded box for maximum clarity.

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
-   **Shift + Mouse Wheel**: Adjusts the background overlay opacity.
-   **Right Click**: Opens the context menu.

### Keyboard Hotkeys
-   **Drawing Modes**:
    -   `1`: Lines
    -   `2`: Angles
    -   `3`: Circles
    -   `4`: Rectangles
    -   `5`: Grid
    -   `6`: Perspective Plane
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

### v5.1
-   **Improvements & Fixes:**
    -   Fixed inverted drag logic for the captured canvas; default drag now moves the canvas with the window, and `Shift+Drag` moves the window over a static canvas (viewport mode).
    -   Corrected a critical bug where repeated screen captures would progressively offset existing shapes.
    -   Resolved incorrect drawing preview when creating a perspective plane, providing a more intuitive workflow.
    -   Addressed a mathematical degeneracy that caused perspective calculations to fail on vertical or horizontal base lines.
    -   Shape label borders now correctly use the shape's color for better visual cohesion.
    -   Fixed a bug that prevented mouse clicks from registering for drawing points.

### v5.0 - The Perspective Update
-   **New Features:**
    -   **Perspective Measurement**: Added a `Perspective` drawing mode (`6`) to define a 4-point plane.
    -   Lines drawn inside this plane are automatically measured with perspective correction.
    -   The creation workflow guides the user through setting the true width and height of the plane.
-   **Improvements & Fixes:**
    -   **New Label Style**: All shape labels are now drawn inside a rounded white box with a colored border for maximum clarity.
    -   Drawing is now constrained within the boundaries of a perspective plane if started inside one.

### v4.1
-   **Improvements & Fixes:**
    -   Fixed label drawing logic for all shapes.
	
### v4.0
-   **New Features:**
    -   Added interactive recalibration mode (`R`).
    -   Added session saving/loading to a compressed `.sez` file, including the background image.
    -   Added command-line argument support for opening `.sez` files.
-   **Improvements & Fixes:**
    -   Reworked mouse drag logic for intuitive canvas vs. viewport panning.
    -   Improved grid tool with smart defaults and proportional resizing.
    -   Fixed a UI bug where status text could overlap the ruler scale.

## 🤝 Contributing

Contributions are welcome! If you have ideas for new features, find a bug, or want to improve the code, please feel free to:

-   Open an issue to discuss the change.
-   Fork the repository and submit a pull request.

## 📜 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.