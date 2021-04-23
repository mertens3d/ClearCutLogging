using System.Windows.Controls;
using System.Windows.Input;

namespace ClearCut.Main.Views
{
  /// <summary>
  /// Interaction logic for ResultsList.xaml
  /// </summary>
  public partial class ResultsList : UserControl
  {
    public ResultsList()
    {
      InitializeComponent();
    }

    private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      string tagValue = ((TextBlock)sender).Tag.ToString();
      if (!string.IsNullOrEmpty(tagValue))
      {
        var args = @"" + tagValue + "";
        var program = @"C:\Program Files\Notepad++\notepad++.exe";
        System.Diagnostics.Process.Start(program, args);
      }
    }
  }
}