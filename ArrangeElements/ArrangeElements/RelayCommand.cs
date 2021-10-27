namespace ArrangeElements
{

  using System;
  using System.Windows.Input;

  public abstract class RelayCommand<T> : ICommand where T : class
  {
    public abstract void ExecuteCommand(T parameter);

    public virtual bool CanExecuteCommand(T parameter)
    {
      return true;
    }

    public void RaiseCanExecuteChanged()
    {
      CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    #region ICommand
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return CanExecuteCommand(parameter as T);
    }
    public void Execute(object parameter)
    {
      ExecuteCommand(parameter as T);
    }
    #endregion
  }
}
