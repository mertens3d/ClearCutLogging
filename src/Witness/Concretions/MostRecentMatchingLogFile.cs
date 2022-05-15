using ClearCut.Support.Abstractions;
using System;
using System.IO;

namespace ClearCut.Support.Witness.Concretions
{
    public class MostRecentMatchingLogFile : IMostRecentMatchingLogFile
    {
        public FileInfo FileInfo { get;  set; }
        public string FriendlyName { get; set; }
        public string Age { get;  set; }
        public TimeSpan TimeSpan { get;  set; }

    }
}