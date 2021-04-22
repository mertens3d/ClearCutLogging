using ClearCut.Support.Abstractions;
using System;
using System.Collections.Generic;

namespace ClearCut.Support.Witness.Models
{
  public class WitnessEventArgs : EventArgs
  {
    public List<ILastFile> LastFiles { get; internal set; }
  }
}