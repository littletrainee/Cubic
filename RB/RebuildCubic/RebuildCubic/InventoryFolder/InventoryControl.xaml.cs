using System.Threading;
using System.Windows;
using System.Windows.Controls;
using JW.UHF;
using System.Data;
using System;
namespace RebuildCubic.InventoryFolder {
  /// <summary>
  /// InventoryControl.xaml 的互動邏輯
  /// </summary>
  public partial class InventoryControl : UserControl {


    private JWReader jwReader { get; set; }

    public InventoryControl() {
      InitializeComponent();
    }
  }
}
