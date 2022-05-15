using ClearCut.Main.Helpers;
using ClearCut.Support.Abstractions;
using ClearCut.Support.Witness.Concretions;
using Serilog;
using System;
using System.IO;
using System.Linq;

namespace ClearCut.Support.Witness
{
    public class OneLogFileFileChooser
    {
        private ILogger Logger;
        private AgeHelper _ageHelper;
        private ExternalProgramHelpers _externalProgramHelper;

        public string RootFolder { get; }

        public IOneLogDataContext LogDataForDataContext { get; set; }
        private string _previousLogFileName = string.Empty;

        public OneLogFileFileChooser(ILogger logger, string rootFolder, ITargetOptions target, Guid watcherId)
        {
            this.Logger = logger;
            RootFolder = rootFolder;
            LogDataForDataContext = new LogDataForDataContext()
            {
                TargetId = watcherId,
                Target = target
            };
            _ageHelper = new AgeHelper(Logger);
            _externalProgramHelper = new ExternalProgramHelpers();
        }

        public void PopulateLastFile()
        {
            LogDataForDataContext.MostRecentLogFile = GetNewestMatchingLogFile();
            if (LogDataForDataContext.MostRecentLogFile?.FileInfo != null)
            {
                if (!_previousLogFileName.Equals(LogDataForDataContext.MostRecentLogFile.FileInfo.FullName)
                    && LogDataForDataContext.AutoLoadEnabled)
                {
                    _externalProgramHelper.OpenInNotePadCPP(LogDataForDataContext.MostRecentLogFile.FileInfo.FullName);
                }

                
                _previousLogFileName = LogDataForDataContext.MostRecentLogFile.FileInfo.FullName;
            }
        }

        public IMostRecentMatchingLogFile GetNewestMatchingLogFile()
        {
            IMostRecentMatchingLogFile toReturn = null;

            var dirInfo = new DirectoryInfo(Path.Combine(RootFolder, LogDataForDataContext.Target.ChildDirectory));
            if (dirInfo != null && dirInfo.Exists)
            {
                var lastFileInfo = dirInfo.GetFiles(LogDataForDataContext.Target.FileFilter)
                  .OrderByDescending(x => x.LastWriteTime)
                  .FirstOrDefault();

                if (lastFileInfo != null)
                {
                    lastFileInfo.Refresh();
                    var timeSpan = _ageHelper.CalculateTimeSpan(lastFileInfo);

                    toReturn = new MostRecentMatchingLogFile()
                    {
                        FileInfo = lastFileInfo,
                        FriendlyName = LogDataForDataContext.Target.FriendlyName,
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