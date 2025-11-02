using Newtonsoft.Json;
using ScreenRuler.Shapes;
using ScreenRuler.State;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenRuler
{
    public partial class RulerForm : Form
    {
        #region Win32 API for Cursor Clipping
        private static class NativeMethods
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ClipCursor(ref RECT lpRect);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ClipCursor(IntPtr lpRect);

            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }
        }
        #endregion

        private readonly string? _initialFilePath;

        private Bitmap? _backgroundBitmap = null;
        private Point _backgroundBitmapOrigin = Point.Empty;
        private readonly double _defaultOpacity;
        private float _overlayOpacity = 0.4f;

        private Point _canvasOffset = Point.Empty;
        private Point _panStartCanvasOffset = Point.Empty;

        private DrawingMode _currentMode = DrawingMode.Lines;
        private List<IShape> _shapes = new List<IShape>();
        private readonly List<Point> _previewPoints = new List<Point>();
        private Point _currentMousePosition;
        private double _gridCellSize = 10.0;

        private bool _isSnapEnabled = false;
        private bool _isGuidesEnabled = false;
        private int _snapRadius = 10;
        private Point? _snappedPoint = null;
        private bool _measureOuterAngle = false;
        private bool _showHelp = false;
        private bool _isCursorLocked = false;
        private LineShape? _hoveredLine = null;

        private readonly List<Color> _lineColors = new List<Color>
        {
            Color.FromArgb(220, 231, 76, 60), Color.FromArgb(220, 52, 152, 219),
            Color.FromArgb(220, 142, 68, 173), Color.FromArgb(220, 26, 188, 156),
            Color.FromArgb(220, 243, 156, 18), Color.FromArgb(220, 230, 126, 34),
        };
        private int _lineColorIndex = 0;

        private readonly Color _horizontalColor = Color.Firebrick;
        private readonly Color _verticalColor = Color.RoyalBlue;

        private const int AbbreviationWidthThreshold = 220;
        private const int AbbreviationHeightThreshold = 120;
        private const int VerticalLayoutMinWidth = 100;


        public RulerForm(string? filePath = null)
        {
            InitializeComponent();
            _initialFilePath = filePath;
            this.KeyPreview = true;
            _defaultOpacity = this.Opacity;
            alwaysOnTopMenuItem.Checked = this.TopMost;
            this.Load += RulerForm_Load;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            NativeMethods.ClipCursor(IntPtr.Zero);
            base.OnFormClosing(e);
        }

        private void RulerForm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_initialFilePath) && File.Exists(_initialFilePath))
            {
                LoadSession(_initialFilePath);
            }
        }

        #region Painting Logic

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (_backgroundBitmap != null)
            {
                var sourceRect = new Rectangle(_canvasOffset, this.Size);
                g.DrawImage(_backgroundBitmap, this.ClientRectangle, sourceRect, GraphicsUnit.Pixel);

                int alpha = (int)(_overlayOpacity * 255);
                using (var overlay = new SolidBrush(Color.FromArgb(alpha, this.BackColor)))
                {
                    g.FillRectangle(overlay, this.ClientRectangle);
                }
            }

            using (Font font = new Font("Segoe UI", 8))
            {
                if (_showHelp)
                {
                    DrawHelpScreen(g, font);
                    return;
                }

                using (Pen hPen = new Pen(_horizontalColor, 1))
                using (Brush hBrush = new SolidBrush(_horizontalColor))
                using (Pen vPen = new Pen(_verticalColor, 1))
                using (Brush vBrush = new SolidBrush(_verticalColor))
                using (Brush infoBrush = new SolidBrush(Color.Black))
                {
                    DrawHorizontalTicks(g, hPen, hBrush, font);
                    DrawVerticalTicks(g, vPen, vBrush, font);

                    var savedState = g.Save();
                    g.TranslateTransform(-_canvasOffset.X, -_canvasOffset.Y);

                    foreach (var shape in _shapes)
                    {
                        shape.Draw(g, font);
                    }

                    DrawPreviewShape(g, font);
                    DrawSnapIndicator(g);
                    
                    g.Restore(savedState);

                    DrawGuides(g);
                    DrawCenterInfo(g, infoBrush, font);
                    DrawUIIndicators(g, infoBrush, font);
                }
            }
        }

        private void DrawHelpScreen(Graphics g, Font font)
        {
            using (var bgBrush = new SolidBrush(Color.FromArgb(220, 30, 30, 30)))
            {
                g.FillRectangle(bgBrush, this.ClientRectangle);
            }
            using (var textBrush = new SolidBrush(Color.White))
            {
                var helpLines = new List<string>
                {
                    "--- Drawing Modes ---",
                    "[1] - Lines | [2] - Angles | [3] - Circles",
                    "[4] - Rectangles | [5] - Grid | [*] - Markers",
                    "[R] - Recalibrate by Line",
                    "",
                    "--- Precision Modifiers ---",
                    "[S] - Toggle Snap | [D] - Toggle Guides",
                    "[Ctrl] - Lock Cursor Position",
                    "[Shift] - Lock Axis / Fast Pan / Viewport Drag",
                    "",
                    "--- Mouse Controls ---",
                    "Left Click - Place points",
                    "Middle Click - Cancel drawing / Clear all",
                    "Mouse Wheel - Adjust context value",
                    "Shift + Wheel - Adjust Overlay Opacity",
                    "",
                    "--- Canvas ---",
                    "[Arrow Keys] - Pan Canvas",
                    "[Home] - Reset Pan",
                    "[C] - Capture Background | [X] - Clear Background",
                    "",
                    "[H] - Close Help"
                };
                float totalHeight = helpLines.Count * font.Height;
                float startY = (this.Height - totalHeight) / 2;

                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center })
                {
                    for (int i = 0; i < helpLines.Count; i++)
                    {
                        var pos = new PointF(this.Width / 2, startY + i * font.Height);
                        DrawingHelpers.DrawStringWithShadow(g, helpLines[i], font, textBrush, pos, sf, Color.Black);
                    }
                }
            }
        }

        private void DrawPreviewShape(Graphics g, Font font)
        {
            if (_currentMode == DrawingMode.Recalibrate && _hoveredLine != null)
            {
                using (var highlightPen = new Pen(Color.Cyan, 4))
                {
                    g.DrawLine(highlightPen, _hoveredLine.P1, _hoveredLine.P2);
                }
                return;
            }

            if (_previewPoints.Count == 0) return;
            Color nextColor = _lineColors[_lineColorIndex];
            using (var pen = new Pen(nextColor, 2) { DashStyle = DashStyle.Dash })
            using (var brush = new SolidBrush(nextColor))
            {
                Point canvasMousePosition = Point.Add(_currentMousePosition, new Size(_canvasOffset));
                switch (_currentMode)
                {
                    case DrawingMode.Lines when _previewPoints.Count == 1:
                        LineShape.DrawLineWithLength(g, pen, brush, font, _previewPoints[0], canvasMousePosition);
                        break;
                    case DrawingMode.Angles when _previewPoints.Count == 1:
                        LineShape.DrawLineWithLength(g, pen, brush, font, _previewPoints[0], canvasMousePosition);
                        break;
                    case DrawingMode.Angles when _previewPoints.Count == 2:
                        AngleShape.DrawAngle(g, pen, brush, font, _previewPoints[0], _previewPoints[1], canvasMousePosition, _measureOuterAngle);
                        break;
                    case DrawingMode.Circles when _previewPoints.Count == 1:
                        double radius = Math.Sqrt(Math.Pow(canvasMousePosition.X - _previewPoints[0].X, 2) + Math.Pow(canvasMousePosition.Y - _previewPoints[0].Y, 2));
                        float r = (float)radius;
                        g.DrawEllipse(pen, _previewPoints[0].X - r, _previewPoints[0].Y - r, r * 2, r * 2);
                        break;
                    case DrawingMode.Rectangles when _previewPoints.Count == 1:
                        g.DrawRectangle(pen, RectangleShape.GetRectangle(_previewPoints[0], canvasMousePosition));
                        break;
                    case DrawingMode.Grid when _previewPoints.Count == 1:
                        var gridPreview = new GridShape { P1 = _previewPoints[0], P2 = canvasMousePosition, CellSize = _gridCellSize, Color = nextColor };
                        gridPreview.Draw(g, font);
                        break;
                }
            }
        }

        private void DrawSnapIndicator(Graphics g)
        {
            if (_snappedPoint.HasValue)
            {
                using (var pen = new Pen(Color.Magenta, 2))
                {
                    g.DrawRectangle(pen, _snappedPoint.Value.X - 5, _snappedPoint.Value.Y - 5, 10, 10);
                }
            }
        }

        private void DrawGuides(Graphics g)
        {
            if (!_isGuidesEnabled || _previewPoints.Count > 0) return;
            using (var pen = new Pen(Color.Magenta, 1) { DashStyle = DashStyle.Dot })
            {
                foreach (var point in GetAllSnapPoints())
                {
                    Point screenPoint = Point.Subtract(point, new Size(_canvasOffset));
                    if (Math.Abs(_currentMousePosition.X - screenPoint.X) < 2)
                        g.DrawLine(pen, screenPoint.X, 0, screenPoint.X, this.Height);
                    if (Math.Abs(_currentMousePosition.Y - screenPoint.Y) < 2)
                        g.DrawLine(pen, 0, screenPoint.Y, this.Width, screenPoint.Y);
                }
            }
        }
        
        private void DrawUIIndicators(Graphics g, Brush brush, Font font)
        {
            var indicators = new List<string> { $"Mode: {_currentMode}" };
            if (_isSnapEnabled) indicators.Add($"[S] Snap: {_snapRadius}px");
            if (_isGuidesEnabled) indicators.Add("[D] Guides");
            if (ModifierKeys.HasFlag(Keys.Shift) && _previewPoints.Count > 0) indicators.Add("[A] Axis Lock");
            if (_isCursorLocked) indicators.Add("[Ctrl] Cursor Locked");
            if (_backgroundBitmap != null) indicators.Add("[C] Captured");
            
            var pos = new PointF(5, this.Height - (font.Height * 2) - 10); 
            
            DrawingHelpers.DrawStringWithShadow(g, string.Join(" | ", indicators), font, brush, pos, Color.White);

            string contextText = "";
            if (_isSnapEnabled) contextText = $"Snap Radius: {_snapRadius}px";
            else if (_currentMode == DrawingMode.Grid && _previewPoints.Count == 1) contextText = $"Cell Size: {_gridCellSize:F1} {CalibrationSettings.UnitName}";
            else if (_currentMode == DrawingMode.Angles && _previewPoints.Count == 2) contextText = _measureOuterAngle ? "Outer Angle" : "Inner Angle";
            
            if (!string.IsNullOrEmpty(contextText))
            {
                var contextPos = new PointF(_currentMousePosition.X + 15, _currentMousePosition.Y + 15);
                DrawingHelpers.DrawStringWithShadow(g, contextText, font, Brushes.Black, contextPos, Color.White);
            }
        }

        private void DrawHorizontalTicks(Graphics g, Pen pen, Brush brush, Font font)
        {
            int tickHeight = 15;
            for (int i = 0; i < this.Width; i++)
            {
                if (i % 10 != 0 || i == 0) continue;
                int currentTickHeight = (i % 100 == 0) ? tickHeight : (i % 50 == 0) ? tickHeight - 5 : tickHeight - 10;
                g.DrawLine(pen, i, 0, i, currentTickHeight);
                g.DrawLine(pen, i, this.Height, i, this.Height - currentTickHeight);
                if (i % 100 == 0)
                {
                    string text = CalibrationSettings.ToUnits(i).ToString("F2");
                    DrawingHelpers.DrawStringWithShadow(g, text, font, brush, new PointF(i + 2, 5), Color.White);
                    DrawingHelpers.DrawStringWithShadow(g, text, font, brush, new PointF(i + 2, this.Height - font.Height - 5), Color.White);
                }
            }
        }

        private void DrawVerticalTicks(Graphics g, Pen pen, Brush brush, Font font)
        {
            int tickHeight = 15;
            for (int j = 0; j < this.Height; j++)
            {
                if (j % 10 != 0 || j == 0) continue;
                int currentTickHeight = (j % 100 == 0) ? tickHeight : (j % 50 == 0) ? tickHeight - 5 : tickHeight - 10;
                g.DrawLine(pen, 0, j, currentTickHeight, j);
                g.DrawLine(pen, this.Width, j, this.Width - currentTickHeight, j);
                if (j % 100 == 0)
                {
                    string text = CalibrationSettings.ToUnits(j).ToString("F2");
                    float textY = j - font.Height / 2;
                    DrawingHelpers.DrawStringWithShadow(g, text, font, brush, new PointF(5, textY), Color.White);
                    SizeF textSize = g.MeasureString(text, font);
                    DrawingHelpers.DrawStringWithShadow(g, text, font, brush, new PointF(this.Width - textSize.Width - 5, textY), Color.White);
                }
            }
        }

        private void DrawCenterInfo(Graphics g, Brush brush, Font font)
        {
            double diagonalInPixels = Math.Sqrt(Math.Pow(this.Width, 2) + Math.Pow(this.Height, 2));
            double angleInRadians = Math.Atan2(this.Height, this.Width);
            double angleInDegrees = angleInRadians * (180.0 / Math.PI);
            string unitName = CalibrationSettings.UnitName;
            double widthInUnits = CalibrationSettings.ToUnits(this.Width);
            double heightInUnits = CalibrationSettings.ToUnits(this.Height);
            double diagonalInUnits = CalibrationSettings.ToUnits(diagonalInPixels);
            bool useAbbreviations = this.Width < AbbreviationWidthThreshold || this.Height < AbbreviationHeightThreshold;
            string widthLabel = useAbbreviations ? "W:" : "Width:";
            string heightLabel = useAbbreviations ? "H:" : "Height:";
            string diagonalLabel = useAbbreviations ? "D:" : "Diagonal:";
            string angleLabel = useAbbreviations ? "∠:" : "Angle:";
            bool useSingleLineLayout = this.Height < AbbreviationHeightThreshold;
            if (useSingleLineLayout)
            {
                string line = $"{widthLabel} {widthInUnits:F2} | {heightLabel} {heightInUnits:F2} | {diagonalLabel} {diagonalInUnits:F2} | {angleLabel} {angleInDegrees:F2}°";
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    DrawingHelpers.DrawStringWithShadow(g, line, font, brush, new RectangleF(0, 0, this.Width, this.Height), sf, Color.White);
                }
                return;
            }
            var infoLines = new List<string>
            {
                $"{widthLabel} {widthInUnits:F2} {unitName}",
                $"{heightLabel} {heightInUnits:F2} {unitName}",
                $"{diagonalLabel} {diagonalInUnits:F2} {unitName}",
                $"{angleLabel} {angleInDegrees:F2}°"
            };
            bool useVerticalLayout = this.Height > this.Width && this.Width > VerticalLayoutMinWidth;
            float totalTextHeight = infoLines.Count * font.Height;
            float startY = (this.Height - totalTextHeight) / 2f;
            if (useVerticalLayout)
            {
                float startX = 15f;
                for (int i = 0; i < infoLines.Count; i++)
                {
                    DrawingHelpers.DrawStringWithShadow(g, infoLines[i], font, brush, new PointF(startX, startY + (i * font.Height)), Color.White);
                }
            }
            else
            {
                float centerX = this.Width / 2f;
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center })
                {
                    for (int i = 0; i < infoLines.Count; i++)
                    {
                        DrawingHelpers.DrawStringWithShadow(g, infoLines[i], font, brush, new PointF(centerX, startY + (i * font.Height)), sf, Color.White);
                    }
                }
            }
        }
        
        private void RulerForm_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        #endregion

        #region Input Handling

        private bool _isPotentialClick = false;

        private void ResetDrawingState()
        {
            _previewPoints.Clear();
            _measureOuterAngle = false;
            if (this.Width > 0)
            {
                _gridCellSize = CalibrationSettings.ToUnits(this.Width / 10);
            }
            this.Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.ControlKey && !_isCursorLocked)
            {
                _isCursorLocked = true;
                Point lockPos = Cursor.Position;
                var clipRect = new NativeMethods.RECT { Left = lockPos.X, Top = lockPos.Y, Right = lockPos.X + 1, Bottom = lockPos.Y + 1 };
                NativeMethods.ClipCursor(ref clipRect);
                this.Invalidate();
                return;
            }
            
            int panAmount = e.Shift ? 10 : 1;
            bool canvasChanged = false;
            switch (e.KeyCode)
            {
                case Keys.Left: _canvasOffset.X -= panAmount; canvasChanged = true; break;
                case Keys.Right: _canvasOffset.X += panAmount; canvasChanged = true; break;
                case Keys.Up: _canvasOffset.Y -= panAmount; canvasChanged = true; break;
                case Keys.Down: _canvasOffset.Y += panAmount; canvasChanged = true; break;
                case Keys.Home: _canvasOffset = Point.Empty; canvasChanged = true; break;
            }
            if (canvasChanged)
            {
                this.Invalidate();
                return;
            }

            DrawingMode newMode = _currentMode;
            switch (e.KeyCode)
            {
                case Keys.D1: newMode = DrawingMode.Lines; break;
                case Keys.D2: newMode = DrawingMode.Angles; break;
                case Keys.D3: newMode = DrawingMode.Circles; break;
                case Keys.D4: newMode = DrawingMode.Rectangles; break;
                case Keys.D5: newMode = DrawingMode.Grid; break;
                case Keys.Multiply: newMode = DrawingMode.Markers; break;
                case Keys.S: _isSnapEnabled = !_isSnapEnabled; break;
                case Keys.D: _isGuidesEnabled = !_isGuidesEnabled; break;
                case Keys.H: _showHelp = !_showHelp; break;
                case Keys.C: CaptureScreen(); break;
                case Keys.X: ClearBackground(); break;
                case Keys.R: newMode = DrawingMode.Recalibrate; break;
            }
            if (newMode != _currentMode)
            {
                _currentMode = newMode;
                ResetDrawingState();
            }
            this.Invalidate();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.ControlKey && _isCursorLocked)
            {
                _isCursorLocked = false;
                NativeMethods.ClipCursor(IntPtr.Zero);
                this.Invalidate();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            int change = e.Delta > 0 ? 1 : -1;

            if (ModifierKeys.HasFlag(Keys.Shift) && _backgroundBitmap != null)
            {
                float step = 0.05f;
                _overlayOpacity += change * step;
                _overlayOpacity = Math.Clamp(_overlayOpacity, 0.0f, 0.9f);
            }
            else if (_isSnapEnabled)
            {
                _snapRadius = Math.Max(2, _snapRadius + change);
            }
            else if (_currentMode == DrawingMode.Grid && _previewPoints.Count == 1)
            {
                double adjustmentFactor = ModifierKeys.HasFlag(Keys.Shift) ? 0.01 : 0.10;
                _gridCellSize *= (1.0 + adjustmentFactor * change);
                _gridCellSize = Math.Max(0.1, _gridCellSize);
            }
            else if (_currentMode == DrawingMode.Angles && _previewPoints.Count == 2)
            {
                _measureOuterAngle = !_measureOuterAngle;
            }
            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Middle)
            {
                if (_previewPoints.Count > 0) ResetDrawingState();
                else
                {
                    _shapes.Clear();
                    _lineColorIndex = 0;
                    this.Invalidate();
                }
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                _isPotentialClick = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
                _panStartCanvasOffset = _canvasOffset;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point processedMousePosition = e.Location;
            _snappedPoint = null;
            _hoveredLine = null;

            if (_currentMode == DrawingMode.Recalibrate)
            {
                _hoveredLine = FindClosestLine(Point.Add(e.Location, new Size(_canvasOffset)));
                this.Invalidate();
                return;
            }

            if (e.Button == MouseButtons.Left && _isPotentialClick)
            {
                Point startClient = this.PointToClient(dragCursorPoint);
                if (Math.Abs(e.X - startClient.X) > SystemInformation.DragSize.Width ||
                    Math.Abs(e.Y - startClient.Y) > SystemInformation.DragSize.Height)
                {
                    _isPotentialClick = false;
                    isDragging = true;
                }
            }

            if (isDragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                
                if (_backgroundBitmap != null && ModifierKeys.HasFlag(Keys.Shift))
                {
                    this.Location = Point.Add(dragFormPoint, new Size(diff));
                    _canvasOffset = Point.Add(_panStartCanvasOffset, new Size(diff));
                }
                else
                {
                    this.Location = Point.Add(dragFormPoint, new Size(diff));
                }
                
                this.Invalidate();
                return;
            }
            
            if (_previewPoints.Count > 0)
            {
                if (ModifierKeys.HasFlag(Keys.Shift))
                {
                    Point startPoint = Point.Subtract(_previewPoints.Last(), new Size(_canvasOffset));
                    int dx = Math.Abs(e.Location.X - startPoint.X);
                    int dy = Math.Abs(e.Location.Y - startPoint.Y);
                    if (dx > dy) processedMousePosition.Y = startPoint.Y;
                    else processedMousePosition.X = startPoint.X;
                }
                
                if (_isSnapEnabled)
                {
                    Point canvasMousePos = Point.Add(processedMousePosition, new Size(_canvasOffset));
                    Point? closestPoint = FindClosestSnapPoint(canvasMousePos);
                    if (closestPoint.HasValue)
                    {
                        processedMousePosition = Point.Subtract(closestPoint.Value, new Size(_canvasOffset));
                        _snappedPoint = closestPoint;

                        Point screenPoint = this.PointToScreen(processedMousePosition);
                        if (Cursor.Position != screenPoint)
                        {
                            Cursor.Position = screenPoint;
                        }
                    }
                }
            }
            else if (_isGuidesEnabled)
            {
                foreach (var point in GetAllSnapPoints())
                {
                    Point screenPoint = Point.Subtract(point, new Size(_canvasOffset));
                    if (Math.Abs(processedMousePosition.X - screenPoint.X) < 2) processedMousePosition.X = screenPoint.X;
                    if (Math.Abs(processedMousePosition.Y - screenPoint.Y) < 2) processedMousePosition.Y = screenPoint.Y;
                }
            }
            
            _currentMousePosition = processedMousePosition;
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                if (_isPotentialClick)
                {
                    HandleMouseClickLogic(e);
                }
                isDragging = false;
                _isPotentialClick = false;
            }
        }
        
        private void HandleMouseClickLogic(MouseEventArgs e)
        {
            if (_currentMode == DrawingMode.Recalibrate)
            {
                if (_hoveredLine != null)
                {
                    double lengthInPixels = Math.Sqrt(Math.Pow(_hoveredLine.P2.X - _hoveredLine.P1.X, 2) + Math.Pow(_hoveredLine.P2.Y - _hoveredLine.P1.Y, 2));
                    using (var calibForm = new CalibrationForm(this.Width, lengthInPixels))
                    {
                        if (calibForm.ShowDialog() == DialogResult.OK)
                        {
                            this.Invalidate();
                        }
                    }
                    _currentMode = DrawingMode.Lines;
                    _hoveredLine = null;
                    this.Invalidate();
                }
                return;
            }

            Point canvasMousePosition = Point.Add(_currentMousePosition, new Size(_canvasOffset));
            _previewPoints.Add(canvasMousePosition);

            switch (_currentMode)
            {
                case DrawingMode.Lines: HandleShapeCompletion(2, () => new LineShape { P1 = _previewPoints[0], P2 = _previewPoints[1], Color = GetNextColor() }); break;
                case DrawingMode.Angles: HandleShapeCompletion(3, () => new AngleShape { P1 = _previewPoints[0], Vertex = _previewPoints[1], P3 = _previewPoints[2], Color = GetNextColor(), MeasureOuterAngle = _measureOuterAngle }); break;
                case DrawingMode.Circles: HandleShapeCompletion(2, () => {
                    double radius = Math.Sqrt(Math.Pow(_previewPoints[1].X - _previewPoints[0].X, 2) + Math.Pow(_previewPoints[1].Y - _previewPoints[0].Y, 2));
                    return new CircleShape { Center = _previewPoints[0], Radius = radius, Color = GetNextColor() };
                }); break;
                case DrawingMode.Rectangles: HandleShapeCompletion(2, () => new RectangleShape { P1 = _previewPoints[0], P2 = _previewPoints[1], Color = GetNextColor() }); break;
                case DrawingMode.Grid: HandleShapeCompletion(2, () => new GridShape { P1 = _previewPoints[0], P2 = _previewPoints[1], CellSize = _gridCellSize, Color = GetNextColor() }); break;
                case DrawingMode.Markers:
                    _shapes.Add(new MarkerShape { Position = canvasMousePosition, Color = GetNextColor() });
                    _previewPoints.Clear();
                    _lineColorIndex = (_lineColorIndex + 1) % _lineColors.Count;
                    break;
            }
            this.Invalidate();
        }

        private void HandleShapeCompletion(int requiredClicks, Func<IShape> shapeFactory)
        {
            if (_previewPoints.Count >= requiredClicks)
            {
                _shapes.Add(shapeFactory());
                ResetDrawingState();
                _lineColorIndex = (_lineColorIndex + 1) % _lineColors.Count;
            }
        }

        private Color GetNextColor() => _lineColors[_lineColorIndex];

        private IEnumerable<Point> GetAllSnapPoints() => _shapes.SelectMany(s => s.GetSnapPoints());

        private Point? FindClosestSnapPoint(Point mousePosition)
        {
            if (!GetAllSnapPoints().Any()) return null;
            var closest = GetAllSnapPoints().OrderBy(p => Math.Pow(mousePosition.X - p.X, 2) + Math.Pow(mousePosition.Y - p.Y, 2)).First();
            double distance = Math.Sqrt(Math.Pow(mousePosition.X - closest.X, 2) + Math.Pow(mousePosition.Y - closest.Y, 2));
            return distance < _snapRadius ? (Point?)closest : null;
        }

        private LineShape? FindClosestLine(Point mousePosition)
        {
            LineShape? closestLine = null;
            double minDistance = double.MaxValue;

            foreach (var line in _shapes.OfType<LineShape>())
            {
                double dx = line.P2.X - line.P1.X;
                double dy = line.P2.Y - line.P1.Y;
                if (dx == 0 && dy == 0) continue;

                double t = ((mousePosition.X - line.P1.X) * dx + (mousePosition.Y - line.P1.Y) * dy) / (dx * dx + dy * dy);
                t = Math.Max(0, Math.Min(1, t));

                double closestX = line.P1.X + t * dx;
                double closestY = line.P1.Y + t * dy;
                
                double distance = Math.Sqrt(Math.Pow(mousePosition.X - closestX, 2) + Math.Pow(mousePosition.Y - closestY, 2));

                if (distance < 10 && distance < minDistance)
                {
                    minDistance = distance;
                    closestLine = line;
                }
            }
            return closestLine;
        }

        #endregion

        #region Background Capture

        private void CaptureScreen()
        {
            this.Visible = false;
            System.Threading.Thread.Sleep(200);

            _backgroundBitmap?.Dispose();
            
            var currentScreen = Screen.FromControl(this);
            Rectangle bounds = currentScreen.Bounds;
            _backgroundBitmapOrigin = bounds.Location;
            _backgroundBitmap = new Bitmap(bounds.Width, bounds.Height);

            using (Graphics g = Graphics.FromImage(_backgroundBitmap))
            {
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            }
            
            _canvasOffset = Point.Subtract(this.Location, new Size(_backgroundBitmapOrigin));

            this.Visible = true;
            this.Opacity = 1.0;
            this.Invalidate();
        }

        private void ClearBackground()
        {
            _backgroundBitmap?.Dispose();
            _backgroundBitmap = null;
            _backgroundBitmapOrigin = Point.Empty;
            this.Opacity = _defaultOpacity;
            this.Invalidate();
        }

        #endregion

        #region Session Save/Load
        private void saveSessionMenuItem_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Ruler Session (*.sez)|*.sez";
                sfd.Title = "Save Ruler Session";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var fileStream = new FileStream(sfd.FileName, FileMode.Create))
                        using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                        {
                            var state = new RulerState
                            {
                                Location = this.Location,
                                Size = this.Size,
                                CanvasOffset = _canvasOffset,
                                CalibrationPixels = CalibrationSettings.Pixels,
                                CalibrationUnitsValue = CalibrationSettings.UnitsValue,
                                CalibrationUnitName = CalibrationSettings.UnitName,
                                Shapes = _shapes
                            };
                            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                            string json = JsonConvert.SerializeObject(state, Formatting.Indented, settings);

                            var jsonEntry = archive.CreateEntry("session.json");
                            using (var writer = new StreamWriter(jsonEntry.Open()))
                            {
                                writer.Write(json);
                            }

                            if (_backgroundBitmap != null)
                            {
                                var imageEntry = archive.CreateEntry("background.png");
                                using (var entryStream = imageEntry.Open())
                                {
                                    _backgroundBitmap.Save(entryStream, ImageFormat.Png);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to save session: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void loadSessionMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Ruler Session (*.sez)|*.sez";
                ofd.Title = "Load Ruler Session";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LoadSession(ofd.FileName);
                }
            }
        }

        private void LoadSession(string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Read))
                {
                    var jsonEntry = archive.GetEntry("session.json");
                    if (jsonEntry == null) throw new FileNotFoundException("session.json not found in archive.");

                    string json;
                    using (var reader = new StreamReader(jsonEntry.Open()))
                    {
                        json = reader.ReadToEnd();
                    }

                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    var state = JsonConvert.DeserializeObject<RulerState>(json, settings);

                    if (state != null)
                    {
                        this.Location = state.Location;
                        this.Size = state.Size;
                        _canvasOffset = state.CanvasOffset;
                        CalibrationSettings.Pixels = state.CalibrationPixels;
                        CalibrationSettings.UnitsValue = state.CalibrationUnitsValue;
                        CalibrationSettings.UnitName = state.CalibrationUnitName;
                        _shapes = state.Shapes ?? new List<IShape>();

                        var imageEntry = archive.GetEntry("background.png");
                        if (imageEntry != null)
                        {
                            _backgroundBitmap?.Dispose();
                            using (var entryStream = imageEntry.Open())
                            {
                                _backgroundBitmap = new Bitmap(entryStream);
                            }
                            this.Opacity = 1.0;
                        }
                        else
                        {
                            ClearBackground();
                        }
                        ResetDrawingState();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load session file: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Window Dragging and Resizing
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;
        private int resizeHandleSize = 10;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)
            {
                Point screenPoint = new Point(m.LParam.ToInt32());
                Point clientPoint = this.PointToClient(screenPoint);
                if (clientPoint.X <= resizeHandleSize && clientPoint.Y <= resizeHandleSize) m.Result = (IntPtr)HTTOPLEFT;
                else if (clientPoint.X >= ClientSize.Width - resizeHandleSize && clientPoint.Y <= resizeHandleSize) m.Result = (IntPtr)HTTOPRIGHT;
                else if (clientPoint.X <= resizeHandleSize && clientPoint.Y >= ClientSize.Height - resizeHandleSize) m.Result = (IntPtr)HTBOTTOMLEFT;
                else if (clientPoint.X >= ClientSize.Width - resizeHandleSize && clientPoint.Y >= ClientSize.Height - resizeHandleSize) m.Result = (IntPtr)HTBOTTOMRIGHT;
                else if (clientPoint.X <= resizeHandleSize) m.Result = (IntPtr)HTLEFT;
                else if (clientPoint.X >= ClientSize.Width - resizeHandleSize) m.Result = (IntPtr)HTRIGHT;
                else if (clientPoint.Y <= resizeHandleSize) m.Result = (IntPtr)HTTOP;
                else if (clientPoint.Y >= ClientSize.Height - resizeHandleSize) m.Result = (IntPtr)HTBOTTOM;
            }
        }

        private bool isDragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        #endregion

        #region Context Menu
        private void alwaysOnTopMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = alwaysOnTopMenuItem.Checked;
        }

        private void Calibration_Click(object sender, EventArgs e)
        {
            using (CalibrationForm calibForm = new CalibrationForm(this.Width))
            {
                if (calibForm.ShowDialog() == DialogResult.OK)
                {
                    this.Invalidate();
                }
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void helpMenuItem_Click(object sender, EventArgs e)
        {
            _showHelp = !_showHelp;
            this.Invalidate();
        }

        private void captureBgMenuItem_Click(object sender, EventArgs e)
        {
            CaptureScreen();
        }

        private void clearBgMenuItem_Click(object sender, EventArgs e)
        {
            ClearBackground();
        }
        #endregion
    }
}