using System;
using System.Drawing;
using System.Collections.Generic;

namespace ScreenRuler.Shapes
{
    public class CircleShape : IShape
    {
        public Point Center { get; set; }
        public double Radius { get; set; }
        public Color Color { get; set; }

        public void Draw(Graphics g, Font font)
        {
            using (var pen = new Pen(this.Color, 2))
            using (var brush = new SolidBrush(this.Color))
            {
                float r = (float)this.Radius;
                g.DrawEllipse(pen, Center.X - r, Center.Y - r, r * 2, r * 2);

                double radiusUnits = CalibrationSettings.ToUnits(this.Radius);
                string text = $"R: {radiusUnits:F2} {CalibrationSettings.UnitName}";
                
                DrawingHelpers.DrawStringWithBox(g, text, font, brush, Center);
            }
        }

        public IEnumerable<Point> GetSnapPoints()
        {
            yield return Center;
            yield return new Point(Center.X + (int)Radius, Center.Y);
            yield return new Point(Center.X - (int)Radius, Center.Y);
            yield return new Point(Center.X, Center.Y + (int)Radius);
            yield return new Point(Center.X, Center.Y - (int)Radius);
        }
    }
}