﻿using System.Collections.Generic;

namespace ClearCut.Support.Abstractions
{
    public interface IWitnessOptions
  {
    public List<ITargetedLogOptions> Targets { get; set; }
  }
}