
namespace ArrangeElements.Converters
{
  using System;
  using System.Globalization;
  using System.Windows;
  using System.Windows.Data;
  using System.Windows.Media;

  [ValueConversion(typeof(object[]), typeof(Geometry))]
  public class GeometryConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      var coords = Array.ConvertAll(values, value => (double)value);
      var points = CalculatePoints(coords[0], coords[1], coords[2], coords[3]);
      return new PathGeometry()
      {
        Figures = new PathFigureCollection()
        {
          new PathFigure()
          {
            StartPoint = points[0],
            Segments= new PathSegmentCollection()
            {
              new BezierSegment(points[1], points[2], points[3], true)
            }
          }
        }
      };
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    private static Point[] CalculatePoints(double firstX, double firstY, double secondX, double secondY)
    {
      Vector start = new Vector(firstX, firstY);
      Vector third = new Vector(secondX, secondY);
      Vector middle = Lerp(third, start, factor: 0.5);
      Vector first = new Vector(middle.X, start.Y);
      Vector second = new Vector(middle.X, third.Y);
      return new Point[] { (Point)start, (Point)first, (Point)second, (Point)third };
    }

    public static Vector Lerp(in Vector source, in Vector target, double factor)
    {
      double clamp = (factor < 0.0) ? 0.0 : (factor > 1.0) ? 1.0 : factor;
      return source + ((target - source) * clamp);
    }
  }
}
