using System.Windows.Controls;

namespace RebuildCubic.InventoryFolder {
  /// <summary>
  /// InventoryControl.xaml 的互動邏輯
  /// </summary>
  public partial class InventoryControl : UserControl {
    // Inventory Button Switch
    public static bool InventoryButtonSwitch { get; set; }

    public InventoryControl() {
      InitializeComponent();
    }
  }
}
