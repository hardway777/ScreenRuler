using System;
using System.Collections.Generic;
using System.Drawing;

namespace ScreenRuler.Shapes
{
    public class PerspectiveShape : IShape
    {
        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public Point P3 { get; set; }
        public Point P4 { get; set; }
        public double TrueBaseWidth { get; set; }
        public double TrueHeight { get; set; }
        public Color Color { get; set; }

        public void Draw(Graphics g, Font font)
        {
            using (var pen = new Pen(this.Color, 2))
            using (var brush = new SolidBrush(Color.FromArgb(40, this.Color)))
            {
                var points = new Point[] { P1, P2, P4, P3 };
                g.FillPolygon(brush, points);
                g.DrawPolygon(pen, points);
            }
        }

        public IEnumerable<Point> GetSnapPoints()
        {
            yield return P1;
            yield return P2;
            yield return P3;
            yield return P4;
        }

        public bool IsPointInside(Point p)
        {
            Point[] polygon = { P1, P2, P4, P3 };
            bool inside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if (((polygon[i].Y > p.Y) != (polygon[j].Y > p.Y)) &&
                    (p.X < (polygon[j].X - polygon[i].X) * (p.Y - polygon[i].Y) / (double)(polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    inside = !inside;
                }
            }
            return inside;
        }

        private (double w1, double w2) GetBarycentricCoords(PointF p, PointF a, PointF b, PointF c)
        {
            var v0 = new PointF(b.X - a.X, b.Y - a.Y);
            var v1 = new PointF(c.X - a.X, c.Y - a.Y);
            var v2 = new PointF(p.X - a.X, p.Y - a.Y);

            double d00 = v0.X * v0.X + v0.Y * v0.Y;
            double d01 = v0.X * v1.X + v0.Y * v1.Y;
            double d11 = v1.X * v1.X + v1.Y * v1.Y;
            double d20 = v2.X * v0.X + v2.Y * v0.Y;
            double d21 = v2.X * v1.X + v2.Y * v1.Y;

            double denom = d00 * d11 - d01 * d01;
            if (Math.Abs(denom) < 1e-9) return (-1, -1);

            double w1 = (d11 * d20 - d01 * d21) / denom;
            double w2 = (d00 * d21 - d01 * d20) / denom;

            return (w1, w2);
        }

        private (double u, double v) GetNormalizedCoordinates(Point p)
        {
            var pF = new PointF(p.X, p.Y);
            var p1F = new PointF(P1.X, P1.Y);
            var p2F = new PointF(P2.X, P2.Y);
            var p3F = new PointF(P3.X, P3.Y);
            var p4F = new PointF(P4.X, P4.Y);

            // Triangle 1: P1, P2, P3
            var (w1_T1, w2_T1) = GetBarycentricCoords(pF, p1F, p2F, p3F);
            if (w1_T1 >= 0 && w2_T1 >= 0 && (w1_T1 + w2_T1) <= 1)
            {
                // Point is in the first triangle. Map to UV space.
                // P1 -> (0,0), P2 -> (1,0), P3 -> (0,1)
                double u = w1_T1;
                double v = w2_T1;
                return (u, v);
            }

            // Triangle 2: P2, P4, P3
            var (w1_T2, w2_T2) = GetBarycentricCoords(pF, p2F, p4F, p3F);
            if (w1_T2 >= 0 && w2_T2 >= 0 && (w1_T2 + w2_T2) <= 1)
            {
                // Point is in the second triangle. Map to UV space.
                // P2 -> (1,0), P4 -> (1,1), P3 -> (0,1)
                var uv_p2 = new PointF(1, 0);
                var uv_p4 = new PointF(1, 1);
                var uv_p3 = new PointF(0, 1);
                double w0_T2 = 1.0 - w1_T2 - w2_T2;

                double u = w0_T2 * uv_p2.X + w1_T2 * uv_p4.X + w2_T2 * uv_p3.X;
                double v = w0_T2 * uv_p2.Y + w1_T2 * uv_p4.Y + w2_T2 * uv_p3.Y;
                return (u, v);
            }

            return (-1, -1); // Point is outside the quadrilateral
        }

        public double GetPerspectiveCorrectedLength(Point start, Point end)
        {
            var (u1, v1) = GetNormalizedCoordinates(start);
            var (u2, v2) = GetNormalizedCoordinates(end);

            if (u1 < 0 || u2 < 0) return -1;

            double trueX1 = u1 * TrueBaseWidth;
            double trueY1 = v1 * TrueHeight;
            double trueX2 = u2 * TrueBaseWidth;
            double trueY2 = v2 * TrueHeight;

            return Math.Sqrt(Math.Pow(trueX2 - trueX1, 2) + Math.Pow(trueY2 - trueY1, 2));
        }

        public Point GetClosestPointOnBoundary(Point p)
        {
            Point[] polygon = { P1, P2, P4, P3 };
            double minDistanceSq = double.MaxValue;
            Point closestPoint = Point.Empty;

            for (int i = 0; i < polygon.Length; i++)
            {
                Point a = polygon[i];
                Point b = polygon[(i + 1) % polygon.Length];

                double dx = b.X - a.X;
                double dy = b.Y - a.Y;

                if (dx == 0 && dy == 0)
                {
                    closestPoint = a;
                    minDistanceSq = Math.Pow(p.X - a.X, 2) + Math.Pow(p.Y - a.Y, 2);
                    continue;
                }

                double t = ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / (dx * dx + dy * dy);
                t = Math.Max(0, Math.Min(1, t));

                Point currentClosest = new Point((int)(a.X + t * dx), (int)(a.Y + t * dy));
                double distanceSq = Math.Pow(p.X - currentClosest.X, 2) + Math.Pow(p.Y - currentClosest.Y, 2);

                if (distanceSq < minDistanceSq)
                {
                    minDistanceSq = distanceSq;
                    closestPoint = currentClosest;
                }
            }
            return closestPoint;
        }
    }
}