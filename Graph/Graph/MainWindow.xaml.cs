namespace Graph
{
  using Graph.ViewModel;
  using System.Windows;

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow(MainViewModel viewModel)
    {
      DataContext = viewModel;
      InitializeComponent();
    }
  }
}
