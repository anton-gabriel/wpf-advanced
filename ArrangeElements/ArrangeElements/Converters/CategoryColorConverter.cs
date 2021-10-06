namespace ArrangeElements.Converters
{
  using System;
  using System.Globalization;
  using System.Windows.Data;
  using System.Windows.Media;
  using ArrangeElements.Model.Enums;

  [ValueConversion(typeof(ElementCategory), typeof(Brush))]
  public class CategoryColorConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      ElementCategory category = (ElementCategory)value;
      Brush color = category switch
      {
        ElementCategory.B => new BrushConverter().ConvertFrom("#FFFFFF") as SolidColorBrush,
        ElementCategory.C => new BrushConverter().ConvertFrom("#3B413C") as SolidColorBrush,
        ElementCategory.P => new BrushConverter().ConvertFrom("#9DB5B2") as SolidColorBrush,
        ElementCategory.A => new BrushConverter().ConvertFrom("#DAF0EE") as SolidColorBrush,
        ElementCategory.L => new BrushConverter().ConvertFrom("#94D1BE") as SolidColorBrush,
        ElementCategory.T => new BrushConverter().ConvertFrom("#6C7B77") as SolidColorBrush,
        _ => Brushes.Black,
      };
      return color;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
