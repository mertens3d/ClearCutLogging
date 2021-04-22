using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

    }

    private void Hyperlink_Click(object sender, RoutedEventArgs e)
    {
      string tagValue = ((Hyperlink)sender).Tag.ToString();
      if (!string.IsNullOrEmpty(tagValue))
      {

        //var args = @"/k " + tagValue + "";
        var args = @"" + tagValue + "";
        var program = @"C:\Program Files\Notepad++\notepad++.exe";
        System.Diagnostics.Process.Start(program, args);
      }
    }
  }
}
