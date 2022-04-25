using System.Threading;
using System.Windows;
using System.Windows.Controls;
using JW.UHF;
using System.Data;
using System;
namespace RebuildCubic.InventoryFolder {
  /// <summary>
  /// InventoryControl.xaml 的互動邏輯
  /// </summary>
  public partial class InventoryControl : UserControl {


    private JWReader jwReader { get; set; }

    public InventoryControl() {
      InitializeComponent();
      IP = ConnectFolder.ConnectControl.IP;
      Port = ConnectFolder.ConnectControl.Port;
    }

    public void InventoryButton_Click(object sender, RoutedEventArgs e) {
      if ((string)InventoryButton.Content == "Inventory Start") {
        InventoryButton.Content = "Inventory Stop";
        Thread thread = new Thread(inventory_thread) {
          IsBackground = true
        };
        thread.Start();
      } else {
        InventoryButton.Content = "Inventory Start";
      }
    }

    public delegate void myupdate();
    private DataTable global_data_table { get; set; } = new DataTable();
    public int start_time { get; set; }
    public int end_time { get; set; }
    public int global_tag_counts { get; set; }
    public bool inventory_update_start { get; set; }
    public string IP { get; set; }
    public int Port { get; set; }

    private void inventory_thread() {
      jwReader = new JWReader(IP, Port);
      myupdate method = delegate {
        global_data_table.Clear();
        dataGridView1.ItemsSource = null;
      };
      Dispatcher.Invoke(method);
      start_time = Environment.TickCount;
      global_tag_counts = 0;
      inventory_update_start = true;
      jwReader.RFID_Start_Inventory();
      end_time = Environment.TickCount;
      inventory_update_start = false;
      myupdate method2 = delegate {
        InventoryButton.Content = "Inventory Start";
      };
      Dispatcher.Invoke(method2);
    }

  }
}
