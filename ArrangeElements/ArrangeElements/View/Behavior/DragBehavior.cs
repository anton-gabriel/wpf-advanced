namespace ArrangeElements.View.Behavior
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;
  using Microsoft.Xaml.Behaviors;

  public sealed class DragBehavior : Behavior<UIElement>
  {
    private Point _AnchorPoint;

    protected override void OnAttached()
    {
      base.OnAttached();
      AssociatedObject.MouseLeftButtonDown += Capture;
      AssociatedObject.MouseLeftButtonUp += Release;
      AssociatedObject.MouseMove += Move;
    }

    protected override void OnDetaching()
    {
      AssociatedObject.MouseLeftButtonDown += Capture;
      AssociatedObject.MouseLeftButtonUp += Release;
      AssociatedObject.MouseMove += Move;
      base.OnDetaching();
    }

    private void Move(object sender, MouseEventArgs args)
    {
      if (args.Source is FrameworkElement element)
      {
        if (element.IsMouseCaptured)
        {
          var parent = element.FindParent<Panel>();
          var mousePosition = args.GetPosition(parent);

          double leftOffset = element.Margin.Left + mousePosition.X - _AnchorPoint.X;
          double rightOffset = element.Margin.Top + mousePosition.Y - _AnchorPoint.Y;

          var margin = new Thickness(leftOffset, rightOffset, element.Margin.Right, element.Margin.Bottom);

          if (margin.Left > 0 && margin.Top > 0)
          {
            element.Margin = margin;
            _AnchorPoint = mousePosition;
          }
        }
      }
    }

    private void Release(object sender, MouseButtonEventArgs args)
    {
      if (args.Source is FrameworkElement element)
      {
        element.ReleaseMouseCapture();
      }
    }

    private void Capture(object sender, MouseButtonEventArgs args)
    {
      if (args.Source is FrameworkElement element)
      {
        var parent = element.FindParent<Panel>();
        if (parent != null)
        {
          _AnchorPoint = args.GetPosition(parent);
          element.CaptureMouse();
        }
      }
    }
  }
}
