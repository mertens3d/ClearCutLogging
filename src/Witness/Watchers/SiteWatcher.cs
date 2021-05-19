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
      InitLogWatchers();
      InitEvents();
    }

    public List<LogWatcher> WatchedLogs { get; set; } = new List<LogWatcher>();
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

    protected void OnDataChanged(object sender, TargetWatcherEventArgs e)
    {
      Guid witnessId = e.WitnessId;
      LogWatcher logwatcher = WatchedLogs.Where(x => x.WatcherId.Equals(witnessId)).FirstOrDefault();

      if (logwatcher != null)
      {
        WitnessEventArgs witnessEventArgs = new WitnessEventArgs();

        logwatcher.LastFile = e.LastFile;

        var lastFiles = new List<ILastFile>();
        WatchedLogs.ForEach(x => lastFiles.Add(x.LastFile));

        if (lastFiles != null && lastFiles.Any())
        {
          witnessEventArgs.LastFiles = lastFiles.Where(x => x != null).OrderBy(x => x.TimeSpan).ToList();
        }

        EventHandler<WitnessEventArgs> handler = DataChanged;
        handler?.Invoke(this, witnessEventArgs);
      }
    }

    private void InitEvents()
    {
      foreach (var watcher in WatchedLogs)
      {
        watcher.DataChanged += OnDataChanged;
      }
    }

    private void InitLogWatchers()
    {
      if (SiteSettings.Targets != null && SiteSettings.Targets.Any())
      {
        foreach (var target in SiteSettings.Targets)
        {
          if (string.IsNullOrEmpty(target.FileFilter))
          {
            target.FileFilter = "";
          }

          WatchedLogs.Add(new LogWatcher(SiteSettings.RootFolder, target, Logger));
        }
      }
    }
  }
}