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
    //private static ILog Logger => LogManager.GetLogger(typeof(MainWindow));

    public MainWindow(ILogger logger)
    {
      //ILoggerRepository repository = log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());
      //var fileInfo = new FileInfo(@"log4net.config");
      //log4net.Config.XmlConfigurator.Configure(repository, fileInfo);
      Logger = logger;

      //BasicConfigurator.Configure();

      InitializeComponent();

      var settingsManager = new SettingsManager();
      InitWitness();
      InitCountdown();
    }

    public ResultsViewModel ResultsViewModel { get; private set; }
    private ILogger Logger { get; }
    private WitnessManager WitnessManager { get; set; }

    private DispatcherTimer _timer;
    private TimeSpan _time;

    private void InitCountdown()
    {
      _time = TimeSpan.FromSeconds(10);

      _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.SystemIdle, delegate
        {
          this.Title = "Clear Cut Log Watcher : " + _time.Seconds;
          if (_time == TimeSpan.Zero)
          {
            WitnessManager.TriggerDataChanged();
            _time = TimeSpan.FromSeconds(10);
            //_timer.Stop();
          }
          _time = _time.Add(TimeSpan.FromSeconds(-1));
        }, Application.Current.Dispatcher);

      _timer.Start();
    }

    private void InitWitness()
    {
      ResultsViewModel = new ResultsViewModel();
      var settingsManager = new SettingsManager();
       WitnessManager = new WitnessManager(settingsManager.GetEnvironmentSettings(), Logger);
      WitnessManager.DataChanged += UpdateDataContext;
      WitnessManager.TriggerDataChanged();
    }

    private void SetDataContext(List<ILastFile> lastFiles)
    {
      ResultsViewModel.LastFiles = lastFiles;
      this.Dispatcher.Invoke(() =>
      {
        ResultsList.idListView.ItemsSource = ResultsViewModel.LastFiles;
        //DataContext = ResultsViewModel;
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