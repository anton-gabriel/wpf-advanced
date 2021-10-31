namespace Graph.Converters
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;
  using System.Windows;
  using System.Windows.Data;
  using System.Windows.Media;

  [ValueConversion(typeof(IEnumerable<Point>), typeof(Geometry))]
  public class GeometryConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is IEnumerable<Point> points && points.Any())
      {
        return new PathGeometry()
        {
          Figures = new PathFigureCollection()
          {
            new PathFigure()
            {
              StartPoint = points.First(),
              Segments= new PathSegmentCollection(new List<PathSegment>()
              {
                new PolyLineSegment(points.Skip(1), true)
              })
            }
          }
        };
      }
      return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
