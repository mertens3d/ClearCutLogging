using ClearCut.Support.Abstractions;
using System;

namespace ClearCut.Support.Witness.Concretions
{
    public class LogDataForDataContext : IOneLogDataContext
    {
        public bool AutoLoadEnabled { get; set; } = false;
        public IMostRecentMatchingLogFile MostRecentLogFile { get; set; }
        public Guid TargetId { get; set; }
        public ITargetOptions Target { get; set; }
    }
}