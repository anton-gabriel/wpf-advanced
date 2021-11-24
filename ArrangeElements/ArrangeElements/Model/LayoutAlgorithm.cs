namespace ArrangeElements.Model
{
  using ArrangeElements.ViewModel;
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows;

  public static class LayoutAlgorithm
  {
    static LayoutAlgorithm()
    {

    }

    public static IEnumerable<Point> DoTopologicalSort(
      IEnumerable<ElementViewModel> elements,
      IEnumerable<ConnectionViewModel> connections)
    {
      ICollection<Point> coords = new List<Point>();
      var source = elements.OrderBy(e => e.Category).First(); //Should be the B

      int level = 0;
      Dictionary<ElementViewModel, int> levels = new();
      List<ElementViewModel> layer = new() { source };
      Dictionary<int, List<ElementViewModel>> layers = new();
      int maxLayerSize = 0;
      while (layer.Any())
      {
        layer.ForEach(elem => levels.Add(elem, level));
        layers.Add(level, layer.OrderBy(c => c.Category).ToList());
        layer = layer.SelectMany(elem => GetNeighbours(elem, connections).Except(levels.Keys)).ToHashSet().ToList();
        if (layer.Count > maxLayerSize)
        {
          maxLayerSize = layer.Count;
        }
        ++level;
      }

      foreach (var l in layers)
      {
        double y = 100 * (double)maxLayerSize / l.Value.Count / 2.0;
        foreach (var e in l.Value)
        {
          e.Y = y;
          y += 100 * (double)maxLayerSize / l.Value.Count;
        }
      }

      foreach (var element in elements)
      {
        coords.Add(new Point(100 + levels[element] * 100, element.Y));
      }

      return coords;
    }


    private static IEnumerable<ElementViewModel> GetNeighbours(ElementViewModel element, IEnumerable<ConnectionViewModel> connections)
    {
      return connections
        .Where(conn => conn.First == element || conn.Second == element)
        .Select(conn => conn.First == element ? conn.Second : conn.First);
    }
  }
}
