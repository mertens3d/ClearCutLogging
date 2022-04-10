using ClearCut.Support.Abstractions;
using ClearCut.Support.Witness.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClearCut.Support.Witness.Watchers
{
    public class SiteWatcher
    {
        public SiteWatcher(ISiteSettings siteSettings, ILogger logger)
        {
            Logger = logger;
            SiteSettings = siteSettings;

            WatcherBuild();
        }

        public List<OneLogWatcher> WatchedLogs { get; set; } = new List<OneLogWatcher>();
        public ISiteSettings SiteSettings { get; }
        private ILogger Logger { get; }

        public void TriggerDataChanged()
        {
            foreach (var watcher in WatchedLogs)
            {
                watcher.TriggerDataChange();
            }
        }

        public event EventHandler<WitnessEventArgs> DataChanged;

        public bool IsEnabled { get; set; } = true;

        protected void OnDataChanged(object sender, TargetWatcherEventArgs e)
        {
            Guid witnessId = e.WitnessId;
            OneLogWatcher logwatcher = WatchedLogs.Where(x => x.WatcherId.Equals(witnessId)).FirstOrDefault();

            if (logwatcher != null)
            {
                NotifyWatchersDataChanged(logwatcher, e);
            }
        }

        private void NotifyWatchersDataChanged(OneLogWatcher logwatcher, TargetWatcherEventArgs e)
        {
            WitnessEventArgs witnessEventArgs = new WitnessEventArgs();

            logwatcher._fileChooser.LastFile = e.LastFile;

            var lastFiles = new List<ILastFile>();
            WatchedLogs.ForEach(x => lastFiles.Add(x._fileChooser.LastFile));

            if (lastFiles != null && lastFiles.Any())
            {
                witnessEventArgs.LastFiles = lastFiles.Where(x => x != null).OrderBy(x => x.TimeSpan).ToList();
            }

            EventHandler<WitnessEventArgs> handler = DataChanged;
            handler?.Invoke(this, witnessEventArgs);
        }

        private void WatcherBuild()
        {
            BuildWatched();

            foreach (var watcher in WatchedLogs)
            {
                watcher.DataChanged += OnDataChanged;
            }
        }

        private void WatchTearDown()
        {
            foreach (var watcher in WatchedLogs)
            {
                watcher.TearDown();
                watcher.DataChanged -= OnDataChanged;
            }

            WatchedLogs.Clear();
        }

        private void BuildWatched()
        {
            if (SiteSettings.Targets != null && SiteSettings.Targets.Any())
            {
                foreach (var target in SiteSettings.Targets)
                {
                    if (string.IsNullOrEmpty(target.FileFilter))
                    {
                        target.FileFilter = "";
                    }

                    WatchedLogs.Add(new OneLogWatcher(SiteSettings.RootFolder, target, Logger));
                }
            }
        }

        public void ToggleWatch()
        {
            IsEnabled = !IsEnabled;
            if (IsEnabled)
            {
                WatcherBuild();
            }
            else
            {
                WatchTearDown();
            }
        }
    }
}