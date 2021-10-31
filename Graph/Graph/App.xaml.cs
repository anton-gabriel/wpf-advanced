namespace Graph
{
  using Graph.ViewModel;
  using System.Windows;

  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      Window window = new MainWindow(new MainViewModel());
      window.Show();

      base.OnStartup(e);
    }
  }
}
