using ClearCut.Support.Abstractions;
using System;

namespace ClearCut.Support.Witness.Models
{
  public class TargetWatcherEventArgs : EventArgs
  {
    public IOneLogDataContext LastFile { get; internal set; }
    public Guid WitnessId { get; internal set; }
  }
}