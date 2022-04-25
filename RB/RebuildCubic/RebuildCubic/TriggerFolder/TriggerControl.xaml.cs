using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using JW.UHF;
using RebuildCubic.ConnectFolder;

namespace RebuildCubic.TriggerFolder {
  /// <summary>
  /// TriggerControl.xaml 的互動邏輯
  /// </summary>
  public partial class TriggerControl : UserControl {

    public DispatcherTimer Timer { get; set; }
    public static string CurrentStateLabelShow { get; set; }
    public TriggerControl() {
      InitializeComponent();
    }
    public void TriggerButton_Click(object sender, RoutedEventArgs e) {
      Task.Run(() => {
        ConnectControl.JwReader.RFID_Start_Inventory();
        foreach (Tag tag in ConnectControl.Taglist) {
          Console.WriteLine(tag.EPC);
        }
      });

      Task.Run(() => {
        Task.Delay(300).Wait();
        ConnectControl.JwReader.RFID_Stop_Inventory();
      });
    }
  }
}
