namespace ArrangeElements.View.Behavior
{
  using Microsoft.Xaml.Behaviors;
  using System.Windows;
  using System.Windows.Input;
  using System.Windows.Media;

  public sealed class ZoomBehavior : Behavior<FrameworkElement>
  {
    private static readonly double _MinZoom = 0.25;
    private static readonly double _MaxZoom = 2;

    protected override void OnAttached()
    {
      base.OnAttached();
      AssociatedObject.PreviewMouseWheel += Zoom;
    }

    protected override void OnDetaching()
    {
      AssociatedObject.PreviewMouseWheel -= Zoom;
      base.OnDetaching();
    }

    private void Zoom(object sender, System.Windows.Input.MouseWheelEventArgs e)
    {
      if (e.Source is FrameworkElement element)
      {
        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
          if (e.Delta > 0)
          {
            Zoom(element, factor: 0.1);
          }
          else if (e.Delta < 0)
          {
            Zoom(element, factor: -0.1);
          }
        }
      }
    }

    private static void Zoom(FrameworkElement element, double factor)
    {
      ScaleTransform scale = new ScaleTransform(element.LayoutTransform.Value.M11 + factor, element.LayoutTransform.Value.M22 + factor);
      if (scale.ScaleX > _MinZoom && scale.ScaleX < _MaxZoom)
      {
        element.LayoutTransform = scale;
        element.UpdateLayout();
      }
    }
  }
}
