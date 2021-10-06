namespace ArrangeElements
{
  using System;
  using System.Threading.Tasks;
  using System.Windows.Input;

  public abstract class RelayCommand<T> : ICommand where T : class
  {
    private readonly Action<Exception> _OnException;

    protected RelayCommand(Action<Exception> onException)
    {
      _OnException = onException;
    }

    private bool _isExecuting;
    public bool IsExecuting
    {
      get => _isExecuting;
      set
      {
        _isExecuting = value;
        CanExecuteChanged?.Invoke(this, new EventArgs());
      }
    }

    protected abstract Task ExecuteAsync(object parameter);

    public void RaiseCanExecuteChanged()
    {
      CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    #region ICommand
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return !IsExecuting;
    }

    public async void Execute(object parameter)
    {
      IsExecuting = true;

      try
      {
        await ExecuteAsync(parameter);
      }
      catch (Exception ex)
      {
        _OnException?.Invoke(ex);
      }

      IsExecuting = false;
    }
    #endregion
  }
}
