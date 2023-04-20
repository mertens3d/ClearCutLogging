using ClearCut.Main.Views;
using ClearCut.Support.Abstractions;
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
        private readonly ILogger _logger;
        private readonly TabItem _tabAdd;
        private readonly List<TabItem> _tabItems;
        private readonly IWitnessManager _witnessManager;

        public MainWindow(ILogger logger, ISettingsManager settingsManager, IWitnessManager witnessManager)
        {
            _logger = logger;
            try
            {
                InitializeComponent();

                _witnessManager = witnessManager;

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

        private TabItem AddTabItem(SiteFolderWatcher siteWatcher)
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
            if (_witnessManager != null)
            {
                //List<ISiteSettings> siteSettings = SettingsManager.GetEnvironmentSettings().SiteSettings;

                foreach (SiteFolderWatcher siteWatcher in _witnessManager.SiteFolderWatchers)
                {
                    this.AddTabItem(siteWatcher);
                }
            }
        }
    }
}