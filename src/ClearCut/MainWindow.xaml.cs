using ClearCut.Main.ViewModels;
using ClearCut.Support.Abstractions;
using ClearCut.Support.Settings;
using ClearCut.Support.Witness;
using ClearCut.Support.Witness.Models;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

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
    }

    public ResultsViewModel ResultsViewModel { get; private set; }
    private ILogger Logger { get; }

    private void InitWitness()
    {
      ResultsViewModel = new ResultsViewModel();
      var settingsManager = new SettingsManager();
      var manager = new WitnessManager(settingsManager.GetEnvironmentSettings(), Logger);
      manager.DataChanged += UpdateDataContext;
      manager.TriggerDataChanged();
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