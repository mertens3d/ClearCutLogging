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

        public List<OneLogWatcher> AllLogWatchers { get; set; } = new List<OneLogWatcher>();
        public ISiteSettings SiteSettings { get; }
        private ILogger Logger { get; }

        public void TriggerDataChanged()
        {
            foreach (var watcher in AllLogWatchers)
            {
                watcher.TriggerDataChange();
            }
        }

        public event EventHandler<WitnessEventArgs> DataChanged;

        public bool IsEnabled { get; set; } = true;

        protected void OnDataChangedEventListener(object sender, TargetWatcherEventArgs e)
        {
            UpdateOneWatchedLog(e.WitnessId, e.LastFile);
        }

        private void UpdateOneWatchedLog(Guid witnessIdOfChangedLog, IOneLogDataContext oneLogDataContext)
        {
            OneLogWatcher oneLogwatcher = AllLogWatchers.FirstOrDefault(x => x.WatcherId.Equals(witnessIdOfChangedLog));

            if (oneLogwatcher != null)
            {
                if (oneLogDataContext?.MostRecentLogFile?.FileInfo != null)
                {
                    if (!oneLogwatcher.MostRecentFileName.Equals(oneLogDataContext.MostRecentLogFile.FileInfo.FullName))
                    {
                        oneLogwatcher.MostRecentFileNameChanged = true;
                    }
                    oneLogwatcher.MostRecentFileName = oneLogDataContext.MostRecentLogFile.FileInfo.FullName;
                }
                oneLogwatcher._fileChooser.LogDataForDataContext = oneLogDataContext;
                NotifyWatchersDataChanged(oneLogwatcher);
            }
        }

        private void NotifyWatchersDataChanged(OneLogWatcher oneLogwatcher)
        {
            WitnessEventArgs witnessEventArgs = new WitnessEventArgs();

            var lastFiles = new List<IOneLogDataContext>();
            AllLogWatchers.ForEach(x => lastFiles.Add(x._fileChooser.LogDataForDataContext));

            if (lastFiles != null && lastFiles.Any())
            {
                witnessEventArgs.LastFiles = lastFiles.Where(x => x?.MostRecentLogFile != null).OrderBy(x => x.MostRecentLogFile.TimeSpan).ToList();
            }

            

            EventHandler<WitnessEventArgs> handler = DataChanged;
            handler?.Invoke(this, witnessEventArgs);
        }

        private void WatcherBuild()
        {
            BuildAllLogWatchers();
            AttachEventToAllLogWatchers();
        }

        private void AttachEventToAllLogWatchers()
        {
            foreach (var watcher in AllLogWatchers)
            {
                watcher.OneLogDataChangedEventHandler += OnDataChangedEventListener;
            }
        }

        private void WatchTearDown()
        {
            foreach (var watcher in AllLogWatchers)
            {
                watcher.TearDown();
                watcher.OneLogDataChangedEventHandler -= OnDataChangedEventListener;
            }

            AllLogWatchers.Clear();
        }

        private void BuildAllLogWatchers()
        {
            if (SiteSettings.Targets != null && SiteSettings.Targets.Any())
            {
                foreach (var target in SiteSettings.Targets)
                {
                    if (string.IsNullOrEmpty(target.FileFilter))
                    {
                        target.FileFilter = "";
                    }

                    AllLogWatchers.Add(new OneLogWatcher(SiteSettings.RootFolder, target, Logger));
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