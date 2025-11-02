using System.Drawing;

namespace ScreenRuler
{
    public static class DrawingHelpers
    {
        public static void DrawStringWithShadow(Graphics g, string text, Font font, Brush mainBrush, PointF position, Color shadowColor)
        {
            using (Brush shadowBrush = new SolidBrush(Color.FromArgb(64, shadowColor)))
            {
                g.DrawString(text, font, shadowBrush, (float)position.X - 0.5f, (float)position.Y - 0.5f);
                g.DrawString(text, font, shadowBrush, (float)position.X + 0.5f, (float)position.Y - 0.5f);
                g.DrawString(text, font, shadowBrush, (float)position.X - 0.5f, (float)position.Y + 0.5f);
                g.DrawString(text, font, shadowBrush, (float)position.X + 0.5f, (float)position.Y + 0.5f);
            }
            g.DrawString(text, font, mainBrush, position);
        }

        public static void DrawStringWithShadow(Graphics g, string text, Font font, Brush mainBrush, PointF position, StringFormat format, Color shadowColor)
        {
            using (Brush shadowBrush = new SolidBrush(Color.FromArgb(64, shadowColor)))
            {
                g.DrawString(text, font, shadowBrush, (float)position.X - 0.5f, (float)position.Y - 0.5f, format);
                g.DrawString(text, font, shadowBrush, (float)position.X + 0.5f, (float)position.Y - 0.5f, format);
                g.DrawString(text, font, shadowBrush, (float)position.X - 0.5f, (float)position.Y + 0.5f, format);
                g.DrawString(text, font, shadowBrush, (float)position.X + 0.5f, (float)position.Y + 0.5f, format);
            }
            g.DrawString(text, font, mainBrush, position, format);
        }

        public static void DrawStringWithShadow(Graphics g, string text, Font font, Brush mainBrush, RectangleF layoutRectangle, StringFormat format, Color shadowColor)
        {
            using (Brush shadowBrush = new SolidBrush(Color.FromArgb(64, shadowColor)))
            {
                var shadowRect = new RectangleF(layoutRectangle.X - 0.5f, layoutRectangle.Y - 0.5f, layoutRectangle.Width, layoutRectangle.Height);
                g.DrawString(text, font, shadowBrush, shadowRect, format);
                
                shadowRect = new RectangleF(layoutRectangle.X + 0.5f, layoutRectangle.Y - 0.5f, layoutRectangle.Width, layoutRectangle.Height);
                g.DrawString(text, font, shadowBrush, shadowRect, format);
                
                shadowRect = new RectangleF(layoutRectangle.X - 0.5f, layoutRectangle.Y + 0.5f, layoutRectangle.Width, layoutRectangle.Height);
                g.DrawString(text, font, shadowBrush, shadowRect, format);                
                
                shadowRect = new RectangleF(layoutRectangle.X + 0.5f, layoutRectangle.Y + 0.5f, layoutRectangle.Width, layoutRectangle.Height);
                g.DrawString(text, font, shadowBrush, shadowRect, format);                  
            }
            g.DrawString(text, font, mainBrush, layoutRectangle, format);
        }
    }
}