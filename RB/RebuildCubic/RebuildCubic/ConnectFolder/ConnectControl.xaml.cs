using System;
using System.Windows;
using System.Windows.Controls;
namespace RebuildCubic.ConnectFolder {
  /// <summary>
  /// ConnectControl.xaml 的互動邏輯
  /// </summary>
  public partial class ConnectControl : UserControl {
    public ConnectControl() {
      InitializeComponent();
    }
    // true = connect false = not connect
    private bool Isconnect { get; set; } = false;

    private void Button(object sender, RoutedEventArgs e) {
      // Change ConnectButton.Content from Enum_Connect
      ConnectButton.Content =
        // first word is Upper
        ((EnumConnectWord)Convert.ToInt32(!Isconnect)).ToString().
        Substring(0, 1).ToUpper() +
        // other word is Lower
        ((EnumConnectWord)Convert.ToInt32(!Isconnect)).ToString().
        Substring(1).ToLower();
      // switch Isconnect bool
      Isconnect = !Isconnect;
    }
  }
}
