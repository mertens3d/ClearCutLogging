using ClearCut.Support.Abstractions;
using ClearCut.Support.Witness.Models;
using Serilog;
using System;
using System.IO;

namespace ClearCut.Support.Witness
{
    public class OneLogWatcher
    {
        private FileSystemEventHandler _eventHandlerDataChanged;
        private FileSystemEventHandler _eventHandlerDataCreated;
        public OneLogFileFileChooser _fileChooser { get; }
        private FileSystemWatcher _fileSystemWatcher;
        private ITargetedLogOptions target;

        public OneLogWatcher(string rootFolder, ITargetedLogOptions target, ILogger logger)
        {
            this.target = target;
            this.Logger = logger;
            this.RootFolder = rootFolder;
            WatcherId = Guid.NewGuid();
            InitFileSystemWatcher();

            _fileChooser = new OneLogFileFileChooser(Logger, RootFolder, target, WatcherId);
            _fileChooser.PopulateLastFile();
        }

        public event EventHandler<TargetWatcherEventArgs> OneLogDataChangedEventHandler;

        public Guid WatcherId { get; internal set; }
        private ILogger Logger { get; }
        private string RootFolder { get; }
        public string MostRecentFileName { get; set; } = string.Empty;
        public bool MostRecentFileNameChanged { get; internal set; }

        internal void TriggerDataChange()
        {
            _fileChooser.PopulateLastFile();
            var targetEventWatcher = new TargetWatcherEventArgs()
            {
                LastFile = _fileChooser.LogDataForDataContext,
                WitnessId = WatcherId,
            };

            OnDataChanged(targetEventWatcher);
        }

        private void CallBackDataChanged(object sender, FileSystemEventArgs e)
        {
            TriggerDataChange();
        }

        private FileSystemWatcher CreateFileSystemWatcher(DirectoryInfo candidateDirectory)
        {
            return new FileSystemWatcher
            {
                Path = candidateDirectory.FullName,
                Filter = target.LogFileFilter,
                EnableRaisingEvents = true,
                IncludeSubdirectories = false,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
            };
        }

        private void InitFileSystemWatcher()
        {
            var candidateDirectory = new DirectoryInfo(Path.Combine(RootFolder, target.LogChildDirectory));

            if (candidateDirectory.Exists)
            {
                _fileSystemWatcher = CreateFileSystemWatcher(candidateDirectory);

                _eventHandlerDataChanged = new FileSystemEventHandler(CallBackDataChanged);
                _eventHandlerDataCreated = new FileSystemEventHandler(CallBackDataChanged);
                _fileSystemWatcher.Changed += _eventHandlerDataChanged;
                _fileSystemWatcher.Created += _eventHandlerDataCreated;
            }
            else
            {
                Logger.Error("Candidate path did not exist: " + candidateDirectory.FullName);
            }
        }

        internal void TearDown()
        {
            if (_fileSystemWatcher != null)
            {
                _fileSystemWatcher.Changed -= _eventHandlerDataChanged;
                _fileSystemWatcher.Created -= _eventHandlerDataCreated;
                _eventHandlerDataChanged = null;
                _eventHandlerDataCreated = null;
            }

            TriggerDataChange();
        }

        private void OnDataChanged(TargetWatcherEventArgs e)
        {
            EventHandler<TargetWatcherEventArgs> eventHandler = OneLogDataChangedEventHandler;
            eventHandler?.Invoke(this, e);
        }

       
    }
}