using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using RebuildCubic.DataTableFolder;
using RebuildCubic.TriggerFolder;
using RebuildCubic.InventoryFolder;

namespace RebuildCubic {
  /// <summary>
  /// The value of this class is binding on ConnectControl.xaml.cs 
  /// and MainWindow.xaml, each value is trace to MainWindow.xaml
  /// </summary>
  internal class ChangeState : INotifyPropertyChanged, ICommand {

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

    // set default to hidden for first time start
    private Visibility _TriggerButtonVisibility = Visibility.Hidden;
    public Visibility TriggerButtonVisibility {
      get {
        return _TriggerButtonVisibility;
      }
      set {
        _TriggerButtonVisibility = value;
        OnPropertyChanged("TriggerButtonVisibility");
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
      //InventoryButtonSwitch = InventoryControl.InventoryButtonSwitch;
      //ListNameBoxVisibility = DataTableControl.ListNameBoxVisibility;
      TriggerButtonSwitch = TriggerControl.TriggerButtonIsEnabled;
      TriggerButtonVisibility = TriggerControl.TriggerButtonVisibility;
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

