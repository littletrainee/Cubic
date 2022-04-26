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
          mainwindow.Close();
          ConnectErrorClass.ShowPopWindow();
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
      // enable triggerbutton
      TriggerButton.IsEnabled = true;
      // enable inventorybutton
      InventoryButton.IsEnabled = true;
      // enable clearbutton
      ClearButton.IsEnabled = true;
      // enable setbutton
      SetButton.IsEnabled = true;
      // show tagnamelist
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
      // disable triggerbutton
      TriggerButton.IsEnabled = false;
      // disable inventorybutton
      InventoryButton.IsEnabled = false;
      // disable clearbutton 
      ClearButton.IsEnabled = false;
      // disable setbutton
      SetButton.IsEnabled = false;
      // hide tagnamelist
      TagNameList.Visibility = Visibility.Hidden;
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

    public int Delaytime { get; set; } = 300;
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
      InventoryButton_Click(null, null);
      //Task.Run(() => {
      //  JwReader.RFID_Start_Inventory();
      //});
      //Task.Run(() => {
      //  Task.Delay(delaytime).Wait();
      //  JwReader.RFID_Stop_Inventory();
      //});
    }

    private void InventoryButton_Click(object sender, RoutedEventArgs e) {
      if ((string)InventoryButton.Content == "Start Inventory") {
        InventoryButton.Content = "Stop Inventory";
        Thread thread = new Thread(inventory_thread) {
          IsBackground = true
        };
        thread.Start();
      } else {

        JwReader.RFID_Stop_Inventory();
        InventoryButton.Content = "Start Inventory";
      }
    }

    public int Start_time { get; set; }
    public int Global_tag_counts { get; set; }
    public bool Inventory_update_start { get; set; }
    public int End_time { get; set; }

    private void inventory_thread() {
      Dispatcher.Invoke(() => {
        TagNameList.Items.Clear();
        //dataGridView1.Refresh();
      });
      Start_time = Environment.TickCount;
      Global_tag_counts = 0;
      Inventory_update_start = true;
      JwReader.RFID_Start_Inventory();
      End_time = Environment.TickCount;
      Inventory_update_start = false;
      Dispatcher.Invoke(() => {
        InventoryButton.Content = "Start Inventory";
      });
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e) {
      if (!Inventory_update_start) {
        Global_tag_counts = 0;
      }
      TagNameList.Items.Clear();
    }

    private void SetButton_Click(object sender, RoutedEventArgs e) {
      SetButton.IsEnabled = false;
      RfidSetting rfidSetting = new RfidSetting();
      rfidSetting.AntennaPort_List = new List<AntennaPort>();
      AntennaPort antennaPort = new AntennaPort();
      //if (checkBox_ant0.Checked) {
      //  antennaPort.AntennaIndex = 0;
      //  antennaPort.Power = int.Parse(textBox_ant0.Text);
      //  rfidSetting.AntennaPort_List.Add(antennaPort);
      //}
      //if (checkBox_ant1.Checked) {
      //  antennaPort.AntennaIndex = 1;
      //  antennaPort.Power = int.Parse(textBox_ant1.Text);
      //  rfidSetting.AntennaPort_List.Add(antennaPort);
      //}
      //if (checkBox_ant2.Checked) {
      //  antennaPort.AntennaIndex = 2;
      //  antennaPort.Power = int.Parse(textBox_ant2.Text);
      //  rfidSetting.AntennaPort_List.Add(antennaPort);
      //}
      //if (checkBox_ant3.Checked) {
      //  antennaPort.AntennaIndex = 3;
      //  antennaPort.Power = int.Parse(textBox_ant3.Text);
      //  rfidSetting.AntennaPort_List.Add(antennaPort);
      //}
      //GPIOConfig gPIOConfig = new GPIOConfig();
      //if (comboBox_gpo0.SelectedIndex == 0) {
      //  gPIOConfig.GPO0_VALUE = GPOTriggerValue.Low;
      //} else {
      //  gPIOConfig.GPO0_VALUE = GPOTriggerValue.Hign;
      //}
      //if (comboBox_gpo1.SelectedIndex == 0) {
      //  gPIOConfig.GPO1_VALUE = GPOTriggerValue.Low;
      //} else {
      //  gPIOConfig.GPO1_VALUE = GPOTriggerValue.Hign;
      //}
      //if (comboBox_gpi0.SelectedIndex == 0) {
      //  gPIOConfig.GPI0_VALUE = GPITriggerValue.None;
      //} else if (comboBox_gpi0.SelectedIndex == 1) {
      //  gPIOConfig.GPI0_VALUE = GPITriggerValue.Inventory;
      //}
      //if (comboBox_gpi1.SelectedIndex == 0) {
      //  gPIOConfig.GPI1_VALUE = GPITriggerValue.None;
      //} else if (comboBox_gpi1.SelectedIndex == 1) {
      //  gPIOConfig.GPI1_VALUE = GPITriggerValue.Inventory;
      //}
      //if (jwReader.RFID_Set_GPIO(gPIOConfig) == Result.OK) {
      //  Console.WriteLine("GPIO Set Success");
      //  rfidSetting.Inventory_Time = int.Parse(textBox_inv_time.Text);
      //  rfidSetting.RSSI_Filter = new RSSIFilter();
      //  rfidSetting.RSSI_Filter.Enable = false;
      //  if (comboBox_speed.SelectedIndex == 0) {
      //    rfidSetting.Speed_Mode = SpeedMode.SPEED_FASTEST;
      //  } else if (comboBox_speed.SelectedIndex == 1) {
      //    rfidSetting.Speed_Mode = SpeedMode.SPEED_NORMAL;
      //  } else if (comboBox_speed.SelectedIndex == 2) {
      //    rfidSetting.Speed_Mode = SpeedMode.SPEED_POWERSAVE;
      //  } else if (comboBox_speed.SelectedIndex == 3) {
      //    rfidSetting.Speed_Mode = SpeedMode.SPEED_FULL_POWER;
      //  }
      //  if (comboBox_region.SelectedIndex == 0) {
      //    rfidSetting.Region_List = RegionList.FCC;
      //  } else if (comboBox_region.SelectedIndex == 1) {
      //    rfidSetting.Region_List = RegionList.CCC;
      //  } else if (comboBox_region.SelectedIndex == 2) {
      //    rfidSetting.Region_List = RegionList.NCC;
      //  } else if (comboBox_region.SelectedIndex == 3) {
      //    rfidSetting.Region_List = RegionList.OPTIMAL;
      //  }
        rfidSetting.Tag_Group = new TagGroup();
        rfidSetting.Tag_Group.SessionTarget = SessionTarget.A;
        rfidSetting.Tag_Group.SearchMode = SearchMode.DUAL_TARGET;
        rfidSetting.Tag_Group.Session = Session.S0;
        if (JwReader.RFID_Set_Config(rfidSetting) == Result.OK) {
          Console.WriteLine("RFID Config Set Success");
          SetButton.IsEnabled = true;
        } else {
          Console.WriteLine("RFID Config Set Failure");
          SetButton.IsEnabled = true;
        }
      //} else {
      //  Console.WriteLine("GPIO Set Failure");
      //  button_set.Enabled = true;
      //}
    }
  }
}
