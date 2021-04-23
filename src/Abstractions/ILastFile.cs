using System;
using System.IO;

namespace ClearCut.Support.Abstractions
{
  public interface ILastFile
  {
    public string FriendlyName { get; set; }
    public Guid TargetId { get; set; }
    FileInfo FileInfo { get; }
    string Age { get; }
    TimeSpan TimeSpan { get; }
  }
}