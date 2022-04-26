using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RebuildCubic {
  public class ConnectErrorClass {
    private static string Message { get; set; } = "Connect to Reader Failed\n " +
         "Please Check the VPN is ON and \n the Remote Desktop is Normal";

    private static string MessageTitle { get; set; } = "Connect Error";

    private static MessageBoxButton MessageBoxButton { get; set; } = 
      MessageBoxButton.OK;

    private static MessageBoxImage MessageBoxImage { get; set; } =
      MessageBoxImage.Error;

    public static void ShowPopWindow() {
      MessageBox.Show(Message, MessageTitle, MessageBoxButton, MessageBoxImage);
    }

  }
}
