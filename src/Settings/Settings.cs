using ClearCut.Support.Abstractions;
using System.Collections.Generic;

namespace ClearCut.Support.Settings
{
  public class EnvironementSettings : IEnvironementSettings
  {
    public List<ISiteSettings> SiteSettings { get; set; } = new List<ISiteSettings>();
  }
}