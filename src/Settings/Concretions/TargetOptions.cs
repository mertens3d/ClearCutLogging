using ClearCut.Support.Abstractions;

namespace ClearCut.Support.Settings.Concretions
{
  public class TargetOptions : ITargetOptions
  {
    public string ChildDirectory { get; set; }
    public string FileFilter { get; set; }
    public string FriendlyName { get; set; }
  }
}