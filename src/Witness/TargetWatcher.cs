using ClearCut.Support.Abstractions;
using ClearCut.Support.Witness.Concretions;
using ClearCut.Support.Witness.Models;
using Serilog;
using System;
using System.IO;
using System.Linq;

namespace ClearCut.Support.Witness
{
  public class TargetWatcher
  {
    private ITargetOptions target;

    private ILogger Logger { get; }

    public TargetWatcher(string rootFolder, ITargetOptions target, ILogger logger)
    {
      this.target = target;
      this.Logger = logger;
      this.RootFolder = rootFolder;
      WatcherId = Guid.NewGuid();
      InitFileSystemWatcher(); PopulateLastFile();
    }

    public event EventHandler<TargetWatcherEventArgs> DataChanged;

    public ILastFile LastFile { get; internal set; }
    public Guid WatcherId { get; internal set; }
    private string RootFolder { get; }

    internal void TriggerDataChange()
    {
      PopulateLastFile();
      var targetEventWatcher = new TargetWatcherEventArgs()
      {
        LastFile = LastFile,
        WitnessId = WatcherId,
      };

      OnDataChanged(targetEventWatcher);
    }

    private void CallBackDataChanged(object sender, FileSystemEventArgs e)
    {
      TriggerDataChange();
    }

    private ILastFile GetNewestMatch()
    {
      ILastFile toReturn = null;

      var dirInfo = new DirectoryInfo(Path.Combine(RootFolder, target.ChildDirectory));
      if (dirInfo != null && dirInfo.Exists)
      {
        var lastFileInfo = dirInfo.GetFiles(target.FileFilter)
          .OrderByDescending(x => x.LastWriteTime)
          .FirstOrDefault();

        if (lastFileInfo != null)
        {
          toReturn = new LastFile()
          {
            FileInfo = lastFileInfo,
            FriendlyName = target.FriendlyName,
            TargetId = WatcherId,
          };
        }
      }
      else
      {
        if(dirInfo == null)
        {

        Logger.Error("Directory was null: " + dirInfo.FullName);
        }
        else if(! dirInfo.Exists)
        {
          Logger.Error("Directory does not exist: " + dirInfo.FullName);
        }
      }

      return toReturn;
    }

    private void InitFileSystemWatcher()
    {
      var candidateDirectory = new DirectoryInfo(Path.Combine(RootFolder, target.ChildDirectory));

      if (candidateDirectory.Exists)
      {
        var watcher = new FileSystemWatcher
        {
          Path = candidateDirectory.FullName,
          Filter = target.FileFilter,
          EnableRaisingEvents = true,
          IncludeSubdirectories = false,
          NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
        };

        watcher.Changed += new FileSystemEventHandler(CallBackDataChanged);
        watcher.Created += new FileSystemEventHandler(CallBackDataChanged);
      }
      else
      {
        Logger.Error("Candidate path did not exist: " + candidateDirectory.FullName);
      }
    }

    private void OnDataChanged(TargetWatcherEventArgs e)
    {
      EventHandler<TargetWatcherEventArgs> handler = DataChanged;
      handler?.Invoke(this, e);
    }

    private void PopulateLastFile()
    {
      LastFile = GetNewestMatch();
    }
  }
}