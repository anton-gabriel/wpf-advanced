namespace ArrangeElements.View
{
  using System;
  using System.Collections;
  using System.Linq;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;
  using System.Windows.Media;

  /// <summary>
  /// Interaction logic for SelectionTool.xaml
  /// </summary>
  public partial class SelectionTool : UserControl
  {
    public static readonly DependencyProperty ItemsTypeProperty;
    public static readonly DependencyProperty OnSelectCommandProperty;

    static SelectionTool()
    {
      ItemsTypeProperty = DependencyProperty.Register(nameof(ItemsType), typeof(Type), typeof(SelectionTool), new PropertyMetadata(null));
      OnSelectCommandProperty = DependencyProperty.Register(nameof(OnSelectCommand), typeof(ICommand), typeof(SelectionTool), new PropertyMetadata(null));
    }

    private Panel _Container;
    private Point? _AnchorPoint;

    public SelectionTool()
    {
      InitializeComponent();

      Loaded += SelectionToolLoaded;
      Unloaded += SelectionToolUnloaded;
    }

    public Type ItemsType
    {
      get => (Type)GetValue(ItemsTypeProperty);
      set => SetValue(ItemsTypeProperty, value);
    }

    public ICommand OnSelectCommand
    {
      get => (ICommand)GetValue(OnSelectCommandProperty);
      set => SetValue(OnSelectCommandProperty, value);
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
        var elements = GetChildrenOfType(_Container, ItemsType).OfType<FrameworkElement>();

        var selection = elements.Where(control =>
        {
          Point relativeLocation = control.TranslatePoint(new Point(0, 0), _Container);
          return data.Rect.Contains(relativeLocation);
        }).OrderBy(e => e.Margin.Left).ThenBy(e => e.Margin.Top);

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



    public static IEnumerable GetChildrenOfType(DependencyObject @object, Type type)
    {
      for (int index = 0; index < VisualTreeHelper.GetChildrenCount(@object); index++)
      {
        DependencyObject child = VisualTreeHelper.GetChild(@object, index);
        if (child != null && child.GetType() == type)
        {
          yield return child;
        }
        foreach (var childOfChild in GetChildrenOfType(child, type))
        {
          yield return childOfChild;
        }
      }
    }

    private static bool IsMouseHold()
    {
      bool hold = Mouse.LeftButton == MouseButtonState.Pressed;
      return hold;
    }
  }
}
