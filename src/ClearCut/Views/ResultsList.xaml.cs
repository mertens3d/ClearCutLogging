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
        var notepadPPArgs = @"" + tagValue + "";
        var notepadPPProgram = @"C:\Program Files\Notepad++\notepad++.exe";
        var bareTailArgs = @"" + tagValue + "";
        var bareTailProgram = @"C:\BareTail\baretail.exe";
        //System.Diagnostics.Process.Start(notepadPPProgram, notepadPPArgs);
        System.Diagnostics.Process.Start(bareTailProgram, bareTailArgs);
      }
    }
  }
}