using ClearCut.Support.Abstractions;
using ClearCut.Support.Witness.Models;
using Serilog;
using System;
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
      InitWatchers();
      InitEvents();
    }

    public event EventHandler<WitnessEventArgs> DataChanged;

    private List<TargetWatcher> TargetWatchers { get; set; } = new List<TargetWatcher>();
    private ILogger Logger { get; }
    private IEnvironementSettings EnvironementSettings { get; }

    private void InitWatchers()
    {
      if (EnvironementSettings != null && EnvironementSettings.SiteSettings.Any())
      {
        foreach (var siteSetting in EnvironementSettings.SiteSettings)
        {
          if (siteSetting.Targets != null && siteSetting.Targets.Any())
          {
            foreach (var target in siteSetting.Targets)
            {
              if (string.IsNullOrEmpty(target.FileFilter))
              {
                target.FileFilter = "";
              }

              TargetWatchers.Add(new TargetWatcher(siteSetting.RootFolder, target, Logger));
            }
          }
        }
      }
    }

    public void TriggerDataChanged()
    {
      foreach (var watcher in TargetWatchers)
      {
        watcher.TriggerDataChange();
      }
    }

    protected void OnDataChanged(object sender, TargetWatcherEventArgs e)
    {
      Guid witnessId = e.WitnessId;
      TargetWatcher foundWitness = TargetWatchers.Where(x => x.WatcherId.Equals(witnessId)).FirstOrDefault();

      if (foundWitness != null)
      {
        WitnessEventArgs witnessEventArgs = new WitnessEventArgs();

        foundWitness.LastFile = e.LastFile;

        var lastFiles = new List<ILastFile>();
        TargetWatchers.ForEach(x => lastFiles.Add(x.LastFile));

        witnessEventArgs.LastFiles = lastFiles.OrderBy(x => x.TimeSpan).ToList();


        EventHandler<WitnessEventArgs> handler = DataChanged;
        handler?.Invoke(this, witnessEventArgs);
      }
    }

    //private WitnessOptions GetWitnessOptions()
    //{
    //  var targets = new List<ITargetOptions> {
    //  new TargetOptions()
    //  {
    //    FriendlyName = "Sitecore Log",
    //    ChildDirectory = @"C:\inetpub\wwwroot\LearnEXMsc.dev.local\App_Data\logs",
    //    FileFilter = "log.*.txt"
    //  },
    //  new TargetOptions()
    //  {
    //    FriendlyName = "xConnect Log",
    //    ChildDirectory = @"C:\inetpub\wwwroot\LearnEXMxconnect.dev.local\App_Data\Logs",
    //    FileFilter = "*.txt"
    //  } };

    //  var witness = new WitnessOptions()
    //  {
    //    Targets = targets
    //  };

    //  return witness;
    //}

    private void InitEvents()
    {
      foreach (var watcher in TargetWatchers)
      {
        watcher.DataChanged += OnDataChanged;
      }
    }
  }
}