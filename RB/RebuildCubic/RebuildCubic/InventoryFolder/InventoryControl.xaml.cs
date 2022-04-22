using System.Threading;
using System.Windows;
using System.Windows.Controls;
using JW.UHF;
namespace RebuildCubic.InventoryFolder {
  /// <summary>
  /// InventoryControl.xaml 的互動邏輯
  /// </summary>
  public partial class InventoryControl : UserControl {

    private JWReader jwReader { get; set; }

    public InventoryControl() {
      InitializeComponent();
    }

    public void Button(object sender, RoutedEventArgs e) {
      if ((string)InventoryButton.Content == "Inventory Start") {
        InventoryButton.Content = "Inventory Stop";
        Thread thread = new Thread(inventory_thread);
        thread.IsBackground = true;
        thread.Start();
      } else {
        InventoryButton.Content = "Inventory Start";
      }
    }
    // UHF_Demo_EPC.FormMain

    public delegate void myupdate();
    private void inventory_thread() {
      //myupdate method = delegate {
      //  global_data_table.Clear();
      //  dataGridView1.Refresh();
      //};
      //Invoke(method);
      //start_time = Environment.TickCount;
      //global_tag_counts = 0;
      //inventory_update_start = true;
      jwReader.RFID_Start_Inventory();
      //end_time = Environment.TickCount;
      //inventory_update_start = false;
      //myupdate method2 = delegate {
      //  button_inventory.Text = "inv_start";
      //};
      //Invoke(method2);
    }

  }
}
