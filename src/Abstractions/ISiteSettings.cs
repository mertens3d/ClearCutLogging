using System.Collections.Generic;

namespace ClearCut.Support.Abstractions
{
  public interface ISiteSettings
  {
    string FriendlyName { get; set; }
    string RootFolder { get; set; }
    List<ITargetOptions> Targets { get; set; }
  }
}