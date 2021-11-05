namespace ArrangeElements.ViewModel
{
  using ArrangeElements.Model.Enums;
  using Prism.Commands;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Windows.Input;

  public sealed class MainViewModel : NotifyPropertyChanged
  {
    public MainViewModel()
    {
      Elements = new ObservableCollection<ElementViewModel>()
      {
        new ElementViewModel(ElementCategory.B)  {X =100, Y=300 },
        new ElementViewModel(ElementCategory.C)  {X =200, Y=100 },
        new ElementViewModel(ElementCategory.C)  {X =200, Y=200 },
        new ElementViewModel(ElementCategory.C)  {X =200, Y=300 },
        new ElementViewModel(ElementCategory.C)  {X =200, Y=400 },
        new ElementViewModel(ElementCategory.C)  {X =200, Y=500 },
        new ElementViewModel(ElementCategory.C)  {X =400, Y=100 },
        new ElementViewModel(ElementCategory.C)  {X =400, Y=200 },
        new ElementViewModel(ElementCategory.C)  {X =400, Y=300 },
        new ElementViewModel(ElementCategory.C)  {X =400, Y=400 },
        new ElementViewModel(ElementCategory.C)  {X =400, Y=500 },
        new ElementViewModel(ElementCategory.P) {X =300, Y=300 },
        new ElementViewModel(ElementCategory.A) {X =200, Y=600 },
        new ElementViewModel(ElementCategory.A) {X =500, Y=300 },
        new ElementViewModel(ElementCategory.L)  {X =300, Y=600 },
        new ElementViewModel(ElementCategory.L)  {X =600, Y=300 },
        new ElementViewModel(ElementCategory.T)  {X =700, Y=300 }
      };

      Connections = new ObservableCollection<ConnectionViewModel>()
      {
        new ConnectionViewModel(Elements.ElementAt(0),Elements.ElementAt(1)),//b 5C1
        new ConnectionViewModel(Elements.ElementAt(0),Elements.ElementAt(2)),
        new ConnectionViewModel(Elements.ElementAt(0),Elements.ElementAt(3)),
        new ConnectionViewModel(Elements.ElementAt(0),Elements.ElementAt(4)),
        new ConnectionViewModel(Elements.ElementAt(0),Elements.ElementAt(5)),

        new ConnectionViewModel(Elements.ElementAt(1),Elements.ElementAt(11)),//5C1 Pc
        new ConnectionViewModel(Elements.ElementAt(2),Elements.ElementAt(11)),
        new ConnectionViewModel(Elements.ElementAt(3),Elements.ElementAt(11)),
        new ConnectionViewModel(Elements.ElementAt(4),Elements.ElementAt(11)),
        new ConnectionViewModel(Elements.ElementAt(5),Elements.ElementAt(11)),

        new ConnectionViewModel(Elements.ElementAt(11),Elements.ElementAt(6)),//PC 5c2
        new ConnectionViewModel(Elements.ElementAt(11),Elements.ElementAt(7)),
        new ConnectionViewModel(Elements.ElementAt(11),Elements.ElementAt(8)),
        new ConnectionViewModel(Elements.ElementAt(11),Elements.ElementAt(9)),
        new ConnectionViewModel(Elements.ElementAt(11),Elements.ElementAt(10)),

        new ConnectionViewModel(Elements.ElementAt(6),Elements.ElementAt(13)),//5c2 ac2
        new ConnectionViewModel(Elements.ElementAt(7),Elements.ElementAt(13)),
        new ConnectionViewModel(Elements.ElementAt(8),Elements.ElementAt(13)),
        new ConnectionViewModel(Elements.ElementAt(9),Elements.ElementAt(13)),
        new ConnectionViewModel(Elements.ElementAt(10),Elements.ElementAt(13)),

        new ConnectionViewModel(Elements.ElementAt(12),Elements.ElementAt(14)),//AC1 - L1

        new ConnectionViewModel(Elements.ElementAt(14),Elements.ElementAt(16)),//L1 - t1

        new ConnectionViewModel(Elements.ElementAt(0),Elements.ElementAt(12)),//b ac1

        new ConnectionViewModel(Elements.ElementAt(13),Elements.ElementAt(15)),//ac2 - l2

        new ConnectionViewModel(Elements.ElementAt(15),Elements.ElementAt(16)),//l2 - t2
      };


      ToggleTooltipCommand = new DelegateCommand(() => ShowTooltip = !ShowTooltip);
      HideTooltipCommand = new DelegateCommand(() => ShowTooltip = false);
    }

    public ObservableCollection<ElementViewModel> Elements { get; set; }
    public ObservableCollection<ConnectionViewModel> Connections { get; set; }


    private bool _ShowTooltip;
    public bool ShowTooltip
    {
      get => _ShowTooltip;
      private set
      {
        _ShowTooltip = value;
        OnPropertyChanged();
      }
    }

    public ICommand ToggleTooltipCommand { get; }
    public ICommand HideTooltipCommand { get; }
  }
}
