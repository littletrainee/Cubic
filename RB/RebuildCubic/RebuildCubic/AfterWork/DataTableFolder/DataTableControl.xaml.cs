using System.Windows;
using System.Windows.Controls;
namespace RebuildCubic.DataTableFolder {
  /// <summary>
  /// DataTableControl.xaml 的互動邏輯
  /// </summary>
  public partial class DataTableControl : UserControl {
    public static Visibility ListNameBoxVisibility { get; set; } = Visibility.Hidden;
    public DataTableControl() {
      InitializeComponent();
    }
  }
}
