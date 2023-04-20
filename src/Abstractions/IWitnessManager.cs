using System.Collections.Generic;

namespace ClearCut.Support.Abstractions
{
    public interface IWitnessManager
    {
        List<ISiteFolderWatcher> SiteFolderWatchers { get; }
    }
}