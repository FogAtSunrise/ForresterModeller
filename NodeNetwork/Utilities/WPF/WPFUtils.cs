using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace NodeNetwork.Views.Controls
{
    public static class WPFUtils
    {
        public static T FindParent<T>(DependencyObject childObject) where T : DependencyObject
        {
            DependencyObject curObj = childObject;
            do
            {
                curObj = VisualTreeHelper.GetParent(curObj);
                if (curObj == null) return default(T);
            } while (!(curObj is T));
            return (T)curObj;
        }

        public static DependencyObject GetVisualAncestorNLevelsUp(DependencyObject childObject, int levels)
        {
            DependencyObject curObj = childObject;
            for (int i = 0; i < levels; i++)
            {
                curObj = VisualTreeHelper.GetParent(curObj);
                if (curObj == null) return null;
            }
            return curObj;
        }

        public static IEnumerable<Point> GetIntersectionPoints(Geometry g1, Geometry g2)
        {
            Geometry og1 = g1.GetWidenedPathGeometry(new Pen(Brushes.Black, 1.0));
            Geometry og2 = g2.GetWidenedPathGeometry(new Pen(Brushes.Black, 1.0));

            CombinedGeometry cg = new CombinedGeometry(GeometryCombineMode.Intersect, og1, og2);

            PathGeometry pg = cg.GetFlattenedPathGeometry();
            foreach (PathFigure figure in pg.Figures)
            {
                Rect fig = new PathGeometry(new[] { figure }).Bounds;
                yield return new Point(fig.Left + fig.Width / 2.0, fig.Top + fig.Height / 2.0);
            }
        }

        public static IEnumerable<T> FindDescendantsOfType<T>(DependencyObject root, bool skipChildrenOnHit) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < childCount; i++)
            {
                var obj = VisualTreeHelper.GetChild(root, i);
                if (obj is T t)
                {
                    yield return t;

                    if (skipChildrenOnHit)
                    {
                        continue;
                    }
                }

                foreach (var subChild in FindDescendantsOfType<T>(obj, skipChildrenOnHit))
                {
                    yield return subChild;
                }
            }
        }


        public static Point GetClosestPointOnPath(Point p, Geometry geometry)
        {
            PathGeometry pathGeometry = geometry.GetFlattenedPathGeometry();

            var points = pathGeometry.Figures.Select(f => GetClosestPointOnPathFigure(f, p))
                .OrderBy(t => t.Item2).FirstOrDefault();
            return (points == null) ? new Point(0, 0) : points.Item1;
        }


        private static Tuple<Point, double> GetClosestPointOnPathFigure(PathFigure figure, Point p)
        {
            List<Tuple<Point, double>> closePoints = new List<Tuple<Point, double>>();
            Point current = figure.StartPoint;
            foreach (PathSegment s in figure.Segments)
            {
                PolyLineSegment segment = s as PolyLineSegment;
                LineSegment line = s as LineSegment;
                Point[] points;
                if (segment != null)
                {
                    points = segment.Points.ToArray();
                }
                else if (line != null)
                {
                    points = new[] { line.Point };
                }
                else
                {
                    throw new InvalidOperationException("Unexpected segment type");
                }
                foreach (Point next in points)
                {
                    Point closestPoint = GetClosestPointOnLine(current, next, p);
                    double d = (closestPoint - p).LengthSquared;
                    closePoints.Add(new Tuple<Point, double>(closestPoint, d));
                    current = next;
                }
            }
            return closePoints.OrderBy(t => t.Item2).First();
        }

        private static Point GetClosestPointOnLine(Point start, Point end, Point p)
        {
            double length = (start - end).LengthSquared;
            if (length == 0.0)
            {
                return start;
            }
            Vector v = end - start;
            double param = (p - start) * v / length;
            return (param < 0.0) ? start : (param > 1.0) ? end : (start + param * v);
        }

    }
}
