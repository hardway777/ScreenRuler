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
        public double? OverriddenLength { get; set; }

        public void Draw(Graphics g, Font font)
        {
            using (var pen = new Pen(this.Color, 2))
            using (var brush = new SolidBrush(this.Color))
            {
                g.DrawLine(pen, P1, P2);
                g.FillEllipse(brush, P1.X - 3, P1.Y - 3, 6, 6);
                g.FillEllipse(brush, P2.X - 3, P2.Y - 3, 6, 6);

                double length;
                if (OverriddenLength.HasValue && OverriddenLength.Value >= 0)
                {
                    length = OverriddenLength.Value;
                }
                else
                {
                    double distanceInPixels = Math.Sqrt(Math.Pow(P2.X - P1.X, 2) + Math.Pow(P2.Y - P1.Y, 2));
                    if (distanceInPixels == 0) return;
                    length = CalibrationSettings.ToUnits(distanceInPixels);
                }
                
                string text = $"{length:F2} {CalibrationSettings.UnitName}";
                var midpoint = new PointF((P1.X + P2.X) / 2.0f, (P1.Y + P2.Y) / 2.0f);
                float angle = (float)(Math.Atan2(P2.Y - P1.Y, P2.X - P1.X) * 180.0 / Math.PI);

                DrawingHelpers.DrawRotatedStringWithBox(g, text, font, brush, midpoint, angle);
            }
        }

        public IEnumerable<Point> GetSnapPoints()
        {
            yield return P1;
            yield return P2;
        }
    }
}