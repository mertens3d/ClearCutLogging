using ClearCut.Main.ViewModels;
using ClearCut.Support.Abstractions;
using ClearCut.Support.Settings;
using ClearCut.Support.Witness;
using ClearCut.Support.Witness.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace ClearCut
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  ///
  public partial class MainWindow : Window
  {
    private TimeSpan _time;

    private DispatcherTimer _timer;

    public MainWindow(ILogger logger)
    {
      Logger = logger;

      InitializeComponent();

      SettingsManager = new SettingsManager();
      InitWitness();
      InitCountdown();
    }

    private ILogger Logger { get; }
    private ResultsViewModel ResultsViewModel { get; set; }
    private SettingsManager SettingsManager { get; }
    private WitnessManager WitnessManager { get; set; }

    private void InitCountdown()
    {
      _time = TimeSpan.FromSeconds(10);

      _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.SystemIdle, delegate
        {
          this.Title = Constants.Ui.TitlePrefix + _time.Seconds;
          if (_time == TimeSpan.Zero)
          {
            WitnessManager.TriggerDataChanged();
            _time = TimeSpan.FromSeconds(10);
          }
          _time = _time.Add(TimeSpan.FromSeconds(-1));
        }, Application.Current.Dispatcher);

      _timer.Start();
    }

    private void InitWitness()
    {
      ResultsViewModel = new ResultsViewModel();
      WitnessManager = new WitnessManager(SettingsManager.GetEnvironmentSettings(), Logger);
      WitnessManager.DataChanged += UpdateDataContext;
      WitnessManager.TriggerDataChanged();
    }

    private void SetDataContext(List<ILastFile> lastFiles)
    {
      ResultsViewModel.LastFiles = lastFiles;
      this.Dispatcher.Invoke(() =>
      {
        ResultsList.idListView.ItemsSource = ResultsViewModel.LastFiles;
      });
    }

    private void UpdateDataContext(object sender, WitnessEventArgs e)
    {
      if (e?.LastFiles != null && e.LastFiles.Any())
      {
        SetDataContext(e.LastFiles);
      }
    }
  }
}