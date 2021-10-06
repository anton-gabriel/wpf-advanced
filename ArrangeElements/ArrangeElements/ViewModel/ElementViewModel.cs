namespace ArrangeElements.ViewModel
{
  using System.Windows;
  using ArrangeElements.Model.Enums;

  public sealed class ElementViewModel : NotifyPropertyChanged
  {
    public ElementViewModel(ElementCategory category)
    {
      Category = category;
    }

    private double _X;
    private double _Y;

    public ElementCategory Category { get; }
    public Thickness Margin
    {
      get => new Thickness(X, Y, 0, 0);
      set
      {
        X = value.Left;
        Y = value.Top;
        OnPropertyChanged(nameof(X));
        OnPropertyChanged(nameof(Y));
      }
    }

    public double X
    {
      get => _X;
      set
      {
        _X = value;
        OnPropertyChanged();
        OnPropertyChanged(nameof(Margin));
      }
    }

    public double Y
    {
      get => _Y;
      set
      {
        _Y = value;
        OnPropertyChanged();
        OnPropertyChanged(nameof(Margin));
      }
    }

    public override string ToString()
    {
      return $"{Category}  [X: {X}, Y: {Y}]";
    }

  }
}
