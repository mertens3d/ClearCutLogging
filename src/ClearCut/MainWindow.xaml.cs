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

        private readonly IWitnessManager _witnessManager;
        private readonly ISettingsManager _settingsManager;
        private readonly XamlTabManager _xamlTabManager;

        public MainWindow(ILogger logger, ISettingsManager settingsManager, IWitnessManager witnessManager)
        {
            _logger = logger;
            try
            {
                InitializeComponent();

                _witnessManager = witnessManager;
                _settingsManager = settingsManager;

               
                _xamlTabManager = new XamlTabManager(tabDynamic, _logger);

                InitWatch();
                RestoreWindowSize();
                this.SizeChanged += MyWindow_SizeChanged;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
        }

        private void RestoreWindowSize()
        {
            if(_settingsManager?.ApplicationSettings != null)
            {
                this.Width = _settingsManager.ApplicationSettings.WindowWidth;
                this.Height = _settingsManager.ApplicationSettings.WindowHeight; 
            }
        }

        private void MyWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _settingsManager.ApplicationSettings.WindowWidth = this.Width;
            _settingsManager.ApplicationSettings.WindowHeight = this.Height;
            _settingsManager.SaveApplicationSettings();

        }


        private void InitWatch()
        {
            var tabs = new ObservableCollection<ResultsList>();
            if (_witnessManager != null)
            {
                //List<ISiteSettings> siteSettings = SettingsManager.GetEnvironmentSettings().SiteSettings;
                _logger.Information($"{nameof(_witnessManager.SiteFolderWatchers)} count: {_witnessManager.SiteFolderWatchers.Count}");

                _xamlTabManager.AddTabsForWatchedFolders(_witnessManager.SiteFolderWatchers);

                
            }
        }
    }
}