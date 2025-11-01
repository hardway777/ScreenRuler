# Advanced Screen Ruler

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A versatile, on-screen measurement tool for Windows, built with C# and Windows Forms. Measure pixels, calibrate to real-world units, and get instant diagonal and angle calculations right on your screen. Perfect for designers, engineers, cartographers, and anyone who needs to measure objects within digital images, maps, or blueprints.

![Advanced Screen Ruler in action](images/screenshot.png)


## ✨ Features

-   **On-Screen Measurement**: A semi-transparent, frameless window that you can drag and resize over any application.
-   **Custom Calibration**: Define your own scale (e.g., `150 pixels = 4.5 meters`) to measure anything in your preferred units. The tool does the conversion for you.
-   **Multi-Axis Ticks**: Displays horizontal and vertical tick marks on all four sides for precise alignment.
-   **Real-Time Info Hub**: A central display shows live calculations for:
    -   **Width & Height**: The dimensions of the ruler in your calibrated units.
    -   **Diagonal Length**: The diagonal measurement, calculated instantly.
    -   **Angle**: The angle of the diagonal in degrees, perfect for checking slopes or gradients.
-   **Resizable & Draggable**: Easily move the ruler and resize it from any corner or edge.
-   **Always on Top**: Stays on top of other windows so you can measure without interruption.

## 🚀 Getting Started

You can either run the pre-compiled executable or build the project from the source code.

### For Users (Pre-compiled)

1.  Go to the [**Releases**]([[https://github.com/hardway777/ScreenRuler/releases](https://github.com/hardway777/ScreenRuler/releases)) page of this repository.
2.  Download the latest `.zip` file.
3.  Extract the contents and run `ScreenRuler.exe`.

### For Developers (Building from Source)

#### Prerequisites

-   [.NET 6.0 SDK (or newer)](https://dotnet.microsoft.com/download)
-   [Visual Studio 2022](https://visualstudio.microsoft.com/) with the ".NET desktop development" workload installed.

#### Build Steps

1.  Clone the repository:
    ```sh
    git clone https://github.com/hardway777/ScreenRuler.git
    ```
2.  Navigate to the project directory:
    ```sh
    cd ScreenRuler
    ```
3.  Open the `ScreenRuler.sln` file in Visual Studio.
4.  Build the solution by pressing `Ctrl+Shift+B` or run it directly by pressing `F5`.

## 📖 How to Use

1.  **Launch** the application. The ruler will appear on your screen.
2.  **Drag** the ruler by clicking and holding the left mouse button anywhere inside its body.
3.  **Resize** the ruler by dragging its edges or corners.
4.  **Right-click** anywhere on the ruler to open the context menu.
5.  Select **"Calibration..."** to open the settings dialog:
    -   Enter a number of **pixels**.
    -   Enter the corresponding **value** in your desired units.
    -   Enter the **unit name** (e.g., "meters", "ft", "cm").
    -   Click **OK**.
6.  The ruler will instantly update its tick marks and the central information hub will display all measurements based on your new scale.
7.  To close the application, right-click and select **"Close"**.

## 🤝 Contributing

Contributions are welcome! If you have ideas for new features, find a bug, or want to improve the code, please feel free to:

-   Open an issue to discuss the change.
-   Fork the repository and submit a pull request.

## 📜 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
