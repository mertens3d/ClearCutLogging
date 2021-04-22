using ClearCut.Support.Abstractions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ClearCut.Support.Settings.Concretions
{
  public class SiteSettings : ISiteSettings
  {
    public SiteSettings()
    {
    }

    [JsonConstructor]
    public SiteSettings(ICollection<TargetOptions> targets)
    {
      Targets = targets.Cast<ITargetOptions>().ToList();
    }
    public string FriendlyName { get; set; }
    public List<ITargetOptions> Targets { get; set; }
    public string RootFolder { get; set; }
  }
}
