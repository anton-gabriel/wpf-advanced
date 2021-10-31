namespace Graph.ViewModel
{
  using Prism.Mvvm;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Windows;

  public sealed class SegmentViewModel : BindableBase
  {
    public SegmentViewModel(IEnumerable<Point> points)
    {
      Points = new ObservableCollection<Point>(points);
    }

    public ObservableCollection<Point> Points { get; }
  }
}
