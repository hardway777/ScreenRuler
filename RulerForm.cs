using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ScreenRuler
{
    public partial class RulerForm : Form
    {
        private struct MeasurementLine
        {
            public Point P1;
            public Point P2;
            public Color LineColor;
        }

        private readonly List<MeasurementLine> _completedLines = new List<MeasurementLine>();
        private Point? _startPoint = null;
        private Point _currentMousePosition;

        private readonly List<Color> _lineColors = new List<Color>
        {
            Color.FromArgb(220, 231, 76, 60),
            Color.FromArgb(220, 52, 152, 219),
            Color.FromArgb(220, 142, 68, 173),
            Color.FromArgb(220, 26, 188, 156),
            Color.FromArgb(220, 243, 156, 18),
            Color.FromArgb(220, 230, 126, 34),
        };
        private int _lineColorIndex = 0;

        private readonly Color _horizontalColor = Color.Firebrick;
        private readonly Color _verticalColor = Color.RoyalBlue;

        private const int AbbreviationWidthThreshold = 220;
        private const int AbbreviationHeightThreshold = 120;
        private const int VerticalLayoutMinWidth = 100;

        public RulerForm()
        {
            InitializeComponent();
            alwaysOnTopMenuItem.Checked = this.TopMost;
        }

        #region Painting Logic

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (Font font = new Font("Segoe UI", 8))
            using (Pen hPen = new Pen(_horizontalColor, 1))
            using (Brush hBrush = new SolidBrush(_horizontalColor))
            using (Pen vPen = new Pen(_verticalColor, 1))
            using (Brush vBrush = new SolidBrush(_verticalColor))
            using (Brush infoBrush = new SolidBrush(Color.Black))
            {
                DrawHorizontalTicks(g, hPen, hBrush, font);
                DrawVerticalTicks(g, vPen, vBrush, font);
                DrawCenterInfo(g, infoBrush, font);
                DrawMeasurements(g, font);
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
                    g.DrawString(text, font, brush, i + 2, 5);
                    g.DrawString(text, font, brush, i + 2, this.Height - font.Height - 5);
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
                    g.DrawString(text, font, brush, 5, textY);
                    SizeF textSize = g.MeasureString(text, font);
                    g.DrawString(text, font, brush, this.Width - textSize.Width - 5, textY);
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
                    g.DrawString(line, font, brush, new RectangleF(0, 0, this.Width, this.Height), sf);
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
                    g.DrawString(infoLines[i], font, brush, startX, startY + (i * font.Height));
                }
            }
            else
            {
                float centerX = this.Width / 2f;
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center })
                {
                    for (int i = 0; i < infoLines.Count; i++)
                    {
                        g.DrawString(infoLines[i], font, brush, centerX, startY + (i * font.Height), sf);
                    }
                }
            }
        }

        private void DrawMeasurements(Graphics g, Font font)
        {
            foreach (var line in _completedLines)
            {
                using (var pen = new Pen(line.LineColor, 2))
                using (var brush = new SolidBrush(line.LineColor))
                {
                    DrawLineWithLength(g, pen, brush, font, line.P1, line.P2);
                }
            }

            if (_startPoint.HasValue)
            {
                Color nextColor = _lineColors[_lineColorIndex];
                using (var pen = new Pen(nextColor, 2) { DashStyle = DashStyle.Dash })
                using (var brush = new SolidBrush(nextColor))
                {
                    DrawLineWithLength(g, pen, brush, font, _startPoint.Value, _currentMousePosition);
                }
            }
        }

        private void DrawLineWithLength(Graphics g, Pen pen, Brush brush, Font font, Point p1, Point p2)
        {
            using (Pen outlinePen = new Pen(Color.FromArgb(150, Color.Black), pen.Width + 1))
            {
                g.DrawLine(outlinePen, p1, p2);
            }

            g.DrawLine(pen, p1, p2);

            g.FillEllipse(brush, p1.X - 3, p1.Y - 3, 6, 6);
            g.FillEllipse(brush, p2.X - 3, p2.Y - 3, 6, 6);

            double distanceInPixels = Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
            double distanceInUnits = CalibrationSettings.ToUnits(distanceInPixels);
            string text = $"{distanceInUnits:F2} {CalibrationSettings.UnitName}";

            Point midpoint = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
            Point textPosition = new Point(midpoint.X + 5, midpoint.Y + 5);

            using (Brush outlineBrush = new SolidBrush(Color.FromArgb(180, Color.White)))
            {
                g.DrawString(text, font, outlineBrush, textPosition.X - 1, textPosition.Y);
                g.DrawString(text, font, outlineBrush, textPosition.X + 1, textPosition.Y);
                g.DrawString(text, font, outlineBrush, textPosition.X, textPosition.Y - 1);
                g.DrawString(text, font, outlineBrush, textPosition.X, textPosition.Y + 1);
            }
            
            g.DrawString(text, font, brush, textPosition);
        }

        private void RulerForm_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
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

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Middle)
            {
                if (_startPoint.HasValue)
                {
                    _startPoint = null;
                    this.Invalidate();
                }
                else
                {
                    _completedLines.Clear();
                    _lineColorIndex = 0;
                    this.Invalidate();
                }
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (isDragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(diff));
            }

            if (_startPoint.HasValue)
            {
                _currentMousePosition = e.Location;
                this.Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }
        #endregion

        #region Context Menu and Click Logic

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button != MouseButtons.Left) return;

            if (!_startPoint.HasValue)
            {
                _startPoint = e.Location;
                _currentMousePosition = e.Location;
            }
            else
            {
                var newLine = new MeasurementLine
                {
                    P1 = _startPoint.Value,
                    P2 = e.Location,
                    LineColor = _lineColors[_lineColorIndex]
                };
                _completedLines.Add(newLine);

                _startPoint = null;
                _lineColorIndex = (_lineColorIndex + 1) % _lineColors.Count;
            }
            this.Invalidate();
        }

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

        #endregion
    }
}