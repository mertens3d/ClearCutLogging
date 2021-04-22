namespace ClearCut.Support.Abstractions
{
  public interface ITargetOptions
  {
    public string ChildDirectory { get; set; }
    public string FileFilter { get; set; }
    string FriendlyName { get; }
  }
}