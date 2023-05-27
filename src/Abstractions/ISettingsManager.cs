using System.Collections.Generic;

namespace ClearCut.Support.Abstractions
{
    public interface ISettingsManager
    {
        IEnvironementSettings EnvironmentSettings { get; }
        IApplicationSettings ApplicationSettings { get; }

        void SaveApplicationSettings();
    }
}