namespace ArrangeElements.ViewModel
{
  using ArrangeElements.Model.Enums;
  using System;
  using System.Collections.Generic;
  using System.Windows;

  public sealed class ElementViewModel : NotifyPropertyChanged, IEquatable<ElementViewModel>
  {
    private static long _Id = 0;
    public ElementViewModel(ElementCategory category)
    {
      Id = _Id += 1;
      Category = category;
    }

    private double _X;
    private double _Y;

    public long Id { get; }
    public ElementCategory Category { get; }
    public Thickness Margin
    {
      get => new Thickness(X, Y, 0, 0);
      set
      {
        X = value.Left;
        Y = value.Top;
        OnPropertyChanged(nameof(X));
        OnPropertyChanged(nameof(Y));
      }
    }

    public double X
    {
      get => _X;
      set
      {
        _X = value;
        OnPropertyChanged();
        OnPropertyChanged(nameof(Margin));
      }
    }

    public double Y
    {
      get => _Y;
      set
      {
        _Y = value;
        OnPropertyChanged();
        OnPropertyChanged(nameof(Margin));
      }
    }

    public override string ToString()
    {
      return $"{Category}  [ID: {Id} , X: {X}, Y: {Y}]";
    }

    public bool Equals(ElementViewModel model)
    {
      return Id == model.Id &&
        Category == model.Category;
    }

    public override bool Equals(object obj)
    {
      return obj is ElementViewModel model && Equals(model);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Id, Category);
    }

    public static bool operator ==(ElementViewModel left, ElementViewModel right)
    {
      return EqualityComparer<ElementViewModel>.Default.Equals(left, right);
    }

    public static bool operator !=(ElementViewModel left, ElementViewModel right)
    {
      return !(left == right);
    }
  }
}
