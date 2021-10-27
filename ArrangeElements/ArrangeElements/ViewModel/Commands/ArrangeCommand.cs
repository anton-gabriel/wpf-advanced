namespace ArrangeElements.ViewModel.Commands
{
  using ArrangeElements.Model;
  using System;
  using System.Collections.Generic;
  using System.Linq;

  internal sealed class ArrangeCommand : RelayCommand<object>
  {
    private readonly Random _Random;
    private readonly IEnumerable<ElementViewModel> _Elements;
    private readonly IEnumerable<ConnectionViewModel> _Connections;

    public ArrangeCommand(IEnumerable<ElementViewModel> elements, IEnumerable<ConnectionViewModel> connections)
    {
      _Elements = elements ?? throw new ArgumentNullException(nameof(elements));
      _Connections = connections ?? throw new ArgumentNullException(nameof(connections));
      _Random = new Random();
    }

    public override void ExecuteCommand(object parameter)
    {
      if (!_Elements.Any())
      {
        return;
      }


      IEnumerable<IStage> stages = new List<IStage>
        {
          new CategoryStage(),
          new ConnectionsStage(_Connections),
          new ClusterStagte(_Connections),
          new DependencyStage(_Connections),
        };
      //Alta varianta -> Template/Sablon
      //Primu layer Body
      //Urmatorul Connectori daca sunt (si daca nu sunt connectati cu altceva)
      //Ac daca sunt

      foreach (var stage in stages)
      {
        stage.ComputeLayout(_Elements);
      }
    }

    private static IEnumerable<ElementViewModel> GetNeighbors(IEnumerable<ConnectionViewModel> connections, ElementViewModel elementViewModel)
    {
      var first = connections.Where(conn => conn.Second == elementViewModel).Select(conn => conn.First);
      var second = connections.Where(conn => conn.First == elementViewModel).Select(conn => conn.Second);
      var neighbors = first.Union(second);
      return neighbors;
    }
  }
}
