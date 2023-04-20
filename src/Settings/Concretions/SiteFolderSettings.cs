using ClearCut.Support.Abstractions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ClearCut.Support.Settings.Concretions
{
    public class SiteFolderSettings : ISiteFolderSettings
    {
        public string FriendlyName { get; set; }

        public string RootFolder { get; set; }

        public List<ITargetedLogOptions> TargetedLogs { get; set; }

        public SiteFolderSettings()
        {
        }

        [JsonConstructor]
        public SiteFolderSettings(ICollection<TargetedLogOptions> targets)
        {
            TargetedLogs = targets.Cast<ITargetedLogOptions>().ToList();
        }
    }
}