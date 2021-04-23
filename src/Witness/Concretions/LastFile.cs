using ClearCut.Support.Abstractions;
using System;
using System.IO;

namespace ClearCut.Support.Witness.Concretions
{
  public class LastFile : ILastFile
  {
    public string Age { get; internal set; }
    public FileInfo FileInfo { get; internal set; }
    public string FriendlyName { get; set; }
    public Guid TargetId { get; set; }
    public TimeSpan TimeSpan { get; internal set; }
  }
}