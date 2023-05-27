using ClearCut.Main.Views;
using ClearCut.Support.Abstractions;
using ClearCut.Support.Witness.Services;
using ClearCut.Support.Witness.Watchers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ClearCut
{
    public class XamlTabManager
    {
        private readonly List<TabItem> _tabItems = new List<TabItem>();
        private TabControl tabDynamic;
        private ILogger _logger;

        public XamlTabManager(TabControl tabDynamic, ILogger logger)
        {
            this.tabDynamic = tabDynamic;
            _logger = logger;

        }

        private TabItem AddTabForWatchedFolder(SiteFolderWatcher siteWatcher)
        {
            _logger.Information($"Adding {nameof(siteWatcher)} : {siteWatcher.SiteSettings.FriendlyName}");
            var tab = new TabItem();
            tab.Header = siteWatcher.SiteSettings.FriendlyName;

            //tab.HeaderTemplate = tabDynamic.FindResource("TabHeader") as DataTemplate;

            var resultsList = new ResultsList();

            resultsList.InitWithContext(siteWatcher);

            tab.Content = resultsList;
            _tabItems.Add(tab);
            
            return tab;
        }
        internal void AddTabItems(List<TabItem> tabItems)
        {
            // bind tab control
            tabDynamic.DataContext = tabItems;
            tabDynamic.SelectedIndex = 0;
        }

        internal void AddTabsForWatchedFolders(List<ISiteFolderWatcher> siteFolderWatchers)
        {
            foreach (SiteFolderWatcher siteWatcher in siteFolderWatchers)
            {
                AddTabForWatchedFolder(siteWatcher);
            }

            AddTabItems(_tabItems);
        }
    }
}