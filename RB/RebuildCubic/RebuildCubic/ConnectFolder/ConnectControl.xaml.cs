using System.Windows;
using System.Windows.Controls;
using JW.UHF;
namespace RebuildCubic.ConnectFolder {
  /// <summary>
  /// ConnectControl.xaml 的互動邏輯
  /// </summary>
  public partial class ConnectControl : UserControl {
    // declare variable and class
    public JWReader jwReader { get; set; }
    public static string CurrentState { get; set; }
    public string IP { get; set; } = "192.168.1.100";

    public ConnectControl() {
      InitializeComponent();
    }

    private void Button(object sender, RoutedEventArgs e) {
      if ((string)ConnectButton.Content == "Connect") {
        // instance JWReadet with (ip and 9761)
        jwReader = new JWReader(IP, 9761);
        //if (jwReader.RFID_Open() == Result.OK) {
          // set CurrentState to Connect
          CurrentState = "Is Connected";
          // set ConnectButton.Content to Disconnect
          ConnectButton.Content = "Disconnect";
        //}
      } else {
        // set CurrentState to Disconnect
        CurrentState = "Not Connected";
        jwReader = null;
        // set ConnectButton.Content to Connect
        ConnectButton.Content = "Connect";
        //statecontrol.Refresh();
      }
    }
  }
}
