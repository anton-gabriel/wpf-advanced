namespace Graph.View
{
  using System;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;
  using System.Windows.Media;
  /// <summary>
  /// Interaction logic for ZoomTool.xaml
  /// </summary>
  public partial class ZoomTool : UserControl
  {
    private Panel _Container;
    private Point? _AnchorPoint;

    public ZoomTool()
    {
      InitializeComponent();

      Loaded += SelectionToolLoaded;
      Unloaded += SelectionToolUnloaded;
    }

    private void SelectionToolLoaded(object sender, RoutedEventArgs e)
    {
      _Container = Parent as Panel;
      if (_Container is null)
      {
        throw new InvalidOperationException($"The control must be used within a {nameof(Panel)} control.");
      }
      _Container.MouseLeftButtonDown += StartCapture;
      _Container.MouseLeftButtonUp += ReleaseCapture;
      _Container.MouseMove += MoveCapture;
    }

    private void MoveCapture(object sender, MouseEventArgs e)
    {
      if (IsMouseHold())
      {
        if (_AnchorPoint.HasValue)
        {
          Selection.Data = new RectangleGeometry()
          {
            Rect = new Rect(_AnchorPoint.Value, e.GetPosition(_Container))
          };
        }
      }
    }

    private void ReleaseCapture(object sender, MouseButtonEventArgs e)
    {
      if (_AnchorPoint.HasValue)
      {
        var data = Selection.Data as RectangleGeometry;

        ScaleTransform scale = new ScaleTransform(_Container.LayoutTransform.Value.M11 + .5, _Container.LayoutTransform.Value.M22 + .5);
        _Container.LayoutTransform = scale;
        _Container.UpdateLayout();

        _Container.ReleaseMouseCapture();
        Selection.Data = new RectangleGeometry(new Rect());
        _AnchorPoint = null;
      }
    }

    private void StartCapture(object sender, MouseButtonEventArgs e)
    {
      if (IsMouseHold() && _Container.IsMouseDirectlyOver)
      {
        Selection.Data = new RectangleGeometry()
        {
          Rect = new Rect()
        };
        _Container.CaptureMouse();
        _AnchorPoint = e.GetPosition(_Container);
      }
    }

    private void SelectionToolUnloaded(object sender, RoutedEventArgs e)
    {
      _Container.MouseLeftButtonDown -= StartCapture;
      _Container.MouseLeftButtonUp -= ReleaseCapture;
      _Container.MouseMove -= MoveCapture;
    }

    private static bool IsMouseHold()
    {
      bool hold = Mouse.LeftButton == MouseButtonState.Pressed;
      return hold;
    }
  }
}
