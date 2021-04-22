using ClearCut.Support.Abstractions;
using System;
using System.IO;

namespace ClearCut.Support.Witness.Concretions
{
  public class LastFile : ILastFile
  {
    public string FriendlyName { get; set; }
    public Guid TargetId { get; set; }
    public FileInfo FileInfo { get; internal set; }
  }
}