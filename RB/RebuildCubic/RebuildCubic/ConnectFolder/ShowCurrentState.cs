using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RebuildCubic.ConnectFolder {
  internal class ShowCurrentState : INotifyPropertyChanged, ICommand {
    private string _CurrentState;
    public string CurrentState {
      get {
        // show not connect at first time start
        if (_CurrentState == null) {
          return "Not Connected";
        } else {
          return _CurrentState;
        }
      }
      set {
        _CurrentState = value;
        OnPropertyChanged("CurrentState");
      }
    }
    public void Execute(object parameter) {
      CurrentState = ConnectControl.CurrentState;
    }

    public bool CanExecute(object parameter) {
      return true;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public event EventHandler CanExecuteChanged;

    private void OnPropertyChanged([CallerMemberName] string propName = "Not Connected") {
      PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }
  }
}
