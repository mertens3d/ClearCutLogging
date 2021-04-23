using System;
using System.IO;

namespace ClearCut.Support.Abstractions
{
  public interface ILastFile
  {
    string Age { get; }
    FileInfo FileInfo { get; }
    public string FriendlyName { get; set; }
    public Guid TargetId { get; set; }
    TimeSpan TimeSpan { get; }
  }
}