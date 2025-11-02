using System;
using System.Drawing;
using System.Collections.Generic;

namespace ScreenRuler.Shapes
{
    public class RectangleShape : IShape
    {
        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public Color Color { get; set; }

        public void Draw(Graphics g, Font font)
        {
            using (var pen = new Pen(this.Color, 2))
            using (var brush = new SolidBrush(this.Color))
            {
                var rect = GetRectangle(P1, P2);
                g.DrawRectangle(pen, rect);

                double widthUnits = CalibrationSettings.ToUnits(rect.Width);
                double heightUnits = CalibrationSettings.ToUnits(rect.Height);
                string text = $"W: {widthUnits:F2}\nH: {heightUnits:F2}";
                
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    DrawingHelpers.DrawStringWithShadow(g, text, font, brush, rect, sf, Color.White);
                }
            }
        }

        public static Rectangle GetRectangle(Point p1, Point p2)
        {
            return new Rectangle(
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Abs(p1.X - p2.X),
                Math.Abs(p1.Y - p2.Y)
            );
        }

        public IEnumerable<Point> GetSnapPoints()
        {
            var rect = GetRectangle(P1, P2);
            yield return new Point(rect.Left, rect.Top);
            yield return new Point(rect.Right, rect.Top);
            yield return new Point(rect.Left, rect.Bottom);
            yield return new Point(rect.Right, rect.Bottom);
        }
    }
}