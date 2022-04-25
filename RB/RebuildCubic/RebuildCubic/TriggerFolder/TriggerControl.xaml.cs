using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using RebuildCubic.InventoryFolder;

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
    public void TriggerButton_Click(object sender, RoutedEventArgs e) {
      Timer = new DispatcherTimer {
        Interval = new TimeSpan(0, 0, 1),
        IsEnabled = true
      };
      InventoryControl inv = new InventoryControl();
      // use inventorybutton.Button()
      inv.InventoryButton_Click(null, null);
    }
  }
}
