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

                var p1 = new Point(rect.Left, rect.Top);
                var p2 = new Point(rect.Right, rect.Top);
                var p3 = new Point(rect.Right, rect.Bottom);
                var p4 = new Point(rect.Left, rect.Bottom);

                DrawSide(g, font, brush, p1, p2); // Top
                DrawSide(g, font, brush, p2, p3); // Right
                DrawSide(g, font, brush, p4, p3); // Bottom
                DrawSide(g, font, brush, p1, p4); // Left
            }
        }

        private void DrawSide(Graphics g, Font font, Brush brush, Point p1, Point p2)
        {
            double distanceInPixels = Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
            if (distanceInPixels < 20) return;

            double distanceInUnits = CalibrationSettings.ToUnits(distanceInPixels);
            string text = $"{distanceInUnits:F2}";

            var midpoint = new PointF((p1.X + p2.X) / 2.0f, (p1.Y + p2.Y) / 2.0f);
            float angle = (float)(Math.Atan2(p2.Y - p1.Y, p2.X - p1.X) * 180.0 / Math.PI);

            DrawingHelpers.DrawRotatedStringWithBox(g, text, font, brush, midpoint, angle);
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