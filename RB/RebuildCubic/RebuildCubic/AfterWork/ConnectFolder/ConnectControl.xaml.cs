using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using JW.UHF;
using System.Collections.Generic;
using RebuildCubic.DataTableFolder;
using RebuildCubic.TriggerFolder;
using RebuildCubic.InventoryFolder;

namespace RebuildCubic.ConnectFolder {
  /// <summary>
  /// ConnectControl.xaml 的互動邏輯
  /// </summary>
  public partial class ConnectControl : UserControl {
    // declare variable and class
    public static JWReader JwReader { get; set; }
    // tag list
    public static List<Tag> Taglist { get; set; } = new List<Tag>();
    // reader ip
    public static string IP { get; set; } = "192.168.1.100";
    // reader port
    public static int Port { get; set; } = 9761;

    public ConnectControl() {
      InitializeComponent();
    }

    //  private void ConnectButton_Click(object sender, RoutedEventArgs e) {
    //    // if ConnectButton.Content is Connect
    //    if ((string)ConnectButton.Content == "Connect") {
    //      // instance JWReadet with (ip and 9761)
    //      JwReader = new JWReader(IP, Port);
    //      // check RFID_Open return Result.OK
    //      if (JwReader.RFID_Open() == Result.OK) {
    //        // stop reader's inventory
    //        JwReader.RFID_Stop_Inventory();
    //        // set ConnectButton to another place and content to Disconnect
    //        ConnectOn();
    //        // set triggerbutton IsEnable and Visibility
    //        TriggerButtonSwitch();

    //        //  DataTableControl.ListNameBoxVisibility = Visibility.Visible;

    //        JwReader.TagsReported += JwReader_TagsReported;
    //      }
    //      // ConnectButton.Content isn't Connect
    //    } else {
    //      // set jwReader to null
    //      JwReader = null;
    //      // set ConnectButton to original place and content to Connect
    //      ConnectOff();
    //      // set triggerbutton IsEnable and Visibility
    //      TriggerButtonSwitch();
    //      //InventoryControl.InventoryButtonSwitch = false;
    //      //DataTableControl.ListNameBoxVisibility = Visibility.Hidden;
    //    }
    //  }

    //  private void TriggerButtonSwitch() {
    //    if (TriggerControl.TriggerButtonIsEnabled == false) {
    //      TriggerControl.TriggerButtonIsEnabled = true;
    //      TriggerControl.TriggerButtonVisibility = Visibility.Visible;
    //    } else {
    //      TriggerControl.TriggerButtonIsEnabled = false;
    //      TriggerControl.TriggerButtonVisibility = Visibility.Hidden;
    //    }
    //  }

    //  private void ConnectOn() {
    //    // set ConnectButton.Content to Disconnect
    //    ConnectButton.Content = "Disconnect";
    //    // set ConnectButton.HorizontalAlignment to Left
    //    ConnectButton.HorizontalAlignment = HorizontalAlignment.Left;
    //    // set ConnectButton.VerticalAlignment to Top
    //    ConnectButton.VerticalAlignment = VerticalAlignment.Top;
    //    // set ConnectButton.Margin = "670,30,0,0"
    //    ConnectButton.Margin = new Thickness(670, 30, 0, 0);
    //  }

    //  private void ConnectOff() {
    //    // set Content to Connect
    //    ConnectButton.Content = "Connect";
    //    // set HorizontalAlignment to Center
    //    ConnectButton.HorizontalAlignment = HorizontalAlignment.Center;
    //    // set VerticalAlignment to Center
    //    ConnectButton.VerticalAlignment = VerticalAlignment.Center;
    //    // set Margin = "0,0,0,0"
    //    ConnectButton.Margin = new Thickness(0, 0, 0, 0);

    //  }

    //  private void JwReader_TagsReported(JWReader reader, TagsEventArgs args) {
    //    Taglist.Clear();
    //    Dispatcher.Invoke(() => {
    //      if (!Taglist.Contains(args.tag)) {
    //        Taglist.Add(args.tag);
    //      }
    //    });
    //  }
  }
}
