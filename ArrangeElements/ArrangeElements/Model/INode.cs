namespace ArrangeElements.Model
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using ArrangeElements.ViewModel;

  public interface INode
  {
    double X { get; }
    double Y { get; }
    double Width { get; }
    double Height { get; }
  }


  public interface IStage
  {
    void ComputeLayout(IEnumerable<ElementViewModel> nodes);
  }

  public sealed class CategoryStage : IStage
  {
    public void ComputeLayout(IEnumerable<ElementViewModel> nodes)
    {
      int x = 100;
      int y = 100;
      var categories = nodes.GroupBy(element => element.Category);
      foreach (var category in categories)
      {
        foreach (var element in category)
        {
          element.X = x;
          element.Y = y;
        }
        x += 100;
      }
    }
  }

  public sealed class ConnectionsStage : IStage
  {
    private readonly IEnumerable<ConnectionViewModel> _Connections;

    public ConnectionsStage(IEnumerable<ConnectionViewModel> connections)
    {
      _Connections = connections ?? throw new System.ArgumentNullException(nameof(connections));
    }

    public void ComputeLayout(IEnumerable<ElementViewModel> nodes)
    {
      var components = nodes.OrderBy(node => node.X).ThenBy(node => node.Category);
      ISet<ElementViewModel> visited = new HashSet<ElementViewModel>() { components.First() };
      Queue<ElementViewModel> queue = new Queue<ElementViewModel>();
      queue.Enqueue(components.First());
      int x = 100;

      while (queue.Any())
      {
        int y = 100;
        var current = queue.Dequeue();
        var neighbors =
          GetNeighbors(_Connections, current)
          .Except(visited)
          .Where(node => node.Category != Enums.ElementCategory.L);
        if (neighbors.Any())
        {
          x += 100;
        }

        foreach (var neighbor in neighbors)
        {
          neighbor.Y = y;
          neighbor.X = x;
          y += 100;
          queue.Enqueue(neighbor);
          visited.Add(neighbor);
        }

        var unvisitedNodes = components.Except(visited);
        if (!queue.Any() && unvisitedNodes.Any())
        {
          queue.Enqueue(unvisitedNodes.First());
          visited.Add(unvisitedNodes.First());
        }
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

  public sealed class DependencyStage : IStage
  {
    private readonly IEnumerable<ConnectionViewModel> _Connections;

    public DependencyStage(IEnumerable<ConnectionViewModel> connections)
    {
      _Connections = connections ?? throw new System.ArgumentNullException(nameof(connections));
    }

    public void ComputeLayout(IEnumerable<ElementViewModel> nodes)
    {
      var lCategory = nodes.Where(node => node.Category == Enums.ElementCategory.L);
      foreach (var l in lCategory)
      {
        var a = _Connections
          .Where(conn => conn.Second == l && conn.First.Category == Enums.ElementCategory.A)
          .Select(conn => conn.First)
          .FirstOrDefault();

        if (a != null)
        {
          l.X = a.X + 100;
          l.Y = a.Y;
        }
      }

      var t = nodes.FirstOrDefault(node => node.Category == Enums.ElementCategory.T);
      if (t != null)
      {
        double maxX = nodes.Max(node => node.X);
        t.X = maxX + 100;
        var ls = nodes.Where(node => node.Category == Enums.ElementCategory.L);
        if (ls.Any())
        {
          t.Y = ls.Average(node => node.Y);
        }
      }
    }
  }


  public sealed class ClusterStagte : IStage
  {
    private readonly IEnumerable<ConnectionViewModel> _Connections;

    public ClusterStagte(IEnumerable<ConnectionViewModel> connections)
    {
      _Connections = connections ?? throw new System.ArgumentNullException(nameof(connections));
    }

    public void ComputeLayout(IEnumerable<ElementViewModel> nodes)
    {
      foreach (var node in nodes)
      {
        var neighbors = GetNeighbors(_Connections, node);
        bool isConnectedToCluster = neighbors.GroupBy(n => n.Category).Any(group => group.Count() > 1);
        if (isConnectedToCluster)
        {
          node.Y = neighbors.Average(c => c.Y);
        }
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
