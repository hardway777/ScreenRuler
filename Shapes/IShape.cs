using System.Collections.Generic;
using System.Drawing;

namespace ScreenRuler.Shapes
{
    public interface IShape
    {
        void Draw(Graphics g, Font font);
        IEnumerable<Point> GetSnapPoints();
    }
}