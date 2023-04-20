using ClearCut.Support.Abstractions;
using System.Collections.Generic;

namespace ClearCut.Support.Settings
{
  public class EnvironementSettings : IEnvironementSettings
  {
    public List<ISiteFolderSettings> SiteFolderSettings { get; set; } = new List<ISiteFolderSettings>();
  }
}