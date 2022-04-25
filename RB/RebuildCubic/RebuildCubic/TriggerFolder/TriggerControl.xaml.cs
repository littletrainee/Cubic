using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace RebuildCubic.TriggerFolder {
  /// <summary>
  /// TriggerControl.xaml 的互動邏輯
  /// </summary>
  public partial class TriggerControl : UserControl {
    public DispatcherTimer Timer { get; set; }
    public static string CurrentStateLabelShow { get; set; }
    public TriggerControl() {
      InitializeComponent();
      CurrentStateLabelShow = "Visible";
    }
    public void Button(object sender, RoutedEventArgs e) {
      Timer = new DispatcherTimer {
        Interval = new TimeSpan(0, 0, 1),
        IsEnabled = true
      };
      // use inventorybutton.Button()
      InventoryFolder.InventoryControl inv = new InventoryFolder.InventoryControl();
      inv.Button(null, null);
    }
  }
}
