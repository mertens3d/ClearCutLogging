using ClearCut.Main.ViewModels;
using ClearCut.Support.Abstractions;
using ClearCut.Support.Witness.Models;
using ClearCut.Support.Witness.Watchers;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClearCut.Main.Views
{
    /// <summary>
    /// Interaction logic for ResultsList.xaml
    /// </summary>
    public partial class ResultsList : UserControl
    {
        private ButtonsHandler _buttonsHandler;

        //private SiteWatcher SiteWatcher { get; set; }

        public ResultsList()
        {
            InitializeComponent();
        }

        public RefreshTimer RefreshTimer { get; private set; }
        public SiteFolderWatcher SiteWatcher { get; private set; }
        private ResultsViewModel ResultsViewModel { get; set; }

        public void InitWithContext(SiteFolderWatcher siteWatcher)
        {
            SiteWatcher = siteWatcher;
            //DataContext = siteWatcher;
            _buttonsHandler = new ButtonsHandler(siteWatcher, this);
            StageSetUp();
        }

        private void InitWitness()
        {
            //  var tabControl = tabControl

            ResultsViewModel = new ResultsViewModel();
            if (SiteWatcher != null)
            {
                SiteWatcher.DataChanged += UpdateDataContext;
                SiteWatcher.TriggerDataChanged();
            }

            //var test = this.FindName("DataTemplateStackPanel");
        }

        //private void OnCbAutoLoadCPPChecked(object sender, RoutedEventArgs e)
        //{
        //    //MessageBox.Show("clicked");
        //    _buttonsHandler.HandleNotepadPPAutoLoadClick((CheckBox) sender, true);
        //}

        //private void OnCbAutoLoadCPPUnChecked(object sender, RoutedEventArgs e)
        //{
        //    //MessageBox.Show("clicked");
        //    _buttonsHandler.HandleNotepadPPAutoLoadClick((CheckBox) sender, false);
        //}

        private void OnClickBareTail(object sender, RoutedEventArgs e)
        {
            _buttonsHandler.HandleBareTailClick((Button) sender);
        }

        private void OnClickClearLog(object sender, RoutedEventArgs e)
        {

            _buttonsHandler.HandleClickClearLog((Button) sender);
        }

        private void OnClickNotepadPP(object sender, RoutedEventArgs e)
        {
            _buttonsHandler.HandleNotepadPPClick((Button) sender);
        }
        private void OnClickVSCode(object sender, RoutedEventArgs e)
        {
            _buttonsHandler.HandleVSCodeClick((Button)sender);
        }

        private void OnToggledWatch(object sender, RoutedEventArgs e)
        {
            SiteWatcher.ToggleWatch();
            _buttonsHandler.SetButtonColor();
        }

        private void SetDataContext(List<IOneLogDataContext> lastFiles)
        {
            ResultsViewModel.LastFiles = lastFiles;
            this.Dispatcher.Invoke(() =>
            {
                idListView.ItemsSource = ResultsViewModel.LastFiles;
            });
        }

        private void StageSetUp()
        {
            InitWitness();
            RefreshTimer = new RefreshTimer(this.countDown, SiteWatcher);
            _buttonsHandler.SetButtonColor();
        }

        private void StageTearDown()
        {
        }

        private void UpdateDataContext(object sender, WitnessEventArgs witnessEventArgs)
        {
            if (witnessEventArgs?.LastFiles != null && witnessEventArgs.LastFiles.Any())
            {
                SetDataContext(witnessEventArgs.LastFiles);
            }
        }
    }
}