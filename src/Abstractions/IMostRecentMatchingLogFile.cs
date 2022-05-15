using System;
using System.IO;

namespace ClearCut.Support.Abstractions
{
    public interface IMostRecentMatchingLogFile
    {
        string Age { get; }
        FileInfo FileInfo { get; }
        public string FriendlyName { get; set; }
        TimeSpan TimeSpan { get; }
    }
}