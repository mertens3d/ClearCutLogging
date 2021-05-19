using ClearCut.Main.ViewModels;
using ClearCut.Support.Abstractions;
using ClearCut.Support.Witness.Models;
using ClearCut.Support.Witness.Watchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ClearCut.Main.Views
{
  /// <summary>
  /// Interaction logic for ResultsList.xaml
  /// </summary>
  public partial class ResultsList : UserControl
  {
    private TimeSpan _time;

    //private SiteWatcher SiteWatcher { get; set; }
    private DispatcherTimer _timer;

    private ResultsViewModel ResultsViewModel { get; set; }
    public SiteWatcher SiteWatcher { get; private set; }

    public ResultsList()
    {
      InitializeComponent();
    }

    public void InitWithContext(SiteWatcher siteWatcher)
    {
      SiteWatcher = siteWatcher;
      DataContext = siteWatcher;
      InitWitness();
      InitCountdown();
    }

    private void InitWitness()
    {
      //  var tabControl = tabControl

      ResultsViewModel = new ResultsViewModel();
      if (DataContext != null)
      {
        SiteWatcher.DataChanged += UpdateDataContext;
        SiteWatcher.TriggerDataChanged();
      }
    }

    private void SetDataContext(List<ILastFile> lastFiles)
    {
      ResultsViewModel.LastFiles = lastFiles;
      this.Dispatcher.Invoke(() =>
      {
        idListView.ItemsSource = ResultsViewModel.LastFiles;
      });
    }

    private void UpdateDataContext(object sender, WitnessEventArgs e)
    {
      if (e?.LastFiles != null && e.LastFiles.Any())
      {
        SetDataContext(e.LastFiles);
      }
    }

    private void InitCountdown()
    {
      _time = TimeSpan.FromSeconds(10);

      _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.SystemIdle, delegate
      {
        this.countDown.Content = ClearCut.Support.Settings.Constants.Ui.TitlePrefix + _time.Seconds;
        if (_time == TimeSpan.Zero)
        {
          if (SiteWatcher != null)
          {
            SiteWatcher.TriggerDataChanged();
          }

          _time = TimeSpan.FromSeconds(10);
        }
        _time = _time.Add(TimeSpan.FromSeconds(-1));
      }, Application.Current.Dispatcher);

      _timer.Start();
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