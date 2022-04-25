using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace RebuildCubic.ConnectFolder {
  /// <summary>
  /// The value of this class is binding on ConnectControl.xaml.cs 
  /// and MainWindow.xaml, each value is trace to MainWindow.xaml
  /// </summary>
  internal class ChangeState : INotifyPropertyChanged, ICommand {

       // CurrentState
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

    // InventoryButtonSwitch
    private bool _InventoryButtonSwitch;
    public bool InventoryButtonSwitch {
      get {
        return _InventoryButtonSwitch;
      }
      set {
        _InventoryButtonSwitch = value;
        OnPropertyChanged("InventoryButtonSwitch");
      }
    }

    // TriggerButtonSwitch
    private bool _TriggerButtonSwitch;
    public bool TriggerButtonSwitch {
      get {
        return _TriggerButtonSwitch;
      }
      set {
        _TriggerButtonSwitch = value;
        OnPropertyChanged("TriggerButtonSwitch");
      }
    }

    private Visibility _StateLabelVisibility;
    public Visibility StateLabelVisibility {
      get {
        return _StateLabelVisibility;
      }
      set {
        _StateLabelVisibility = value;
        OnPropertyChanged("StateLabelVisibility");
      }
    }

    private Visibility _ListNameBoxVisibility;
    public Visibility ListNameBoxVisibility {
      get {
        return _ListNameBoxVisibility;
      }
      set {
        _ListNameBoxVisibility = value;
        OnPropertyChanged("ListNameBoxVisibility");
      }
    }



    // set variable in this class from ConnectControl
    public void Execute(object parameter) {
      CurrentState = ConnectControl.CurrentState;
      InventoryButtonSwitch = ConnectControl.InventoryButtonSwitch;
      TriggerButtonSwitch = ConnectControl.TriggerButtonSwitch;
      StateLabelVisibility = ConnectControl.StateLabelVisibility;
      ListNameBoxVisibility = DataTableFolder.DataTableControl.ListNameBoxVisibility;
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

