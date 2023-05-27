using System.Collections.Generic;

namespace ClearCut.Support.Abstractions
{
    public interface IApplicationSettings
    {
        public List<ILogViewers> LogViewers { get; set; }
        double WindowHeight { get; set; }
        double WindowWidth { get; set; }
    }
}