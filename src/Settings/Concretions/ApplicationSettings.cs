using ClearCut.Support.Abstractions;
using System.Collections.Generic;

namespace ClearCut.Support.Settings.Concretions
{
    public class ApplicationSettings : IApplicationSettings
    {
        public List<ILogViewers> LogViewers { get; set; } = new List<ILogViewers>();
        public List<string> WatchWordsRegex { get; set; } = new List<string> {
            Constants.Settings.Defaults.WatchWordInfo
        };

        public double WindowHeight { get; set; } = Constants.Settings.Defaults.WindowHeight;
        public double WindowWidth { get; set; } = Constants.Settings.Defaults.WindowWidth;
    }
}