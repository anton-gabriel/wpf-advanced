namespace Graph.ViewModel
{
  using Prism.Mvvm;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Windows;

  public sealed class GraphViewModel : BindableBase
  {
    public GraphViewModel()
    {
      Segments = new ObservableCollection<SegmentViewModel>()
      {
        new SegmentViewModel(new List<Point>()
        {
          new Point(100, 100),
          new Point(200, 100),
          new Point(300, 50),
          new Point(400, 200),
          new Point(500, 400),
          new Point(600, 200),
          new Point(700, 400),
          new Point(900, 200),
        }),
      };
    }

    public ObservableCollection<SegmentViewModel> Segments { get; set; }
  }
}
