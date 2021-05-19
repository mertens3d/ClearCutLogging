using ClearCut.Main.Views;
using ClearCut.Support.Abstractions;
using ClearCut.Support.Settings;
using ClearCut.Support.Witness;
using ClearCut.Support.Witness.Watchers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ClearCut
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  ///
  public partial class MainWindow : Window
  {
    private List<TabItem> _tabItems;
    private TabItem _tabAdd;

    public MainWindow(ILogger logger)
    {
      Logger = logger;
      try
      {
        InitializeComponent();

        var settingsManager = new SettingsManager();
        WitnessManager = new WitnessManager(settingsManager.GetEnvironmentSettings(), logger);

        _tabItems = new List<TabItem>();
        InitWatch();


        // bind tab control
        tabDynamic.DataContext = _tabItems;

        tabDynamic.SelectedIndex = 0;
      }
      catch (Exception ex)
      {
        logger.Error(ex.ToString());
      }
    }

    private TabItem AddTabItem(SiteWatcher siteWatcher)
    {
      int count = _tabItems.Count;
      var tab = new TabItem();
      tab.Header = siteWatcher.SiteSettings.FriendlyName;


      //tab.HeaderTemplate = tabDynamic.FindResource("TabHeader") as DataTemplate;

      var resultsList = new ResultsList();
      

      resultsList.InitWithContext(siteWatcher);

      tab.Content = resultsList;
      _tabItems.Add(tab);

      return tab;
    }

    private void InitWatch()
    {
      var tabs = new ObservableCollection<ClearCut.Main.Views.ResultsList>();
      if (WitnessManager != null)
      {
        //List<ISiteSettings> siteSettings = SettingsManager.GetEnvironmentSettings().SiteSettings;

        foreach (SiteWatcher siteWatcher in WitnessManager.SiteWatchers)
        {
          this.AddTabItem(siteWatcher);

        }
      }
    }

    private ILogger Logger { get; }

  
    public WitnessManager WitnessManager { get; }
  }
}