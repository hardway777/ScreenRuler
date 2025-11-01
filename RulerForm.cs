using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ScreenRuler
{
    public partial class RulerForm : Form
    {
        public RulerForm()
        {
            InitializeComponent();
        }

        #region Draw

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            using (Pen pen = new Pen(Color.Black, 1))
            using (Brush brush = new SolidBrush(Color.Black))
            using (Font font = new Font("Segoe UI", 9))
            {
                DrawHorizontalTicks(g, pen, brush, font);
                DrawVerticalTicks(g, pen, brush, font);
                DrawCenterInfo(g, brush, font);
            }
        }
        
        private void DrawHorizontalTicks(Graphics g, Pen pen, Brush brush, Font font)
        {
            int tickHeight = 15;

            for (int i = 0; i < this.Width; i++)
            {
                if (i % 10 != 0) continue;

                int currentTickHeight = 0;
                bool isMajorTick = false;

                if (i % 100 == 0) { currentTickHeight = tickHeight; isMajorTick = true; }
                else if (i % 50 == 0) { currentTickHeight = tickHeight - 5; }
                else { currentTickHeight = tickHeight - 10; }

                if (i == 0) continue;

                // Риски сверху
                g.DrawLine(pen, i, 0, i, currentTickHeight);
                // Риски снизу
                g.DrawLine(pen, i, this.Height, i, this.Height - currentTickHeight);

                if (isMajorTick)
                {
                    string text = CalibrationSettings.ToUnits(i).ToString("F2");
                    g.DrawString(text, font, brush, i + 2, 10);
                    g.DrawString(text, font, brush, i + 2, this.Height - font.Height - 10);
                }
            }
        }
        
        private void DrawVerticalTicks(Graphics g, Pen pen, Brush brush, Font font)
        {
            int tickHeight = 15;

            for (int j = 0; j < this.Height; j++)
            {
                if (j % 10 != 0) continue;

                int currentTickHeight = 0;
                bool isMajorTick = false;

                if (j % 100 == 0) { currentTickHeight = tickHeight; isMajorTick = true; }
                else if (j % 50 == 0) { currentTickHeight = tickHeight - 5; }
                else { currentTickHeight = tickHeight - 10; }

                if (j == 0) continue;

                // Риски слева
                g.DrawLine(pen, 0, j, currentTickHeight, j);
                // Риски справа
                g.DrawLine(pen, this.Width, j, this.Width - currentTickHeight, j);

                if (isMajorTick)
                {
                    string text = CalibrationSettings.ToUnits(j).ToString("F2");
                    float textY = j - font.Height / 2;
                    g.DrawString(text, font, brush, 10, textY);
                    
                    SizeF textSize = g.MeasureString(text, font);
                    g.DrawString(text, font, brush, this.Width - textSize.Width - 10, textY);
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
            double diagonalInUnits = CalibrationSettings.ToUnits(((int)diagonalInPixels));


            var infoLines = new List<string>
            {
                $"Width: {widthInUnits:F2} {unitName}",
                $"Height: {heightInUnits:F2} {unitName}",
                $"Diagonal: {diagonalInUnits:F2} {unitName}",
                $"Angle: {angleInDegrees:F2}°"
            };
            
            float centerX = this.Width / 2f;
            float totalTextHeight = infoLines.Count * font.Height;
            float startY = (this.Height - totalTextHeight) / 2f;

            StringFormat sf = new StringFormat { Alignment = StringAlignment.Center };

            for (int i = 0; i < infoLines.Count; i++)
            {
                float currentY = startY + (i * font.Height);
                g.DrawString(infoLines[i], font, brush, centerX, currentY, sf);
            }
        }

        private void RulerForm_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        #endregion

        #region Drag

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

        #region Context menu

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