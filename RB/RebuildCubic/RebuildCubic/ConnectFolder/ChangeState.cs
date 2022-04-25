using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using JW.UHF;

namespace RebuildCubic.ConnectFolder {
  /// <summary>
  /// The value of this class is binding on ConnectControl.xaml.cs 
  /// and MainWindow.xaml, each value is trace to MainWindow.xaml
  /// </summary>
  internal class ChangeState : INotifyPropertyChanged, ICommand {

    //private string _CurrentStateLabelShow;
    //public string CurrentStateLabelShow {
    //  get {
    //    // show not connect at first time start
    //    return _CurrentStateLabelShow;
    //  }
    //  set {
    //    _CurrentStateLabelShow = value;
    //    OnPropertyChanged("CurrentStateLabelShow");
    //  }
    //}
    
    //// CurrentState
    //private string _CurrentState;
    //public string CurrentState {
    //  get {
    //    // show not connect at first time start
    //    if (_CurrentState == null) {
    //      return "Not Connected";
    //    } else {
    //      return _CurrentState;
    //    }
    //  }
    //  set {
    //    _CurrentState = value;
    //    OnPropertyChanged("CurrentState");
    //  }
    //}

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

    // ClearButtonSwitch
    private bool _ClearButtonSwitch;
    public bool ClearButtonSwitch {
      get {
        return _ClearButtonSwitch;
      }
      set {
        _ClearButtonSwitch = value;
        OnPropertyChanged("ClearButtonSwitch");
      }
    }

    // ClearButtonSwitch
    private bool _SetButtonSwitch;
    public bool SetButtonSwitch {
      get {
        return _SetButtonSwitch;
      }
      set {
        _SetButtonSwitch = value;
        OnPropertyChanged("SetButtonSwitch");
      }
    }

    // set variable in this class from ConnectControl
    public void Execute(object parameter) {
      //CurrentState = ConnectControl.CurrentState;
      InventoryButtonSwitch = ConnectControl.InventoryButtonSwitch;
      TriggerButtonSwitch = ConnectControl.TriggerButtonSwitch;
      ClearButtonSwitch = ConnectControl.ClearButtonSwitch;
      SetButtonSwitch = ConnectControl.SetButtonSwitch;
      //CurrentStateLabelShow = TriggerFolder.TriggerControl.CurrentStateLabelShow;
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

