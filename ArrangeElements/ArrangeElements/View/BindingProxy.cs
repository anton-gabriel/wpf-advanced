namespace ArrangeElements.View
{
  using System.Windows;

  public class BindingProxy : Freezable
  {
    static BindingProxy()
    {
      DataProperty = DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }

    public static readonly DependencyProperty DataProperty;

    public object Data
    {
      get => GetValue(DataProperty);
      set => SetValue(DataProperty, value);
    }

    protected override Freezable CreateInstanceCore()
    {
      return new BindingProxy();
    }
  }
}
