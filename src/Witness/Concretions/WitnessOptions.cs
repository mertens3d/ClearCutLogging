using ClearCut.Support.Abstractions;
using System.Collections.Generic;

namespace ClearCut.Support.Witness.Concretions
{
  public class WitnessOptions : IWitnessOptions
  {
    public List<ITargetedLogOptions> Targets { get; set; } = new List<ITargetedLogOptions>();
  }
}