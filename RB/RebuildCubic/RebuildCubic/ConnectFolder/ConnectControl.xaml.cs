using System;
using System.Windows;
using System.Windows.Controls;
using RebuildCubic.JWReaderFolder;
namespace RebuildCubic.ConnectFolder {
  /// <summary>
  /// ConnectControl.xaml 的互動邏輯
  /// </summary>
  public partial class ConnectControl : UserControl {
    public ConnectControl() {
      InitializeComponent();
    }
    public JWReader jwReader { get; set; }
    public string ip { get; set; } = "192.168.1.100";

    private void Button(object sender, RoutedEventArgs e) {
      if ((string)ConnectButton.Content == "Connect") {
        jwReader = new JWReader(ip, 9761);
        if (jwReader.RFID_Open() == Result.OK) {
          ConnectButton.Content = "Disconnect";
        }
      } else {
        jwReader = null;
        ConnectButton.Content = "Connect";
      }
    }
  }
}
