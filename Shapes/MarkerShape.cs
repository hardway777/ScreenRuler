using System.Drawing;
using System.Collections.Generic;

namespace ScreenRuler.Shapes
{
    public class MarkerShape : IShape
    {
        public Point Position { get; set; }
        public Color Color { get; set; }

        public void Draw(Graphics g, Font font)
        {
            using (var pen = new Pen(this.Color, 2))
            using (var brush = new SolidBrush(this.Color))
            {
                int size = 8;
                g.DrawLine(pen, Position.X - size, Position.Y - size, Position.X + size, Position.Y + size);
                g.DrawLine(pen, Position.X + size, Position.Y - size, Position.X - size, Position.Y + size);

                double xUnits = CalibrationSettings.ToUnits(Position.X);
                double yUnits = CalibrationSettings.ToUnits(Position.Y);
                string text = $"({xUnits:F1}, {yUnits:F1})";
                
                DrawingHelpers.DrawStringWithShadow(g, text, font, brush, new PointF(Position.X + size, Position.Y + size), Color.White);
            }
        }

        public IEnumerable<Point> GetSnapPoints()
        {
            yield return Position;
        }
    }
}