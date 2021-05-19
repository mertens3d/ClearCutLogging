using ClearCut.Support.Abstractions;
using ClearCut.Support.Witness.Watchers;
using Serilog;
using System.Collections.Generic;
using System.Linq;

namespace ClearCut.Support.Witness
{
  public class WitnessManager
  {
    public WitnessManager(IEnvironementSettings environmentSettings, ILogger logger)
    {
      Logger = logger;
      EnvironementSettings = environmentSettings;
      InitSiteWatchers();
    }

    private IEnvironementSettings EnvironementSettings { get; }

    private ILogger Logger { get; }

    public List<SiteWatcher> SiteWatchers { get; set; } = new List<SiteWatcher>();

    public void TriggerSiteChanged()
    {
      foreach (var siteWatcher in SiteWatchers)
      {
        siteWatcher.TriggerDataChanged();
      }
    }

    private void InitSiteWatchers()
    {
      if (EnvironementSettings != null && EnvironementSettings.SiteSettings.Any())
      {
        foreach (var siteSetting in EnvironementSettings.SiteSettings)
        {
          SiteWatchers.Add(new SiteWatcher(siteSetting, Logger));
        }
      }
    }
  }
}