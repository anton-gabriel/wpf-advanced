namespace ArrangeElements.ViewModel
{
  public sealed class ConnectionViewModel : NotifyPropertyChanged
  {
    public ConnectionViewModel(ElementViewModel first, ElementViewModel second)
    {
      First = first ?? throw new System.ArgumentNullException(nameof(first));
      Second = second ?? throw new System.ArgumentNullException(nameof(second));
    }

    public ElementViewModel First { get; }
    public ElementViewModel Second { get; }
  }
}
