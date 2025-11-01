using ScreenRuler.Shapes;
using System.Collections.Generic;
using System.Drawing;

namespace ScreenRuler.State
{
    public class RulerState
    {
        public Point Location { get; set; }
        public Size Size { get; set; }
        public double CalibrationPixels { get; set; }
        public double CalibrationUnitsValue { get; set; }
        public string CalibrationUnitName { get; set; } = "m";
        public List<IShape> Shapes { get; set; } = new List<IShape>();
    }
}