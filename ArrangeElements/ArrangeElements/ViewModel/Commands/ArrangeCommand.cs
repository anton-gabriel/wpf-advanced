namespace ArrangeElements.ViewModel.Commands
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  internal sealed class ArrangeCommand : RelayCommand<object>
  {
    private readonly Random _Random;
    private readonly IEnumerable<ElementViewModel> _Elements;
    private readonly IEnumerable<ConnectionViewModel> _Connections;

    public ArrangeCommand(IEnumerable<ElementViewModel> elements, IEnumerable<ConnectionViewModel> connections, Action<Exception> onException)
      : base(onException)
    {
      _Elements = elements ?? throw new ArgumentNullException(nameof(elements));
      _Connections = connections ?? throw new ArgumentNullException(nameof(connections));
      _Random = new Random();
    }

    protected override async Task ExecuteAsync(object parameter)
    {
      await Task.Run(() =>
      {
        if (!_Elements.Any())
        {
          return;
        }

        //Step 1: Group by category -> set X
        //{
        //  int x = 100;
        //  var categories = _Elements.GroupBy(element => element.Category);
        //  foreach (var category in categories)
        //  {
        //    foreach (var element in category)
        //    {
        //      element.X = x;
        //      element.Y = 100;
        //    }
        //    x += 100;
        //  }
        //}


        //Step 2
        var visited = new HashSet<ElementViewModel>();

        Queue<ElementViewModel> queue = new Queue<ElementViewModel>();
        queue.Enqueue(_Elements.First());

        double x = 100;
        while (queue.Any())
        {
          ElementViewModel current = queue.Dequeue();
          var neighbors = GetNeighbors(_Connections, current);

          double y = 100;
          if (!visited.Contains(current))
          {
            current.X = x;
          }
          bool added = false;
          foreach (var neighbor in neighbors)
          {
            if (!visited.Contains(neighbor))
            {
              if (!added)
              {
                x += 100;
                added = true;
              }
              neighbor.Y = y;
              neighbor.X = x;
              y += 100;
              queue.Enqueue(neighbor);
              visited.Add(neighbor);
            }
          }

          if (!visited.Contains(current))
          {
            double yAvg = neighbors.Average(n => n.Y);
            current.Y = yAvg;
          }
          visited.Add(current);
        }
      });
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
