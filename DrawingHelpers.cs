using System.Drawing;

namespace ScreenRuler
{
    public static class DrawingHelpers
    {
        public static void DrawStringWithShadow(Graphics g, string text, Font font, Brush mainBrush, PointF position, Color shadowColor)
        {
            using (Brush shadowBrush = new SolidBrush(Color.FromArgb(128, shadowColor)))
            {
                g.DrawString(text, font, shadowBrush, position.X + 1, position.Y + 1);
            }
            g.DrawString(text, font, mainBrush, position);
        }

        public static void DrawStringWithShadow(Graphics g, string text, Font font, Brush mainBrush, PointF position, StringFormat format, Color shadowColor)
        {
            using (Brush shadowBrush = new SolidBrush(Color.FromArgb(128, shadowColor)))
            {
                g.DrawString(text, font, shadowBrush, position.X + 1, position.Y + 1, format);
            }
            g.DrawString(text, font, mainBrush, position, format);
        }

        public static void DrawStringWithShadow(Graphics g, string text, Font font, Brush mainBrush, RectangleF layoutRectangle, StringFormat format, Color shadowColor)
        {
            using (Brush shadowBrush = new SolidBrush(Color.FromArgb(128, shadowColor)))
            {
                var shadowRect = new RectangleF(layoutRectangle.X + 1, layoutRectangle.Y + 1, layoutRectangle.Width, layoutRectangle.Height);
                g.DrawString(text, font, shadowBrush, shadowRect, format);
            }
            g.DrawString(text, font, mainBrush, layoutRectangle, format);
        }
    }
}