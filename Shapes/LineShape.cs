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
                DrawingHelpers.DrawLineWithLength(g, pen, brush, font, P1, P2);
            }
        }

        public IEnumerable<Point> GetSnapPoints()
        {
            yield return P1;
            yield return P2;
        }
    }
}