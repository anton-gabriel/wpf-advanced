namespace ArrangeElements.Converters
{
  using System;
  using System.Globalization;
  using System.Windows.Data;

  [ValueConversion(typeof(object), typeof(Type))]
  public class InstanceTypeConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value?.GetType();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
