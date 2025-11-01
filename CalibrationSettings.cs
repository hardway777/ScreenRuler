using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenRuler
{
    public static class CalibrationSettings
    {
        public static double Pixels { get; set; } = 100;
        public static double UnitsValue { get; set; } = 1.0;
        public static string UnitName { get; set; } = "m";

        public static double ToUnits(int pixels)
        {
            if (Pixels == 0) return 0;
            return (pixels / Pixels) * UnitsValue;
        }
    }
}
