namespace ArrangeElements.Converters
{
  using System;
  using System.Globalization;
  using System.Windows;
  using System.Windows.Data;

  [ValueConversion(typeof(Thickness), typeof(Thickness))]
  public class CenterPointConverter : IMultiValueConverter
  {
    private double _xOffset;
    private double _yOffset;

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values[0] is Thickness thickness && values[1] is FrameworkElement element)
      {
        _xOffset = element.ActualWidth / 2;
        _yOffset = element.ActualHeight / 2;
        return new Thickness(thickness.Left - _xOffset, thickness.Top - _yOffset, thickness.Right, thickness.Bottom);
      }
      throw new ArgumentException(nameof(values));
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      return value is Thickness thickness
        ? new object[] { new Thickness(thickness.Left + _xOffset, thickness.Top + _yOffset, thickness.Right, thickness.Bottom) }
        : throw new ArgumentException(nameof(value));
    }
  }
}
