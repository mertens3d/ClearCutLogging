using ClearCut.Support.Abstractions;
using System.Collections.Generic;

namespace ClearCut.Main.ViewModels
{
  public class ResultsViewModel
  {
    public List<ILastFile> LastFiles { get; set; } = new List<ILastFile>();
  }
}