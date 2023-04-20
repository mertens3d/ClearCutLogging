using ClearCut.Support.Abstractions;
using Newtonsoft.Json;

namespace ClearCut.Support.Settings.Concretions
{
    public class TargetedLogOptions : ITargetedLogOptions
    {
        [JsonProperty("ChildDirectory")]
        public string LogChildDirectory { get; set; }

        [JsonProperty("FileFilter")]
        public string LogFileFilter { get; set; }

        [JsonProperty("FriendlyName")]
        public string LogFriendlyName { get; set; }
    }
}