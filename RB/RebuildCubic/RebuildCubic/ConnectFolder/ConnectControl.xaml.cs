using System.Threading;
using System.Windows;
using System.Windows.Controls;
using JW.UHF;
namespace RebuildCubic.ConnectFolder {
  /// <summary>
  /// ConnectControl.xaml 的互動邏輯
  /// </summary>
  public partial class ConnectControl : UserControl {
    // declare variable and class
    public JWReader JwReader { get; set; }
    //public static string CurrentState { get; set; }
    public static bool InventoryButtonSwitch { get; set; }
    public static bool TriggerButtonSwitch { get; set; }
    public static bool ClearButtonSwitch { get; set; }
    public static bool SetButtonSwitch { get; set; }
    public string IP { get; set; } = "192.168.1.100";

    public ConnectControl() {
      InitializeComponent();
    }

    private void Button(object sender, RoutedEventArgs e) {
      // if ConnectButton.Content is Connect
      if ((string)ConnectButton.Content == "Connect") {
        // instance JWReadet with (ip and 9761)
        //CurrentState = "Try Connect to " + IP;
        JwReader = new JWReader(IP, 9761);
        //Thread thread = new Thread(() => {
        //  Thread.Sleep(2000);
          if (JwReader.RFID_Open() == Result.OK) {
            // set ConnectButton.Content to Disconnect
            ConnectButton.Content = "Disconnect";
            // set CurrentState to Connect
            //CurrentState = "Is Connected";
            // set ConnectButton.Content to Disconnect
            ConnectButton.Content = "Disconnect";
            InventoryButtonSwitch = true;
            TriggerButtonSwitch = true;
            ClearButtonSwitch = true;
            SetButtonSwitch = true;
          }
        // ConnectButton.Content isn't Connect
        } else {
        // set CurrentState to Disconnect
        //CurrentState = "Not Connected";
        // set jwReader to null
        JwReader = null;
        // set ConnectButton.Content to Connect
        ConnectButton.Content = "Connect";
        InventoryButtonSwitch = false;
        TriggerButtonSwitch = false;
        ClearButtonSwitch = false;
        SetButtonSwitch = false;
      }
    }
  }
}
