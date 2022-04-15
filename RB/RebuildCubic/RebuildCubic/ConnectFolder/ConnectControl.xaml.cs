using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
namespace RebuildCubic.ConnectFolder {
  /// <summary>
  /// ConnectControl.xaml 的互動邏輯
  /// </summary>
  public partial class ConnectControl : UserControl {
    struct NetClient {
      public Socket NSocket { get; set; }
      public short KeepAliveTime { get; set; }
      public short KeepAliveInterval { get; set; }
      public short ConnectWaitTime { get; set; }
      public long StartTime { get; set; }
      public string Ip { get; set; }
      public int Port { get; set; }
    }
    public ConnectControl() {
      InitializeComponent();
    }
    // true = connect false = not connect
    private bool Isconnect { get; set; } = false;

    private bool OP(object sender, RoutedEventArgs e) {
      NetClient nc = new NetClient();
      nc.Ip = "192.168.1.100";
      nc.Port = 9761;

      nc.NSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      IPAddress ipaddress = new IPAddress(new byte[]{ 192,168,1,100 });
      EndPoint poin = new IPEndPoint(ipaddress, 9761);
      nc.KeepAliveTime = 1000;
      nc.KeepAliveInterval = 1000;
      nc.ConnectWaitTime = 20000;
      nc.StartTime = DateTime.Now.Ticks;
      bool result = false;

      IClient client;

      return result;
    }

 
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
