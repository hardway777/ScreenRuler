using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace ScreenRuler.Shapes
{
    public class GridShape : IShape
    {
        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public double CellSize { get; set; }
        public Color Color { get; set; }

        public void Draw(Graphics g, Font font)
        {
            var rect = RectangleShape.GetRectangle(P1, P2);

            using (var pen = new Pen(this.Color, 2))
            using (var gridPen = new Pen(Color.FromArgb(160, this.Color), 1))
            using (var brush = new SolidBrush(this.Color))
            {
                g.DrawRectangle(pen, rect);

                double pixelPerUnit = CalibrationSettings.Pixels / CalibrationSettings.UnitsValue;
                double pixelCellSize = this.CellSize * pixelPerUnit;

                if (pixelCellSize > 1)
                {
                    for (double x = rect.Left + pixelCellSize; x < rect.Right; x += pixelCellSize)
                    {
                        g.DrawLine(gridPen, (float)x, rect.Top, (float)x, rect.Bottom);
                    }
                    for (double y = rect.Top + pixelCellSize; y < rect.Bottom; y += pixelCellSize)
                    {
                        g.DrawLine(gridPen, rect.Left, (float)y, rect.Right, (float)y);
                    }
                }

                string text = $"Grid: {CellSize:F2}x{CellSize:F2} {CalibrationSettings.UnitName}";
                g.DrawString(text, font, brush, rect.Location.X + 5, rect.Location.Y + 5);
            }
        }

        public IEnumerable<Point> GetSnapPoints()
        {
            var rect = RectangleShape.GetRectangle(P1, P2);
            yield return new Point(rect.Left, rect.Top);
            yield return new Point(rect.Right, rect.Top);
            yield return new Point(rect.Left, rect.Bottom);
            yield return new Point(rect.Right, rect.Bottom);
        }
    }
}