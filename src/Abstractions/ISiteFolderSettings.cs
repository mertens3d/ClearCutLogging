using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ClearCut.Support.Abstractions
{
    public interface ISiteFolderSettings
    {
        string FriendlyName { get; set; }
        string RootFolder { get; set; }

        List<ITargetedLogOptions> TargetedLogs { get; set; }
    }
}