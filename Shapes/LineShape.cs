using System;
using System.Collections.Generic;
using System.Drawing;

namespace ScreenRuler.Shapes
{
    public class LineShape : IShape
    {
        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public Color Color { get; set; }

        public void Draw(Graphics g, Font font)
        {
            using (var pen = new Pen(this.Color, 2))
            using (var brush = new SolidBrush(this.Color))
            {
                DrawLineWithLength(g, pen, brush, font, P1, P2);
            }
        }

        public static void DrawLineWithLength(Graphics g, Pen pen, Brush brush, Font font, Point p1, Point p2)
        {
            g.DrawLine(pen, p1, p2);
            g.FillEllipse(brush, p1.X - 3, p1.Y - 3, 6, 6);
            g.FillEllipse(brush, p2.X - 3, p2.Y - 3, 6, 6);

            double distanceInPixels = Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
            double distanceInUnits = CalibrationSettings.ToUnits(distanceInPixels);
            string text = $"{distanceInUnits:F2} {CalibrationSettings.UnitName}";

            Point textPosition = new Point(p2.X + 5, p2.Y + 5);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            DrawingHelpers.DrawStringWithShadow(g, text, font, brush, textPosition, sf, Color.White);
            //g.DrawString(text, font, brush, textPosition);
        }

        public IEnumerable<Point> GetSnapPoints()
        {
            yield return P1;
            yield return P2;
        }
    }
}