using ClearCut.Support.Abstractions;
using System;

namespace ClearCut.Support.Witness.Models
{
  public class TargetWatcherEventArgs : EventArgs
  {
    public Guid WitnessId { get; internal set; }
    public ILastFile LastFile { get; internal set; }
  }
}