using System.Drawing;
using System.Drawing.Drawing2D;

namespace ScreenRuler
{
    public static class DrawingHelpers
    {
        #region Text in a Box

        private static GraphicsPath CreateRoundedRectanglePath(RectangleF rect, float radius)
        {
            var path = new GraphicsPath();
            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }
            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        public static void DrawStringWithBox(Graphics g, string text, Font font, Brush textBrush, PointF position, float cornerRadius = 4.0f)
        {
            SizeF textSize = g.MeasureString(text, font);
            float padding = 4;
            var boxRect = new RectangleF(
                position.X - textSize.Width / 2 - padding / 2,
                position.Y - textSize.Height / 2 - padding / 2,
                textSize.Width + padding,
                textSize.Height + padding
            );

            using (var path = CreateRoundedRectanglePath(boxRect, cornerRadius))
            using (var pen = new Pen(((SolidBrush)textBrush).Color, 0.5f))
            {
                g.FillPath(Brushes.White, path);
                g.DrawPath(pen, path);
            }

            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.DrawString(text, font, textBrush, position, sf);
            }
        }

        public static void DrawRotatedStringWithBox(Graphics g, string text, Font font, Brush textBrush, PointF position, float angle, float cornerRadius = 4.0f)
        {
            var savedState = g.Save();

            g.TranslateTransform(position.X, position.Y);

            float finalAngle = angle;
            if (finalAngle > 90 && finalAngle < 270)
            {
                finalAngle -= 180;
            }
            g.RotateTransform(finalAngle);

            SizeF textSize = g.MeasureString(text, font);
            float padding = 4;
            var boxRect = new RectangleF(
                -textSize.Width / 2 - padding / 2,
                -textSize.Height / 2 - padding / 2,
                textSize.Width + padding,
                textSize.Height + padding
            );

            using (var path = CreateRoundedRectanglePath(boxRect, cornerRadius))
            using (var pen = new Pen(((SolidBrush)textBrush).Color, 0.5f))
            {
                g.FillPath(Brushes.White, path);
                g.DrawPath(pen, path);
            }

            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.DrawString(text, font, textBrush, 0, 0, sf);
            }

            g.Restore(savedState);
        }

        #endregion

        #region Line Drawing

        public static void DrawLineWithLength(Graphics g, Pen pen, Brush brush, Font font, Point p1, Point p2)
        {
            g.DrawLine(pen, p1, p2);
            g.FillEllipse(brush, p1.X - 3, p1.Y - 3, 6, 6);
            g.FillEllipse(brush, p2.X - 3, p2.Y - 3, 6, 6);

            double distanceInPixels = Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
            if (distanceInPixels == 0) return;

            double distanceInUnits = CalibrationSettings.ToUnits(distanceInPixels);
            string text = $"{distanceInUnits:F2} {CalibrationSettings.UnitName}";

            var midpoint = new PointF((p1.X + p2.X) / 2.0f, (p1.Y + p2.Y) / 2.0f);
            float angle = (float)(Math.Atan2(p2.Y - p1.Y, p2.X - p1.X) * 180.0 / Math.PI);

            DrawRotatedStringWithBox(g, text, font, brush, midpoint, angle);
        }

        #endregion

        #region Text with Shadow

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

        #endregion
    }
}