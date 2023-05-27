using ClearCut.Support.Abstractions;
using ClearCut.Support.Witness.Watchers;
using Serilog;
using System.Collections.Generic;

namespace ClearCut.Support.Witness.Services
{
    public class WitnessManager : IWitnessManager
    {
        public List<ISiteFolderWatcher> SiteFolderWatchers { get; set; } = new List<ISiteFolderWatcher>();

        private ISettingsManager SettingsManager { get; }

        private ILogger Logger { get; }

        public WitnessManager(ISettingsManager settingsManager, ILogger logger)
        {
            Logger = logger;
            SettingsManager = settingsManager;
            InitSiteWatchers();
        }

        public void TriggerSiteChanged()
        {
            foreach (var siteWatcher in SiteFolderWatchers)
            {
                siteWatcher.TriggerDataChanged();
            }
        }

        private void InitSiteWatchers()
        {
            SettingsManager.EnvironmentSettings.SiteFolderSettings.ForEach(x => SiteFolderWatchers.Add(new SiteFolderWatcher(x, Logger)));
        }
    }
}