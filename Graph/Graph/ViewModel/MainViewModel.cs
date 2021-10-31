namespace Graph.ViewModel
{
  using Prism.Mvvm;

  public sealed class MainViewModel : BindableBase
  {
    public MainViewModel()
    {
      GraphVM = new GraphViewModel();
    }

    public GraphViewModel GraphVM { get; private set; }
  }
}
