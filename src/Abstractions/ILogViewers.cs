using System.Collections.Generic;

namespace ClearCut.Support.Abstractions
{
    public interface ILogViewers
    {
        public string FriendlyName { get; set; }
        public List<string> ExeLocationCandidates { get; set; }
        public string ButtonText { get; set; }
        public string Args { get; set; }

    }
}