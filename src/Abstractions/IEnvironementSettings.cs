using System.Collections.Generic;

namespace ClearCut.Support.Abstractions
{
    public interface IEnvironementSettings
    {
        List<ISiteFolderSettings> SiteFolderSettings { get; set; }
    }
}