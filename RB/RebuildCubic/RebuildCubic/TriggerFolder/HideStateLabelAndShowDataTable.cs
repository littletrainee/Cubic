using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RebuildCubic.TriggerFolder {
  internal class HideStateLabelAndShowDataTable :INotifyPropertyChanged, ICommand {
    // CurrentState
    private string _CurrentStateLabelShow;
    public string CurrentStateLabelShow {
      get {
        // show not connect at first time start
          return _CurrentStateLabelShow;
      }
      set {
        _CurrentStateLabelShow = value;
        OnPropertyChanged("CurrentStateLabelShow");
      }
    }
    // set variable in this class from ConnectControl
    public void Execute(object parameter) {
      CurrentStateLabelShow = TriggerControl.CurrentStateLabelShow;
   }

    public bool CanExecute(object parameter) {
      return true;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public event EventHandler CanExecuteChanged;

    private void OnPropertyChanged([CallerMemberName] string propName = "") {
      PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }
  }
}
