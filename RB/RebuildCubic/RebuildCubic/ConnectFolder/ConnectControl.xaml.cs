using System.Windows;

using System.Windows.Controls;
using System;
using JW.UHF;
using System.Collections.Generic;

namespace RebuildCubic.ConnectFolder {
  /// <summary>
  /// ConnectControl.xaml 的互動邏輯
  /// </summary>
  public partial class ConnectControl : UserControl {
    // declare variable and class
    public static JWReader JwReader { get; set; }
    // Current State 
    public static string CurrentState { get; set; }
    // Trigger Button Switch
    public static bool TriggerButtonSwitch { get; set; }
    // Inventory Button Switch
    public static bool InventoryButtonSwitch { get; set; }
    // tag list
    public static List<Tag> Taglist { get; set; } = new List<Tag>();
    // state label visibility
    public static Visibility StateLabelVisibility { get; set; } 
    
    // reader ip
    public static string IP { get; set; } = "192.168.1.100";
    // reader port
    public static int Port { get; set; } = 9761;

    public ConnectControl() {
      InitializeComponent();
    }

    private void ConnectButton_Click(object sender, RoutedEventArgs e) {
      // if ConnectButton.Content is Connect
      if ((string)ConnectButton.Content == "Connect") {
        // instance JWReadet with (ip and 9761)
        JwReader = new JWReader(IP, Port);
        //Thread thread = new Thread(() => {
        if (JwReader.RFID_Open() == Result.OK) {
          JwReader.RFID_Stop_Inventory();
          // set ConnectButton.Content to Disconnect
          ConnectButton.Content = "Disconnect";
          // set CurrentState to Connect
          CurrentState = "Is Connected";
          // set ConnectButton.Content to Disconnect
          ConnectButton.Content = "Disconnect";
          TriggerButtonSwitch = true;
          InventoryButtonSwitch = true;
          StateLabelVisibility = Visibility.Hidden;
          DataTableFolder.DataTableControl.ListNameBoxVisibility = Visibility.Visible;

          JwReader.TagsReported += JwReader_TagsReported;
        }
        // ConnectButton.Content isn't Connect
      } else {
        // set CurrentState to Disconnect
        CurrentState = "Not Connected";
        // set jwReader to null
        JwReader = null;
        // set ConnectButton.Content to Connect
        ConnectButton.Content = "Connect";
        TriggerButtonSwitch = false;
        InventoryButtonSwitch = false;
        StateLabelVisibility = Visibility.Visible;
        DataTableFolder.DataTableControl.ListNameBoxVisibility = Visibility.Hidden;
      }
    }

    private void JwReader_TagsReported(JWReader reader, TagsEventArgs args) {
      Taglist.Clear();
      Dispatcher.Invoke(() => {
        if (!Taglist.Contains(args.tag)) {
          Taglist.Add(args.tag);
        }
      });
    }
  }
}
