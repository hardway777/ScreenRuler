using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace ScreenRuler.Shapes
{
    public class AngleShape : IShape
    {
        public Point P1 { get; set; }
        public Point Vertex { get; set; }
        public Point P3 { get; set; }
        public Color Color { get; set; }
        public bool MeasureOuterAngle { get; set; }

        public void Draw(Graphics g, Font font)
        {
            using (var pen = new Pen(this.Color, 2))
            using (var brush = new SolidBrush(this.Color))
            {
                DrawAngle(g, pen, brush, font, P1, Vertex, P3, MeasureOuterAngle);
            }
        }

        public static void DrawAngle(Graphics g, Pen pen, Brush brush, Font font, Point p1, Point vertex, Point p3, bool measureOuter)
        {
            LineShape.DrawLineWithLength(g, pen, brush, font, p1, vertex);
            LineShape.DrawLineWithLength(g, pen, brush, font, vertex, p3);

            double angleRad1 = Math.Atan2(p1.Y - vertex.Y, p1.X - vertex.X);
            double angleRad3 = Math.Atan2(p3.Y - vertex.Y, p3.X - vertex.X);

            double sweepRad = angleRad3 - angleRad1;

            // Normalize sweep to be between -PI and +PI (-180 to 180 degrees)
            while (sweepRad <= -Math.PI) sweepRad += 2 * Math.PI;
            while (sweepRad > Math.PI) sweepRad -= 2 * Math.PI;

            double angleDeg = Math.Abs(sweepRad * (180.0 / Math.PI));

            if (measureOuter)
            {
                angleDeg = 360 - angleDeg;
                // Reverse the direction and draw the long way around
                sweepRad = (2 * Math.PI - Math.Abs(sweepRad)) * -Math.Sign(sweepRad);
            }

            float startAngle = (float)(angleRad1 * (180.0 / Math.PI));
            float sweepAngle = (float)(sweepRad * (180.0 / Math.PI));

            int arcRadius = 30;
            var rect = new Rectangle(vertex.X - arcRadius, vertex.Y - arcRadius, arcRadius * 2, arcRadius * 2);

            // Safety check to prevent GDI+ error
            if (rect.Width > 0 && rect.Height > 0)
            {
                g.DrawArc(pen, rect, startAngle, sweepAngle);
            }

            string text = $"{angleDeg:F1}°";
            double bisectorRad = angleRad1 + sweepRad / 2;
            float textDist = arcRadius + 10;
            var textPosition = new PointF(
                vertex.X + textDist * (float)Math.Cos(bisectorRad),
                vertex.Y + textDist * (float)Math.Sin(bisectorRad)
            );

            DrawingHelpers.DrawStringWithShadow(g, text, font, brush, textPosition, Color.White);
        }

        public IEnumerable<Point> GetSnapPoints()
        {
            yield return P1;
            yield return Vertex;
            yield return P3;
        }
    }
}