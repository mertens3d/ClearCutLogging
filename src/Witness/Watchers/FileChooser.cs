using ClearCut.Support.Abstractions;
using ClearCut.Support.Witness.Concretions;
using Serilog;
using System;
using System.IO;
using System.Linq;

namespace ClearCut.Support.Witness
{
    public class FileChooser
    {
        private ILogger Logger;
        private AgeHelper _ageHelper;
        public string RootFolder { get; }

        public ILastFile LastFile { get;  set; }
        private ITargetOptions _target;

        public Guid WatcherId { get; }

        public FileChooser(ILogger logger, string rootFolder, ITargetOptions target, Guid watcherId)
        {
            this.Logger = logger;
            RootFolder = rootFolder;
            _target = target;
            WatcherId = watcherId;
            _ageHelper = new AgeHelper(Logger);
        }
        public void PopulateLastFile()
        {
            LastFile = GetNewestMatch();
        }

        public ILastFile GetNewestMatch()
        {
            ILastFile toReturn = null;

            var dirInfo = new DirectoryInfo(Path.Combine(RootFolder, _target.ChildDirectory));
            if (dirInfo != null && dirInfo.Exists)
            {
                var lastFileInfo = dirInfo.GetFiles(_target.FileFilter)
                  .OrderByDescending(x => x.LastWriteTime)
                  .FirstOrDefault();

                if (lastFileInfo != null)
                {
                    lastFileInfo.Refresh();
                    var timeSpan = _ageHelper.CalculateTimeSpan(lastFileInfo);

                    toReturn = new LastFile()
                    {
                        FileInfo = lastFileInfo,
                        FriendlyName = _target.FriendlyName,
                        TargetId = WatcherId,
                        TimeSpan = timeSpan,
                        Age = _ageHelper.CalculateAge(timeSpan)
                    };
                }
            }
            else
            {
                if (dirInfo == null)
                {
                    Logger.Error("Directory was null: " + dirInfo.FullName);
                }
                else if (!dirInfo.Exists)
                {
                    Logger.Error("Directory does not exist: " + dirInfo.FullName);
                }
            }

            return toReturn;
        }
    }
}