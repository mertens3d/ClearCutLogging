﻿using System;

namespace ClearCut.Support.Abstractions
{
    public interface IOneLogDataContext
    {
        ITargetedLogOptions Target { get; set; }
        IMostRecentMatchingLogFile MostRecentLogFile { get; set; }
        Guid TargetId { get; set; }
        bool AutoLoadEnabled { get; set; }
    }
}