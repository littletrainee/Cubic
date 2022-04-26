using JW.UHF;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Threading;

namespace RebuildCubic {
  /// <summary>
  /// MainWindow.xaml 的互動邏輯
  /// </summary>
  public partial class MainWindow : Window {
    // declare variable and class
    public static JWReader JwReader { get; set; }
    // tag list
    public static List<Tag> Taglist { get; set; } = new List<Tag>();
    // reader ip
    public static string IP { get; set; } = "192.168.1.100";
    // reader port
    public static int Port { get; set; } = 9761;

    public MainWindow() {
      InitializeComponent();
    }

    private void ConnectButton_Click(object sender, RoutedEventArgs e) {
      // if ConnectButton.Content is Connect
      if ((string)ConnectButton.Content == "Connect") {
        // instance JWReadet with (ip and 9761)
        JwReader = new JWReader(IP, Port);
        // check RFID_Open return Result.OK
        if (JwReader.RFID_Open() == Result.OK) {
          // stop reader's inventory
          JwReader.RFID_Stop_Inventory();
          // set ConnectButton to another place and content to Disconnect
          ConnectOn();
          // set triggerbutton IsEnable and Visibility
          JwReader.TagsReported += JwReader_TagsReported;
        } else {
          MessageBox.Show("Not Connected!");
        }
        // ConnectButton.Content isn't Connect
      } else {
        // set jwReader to null
        JwReader = null;
        // set ConnectButton to original place and content to Connect
        ConnectOff();
      }
    }

    private void ConnectOn() {
      // set ConnectButton.Content to Disconnect
      ConnectButton.Content = "Disconnect";
      // set ConnectButton.HorizontalAlignment to Left
      ConnectButton.HorizontalAlignment = HorizontalAlignment.Left;
      // set ConnectButton.VerticalAlignment to Top
      ConnectButton.VerticalAlignment = VerticalAlignment.Top;
      // set ConnectButton.Margin = "670,30,0,0"
      ConnectButton.Margin = new Thickness(670, 30, 0, 0);
      TriggerButton.IsEnabled = true;
      //InventoryButton.IsEnabled = true;
      //ClearButton.IsEnabled = true;
      TagNameList.Visibility = Visibility.Visible;


    }

    private void ConnectOff() {
      // set Content to Connect
      ConnectButton.Content = "Connect";
      // set HorizontalAlignment to Center
      ConnectButton.HorizontalAlignment = HorizontalAlignment.Center;
      // set VerticalAlignment to Center
      ConnectButton.VerticalAlignment = VerticalAlignment.Center;
      // set Margin = "0,0,0,0"
      ConnectButton.Margin = new Thickness(0, 0, 0, 0);

    }

    private void JwReader_TagsReported(JWReader reader, TagsEventArgs args) {
      Dispatcher.Invoke(() => {
        TagNameList.Items.Add(args.tag.EPC);
        Console.WriteLine(args.tag.DATA);
        Console.WriteLine(args.tag.EPC);
        Console.WriteLine(args.tag.PORT);
        Console.WriteLine(args.tag.RSSI);
      });
    }

    public int delaytime { get; set; } = 300;
    /// <summary>
    /// click to run inventorybutton_click
    /// it is inclue inventory and trigger ability
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void TriggerButton_Click(object sender, RoutedEventArgs e) {
      // timer start 
      //
      // timer end
      // try to clear TagNameList
      TagNameList.Items.Clear();
      Console.WriteLine("trigger button clicked");
      //InventoryButton_Click(null, null);
      Task.Run(() => {
        JwReader.RFID_Start_Inventory();
      });
      Task.Run(() => {
        Task.Delay(delaytime).Wait();
        JwReader.RFID_Stop_Inventory();
      });
    }

    private void InventoryButton_Click(object sender, RoutedEventArgs e) {
      if ((string)InventoryButton.Content == "Start Inventory") {
        InventoryButton.Content = "Stop Inventory";
        Thread thread = new Thread(inventory_thread);
        thread.IsBackground = true;
        thread.Start();
      } else {

        JwReader.RFID_Stop_Inventory();
        InventoryButton.Content = "Start Inventory";
      }
    }

    public int start_time { get; set; }
    public int global_tag_counts { get; set; }
    public bool inventory_update_start = true;
    public int end_time { get; set; }

    private void inventory_thread() {
      Dispatcher.Invoke(() => {
        TagNameList.Items.Clear();
        //dataGridView1.Refresh();
      });
      start_time = Environment.TickCount;
      global_tag_counts = 0;
      inventory_update_start = true;
      JwReader.RFID_Start_Inventory();
      end_time = Environment.TickCount;
      inventory_update_start = false;
      Dispatcher.Invoke(() => {
        InventoryButton.Content = "Start Inventory";
      });
    }

    
    private void ClearButton_Click(object sender, RoutedEventArgs e) {
      if (!inventory_update_start) {
        global_tag_counts = 0;
      }
      TagNameList.Items.Clear();
    }
  }
}
