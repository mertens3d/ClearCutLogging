using ClearCut.Support.Abstractions;
using ClearCut.Support.Witness.Watchers;
using Serilog;
using System.Collections.Generic;
using System.Linq;

namespace ClearCut.Support.Witness.Services
{
    public class WitnessManager : IWitnessManager
    {
        public List<ISiteFolderWatcher> SiteFolderWatchers { get; set; } = new List<ISiteFolderWatcher>();

        private IEnvironementSettings EnvironementSettings { get; }

        private ILogger Logger { get; }

        public WitnessManager(ISettingsManager settingsManager, ILogger logger)
        {
            Logger = logger;
            EnvironementSettings  = settingsManager.GetEnvironmentSettings();
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
            if (EnvironementSettings != null && EnvironementSettings.SiteFolderSettings.Any())
            {
                foreach (var siteSetting in EnvironementSettings.SiteFolderSettings)
                {
                    SiteFolderWatchers.Add(new SiteFolderWatcher(siteSetting, Logger));
                }
            }
        }
    }
}