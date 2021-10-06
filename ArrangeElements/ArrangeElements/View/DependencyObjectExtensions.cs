namespace ArrangeElements.View
{
  using System;
  using System.Collections.Generic;
  using System.Windows;
  using System.Windows.Media;

  internal static class DependencyObjectExtensions
  {
    public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
    {
      return (VisualTreeHelper.GetParent(child)) switch
      {
        null => null,
        T parent => parent,
        _ => FindParent<T>(VisualTreeHelper.GetParent(child)),
      };
      ;
    }

    public static IEnumerable<T> FindChildren<T>(this DependencyObject data, Predicate<T> condition) where T : DependencyObject
    {
      if (data != null)
      {
        for (int index = 0; index < VisualTreeHelper.GetChildrenCount(data); index++)
        {
          DependencyObject child = VisualTreeHelper.GetChild(data, index);
          if (child != null && child is T && condition(child as T))
          {
            yield return child as T;
          }
          foreach (var childOfChild in FindChildren(child, condition))
          {
            yield return childOfChild;
          }
        }
      }
    }
  }
}
