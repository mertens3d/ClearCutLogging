using System.Collections.Generic;

namespace ClearCut.Support.Abstractions
{
  public interface IEnvironementSettings
  {
    List<ISiteSettings> SiteSettings { get; set; }

  }
}