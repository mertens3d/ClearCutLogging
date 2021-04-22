using ClearCut.Support.Abstractions;
using ClearCut.Support.Settings.Concretions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClearCut.Support.Settings
{
  public class SettingsManager
  {
    public SettingsManager()
    {
      FileInfo exeLocation = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
      ExeDirectoryInfo = new DirectoryInfo(exeLocation.DirectoryName);
      //CreateExample();
    }

    private DirectoryInfo ExeDirectoryInfo { get; set; }

    public EnvironementSettings GetEnvironmentSettings()
    {
      var toReturn = new EnvironementSettings();

      if (ExeDirectoryInfo != null)
      {
        var settingsFiles = ExeDirectoryInfo.GetFiles(Constants.FileNames.SettingsFilter, SearchOption.AllDirectories).ToList();

        if (settingsFiles != null && settingsFiles.Any())
        {
          foreach (var settingFile in settingsFiles)
          {
            var result = HarvestSiteSetting(settingFile);
            if (result != null)
            {
              toReturn.SiteSettings.Add(result);
            }
          }
        }
      }

      return toReturn;
    }

    private void CreateExample()
    {
      var sampleSetting = new SiteSettings();
      sampleSetting.FriendlyName = "Example Site Name";
      sampleSetting.RootFolder = @"C:\inetpub\wwwroot\MyExampleFolder";
      sampleSetting.Targets = new List<ITargetOptions>
      {
        new TargetOptions
        {
          ChildDirectory = @"\App_Data\logs",
          FriendlyName = "Logs",
          FileFilter = "log.*.txt"
        },
        new TargetOptions
        {
          ChildDirectory = @"\App_Data\logs",
          FriendlyName = "Other Logs",
          FileFilter =  "Other.log.*.txt"
        },
      };

      WriteSettings(sampleSetting);
    }

    private SiteSettings HarvestSiteSetting(FileInfo settingFile)
    {
      SiteSettings toReturn = null;

      if (settingFile != null)
      {
        using (StreamReader file = File.OpenText(settingFile.FullName))
        {
          var serializer = new JsonSerializer();
          serializer.TypeNameHandling = TypeNameHandling.Auto;
          serializer.NullValueHandling = NullValueHandling.Ignore;
          SiteSettings result = (SiteSettings)serializer.Deserialize(file, typeof(SiteSettings));

          //JsonConvert.DeserializeObject<Account>(json);

          //  JObject result = (JObject)JToken.ReadFrom(reader);

          if (result != null)
          {
            toReturn = result;// result.ToObject<SiteSettings>();
          }
        }
      }

      return toReturn;
    }

    private void WriteSettings(SiteSettings siteSetting)
    {
      if (ExeDirectoryInfo != null && siteSetting != null)
      {
        var serializer = new JsonSerializer();
        serializer.NullValueHandling = NullValueHandling.Ignore;
        serializer.TypeNameHandling = TypeNameHandling.Auto;
        serializer.Formatting = Formatting.Indented;

        using (StreamWriter file = File.CreateText(ExeDirectoryInfo.Parent + @"\" + Constants.FileNames.ExampleJsonFileName))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
          serializer.Serialize(writer, siteSetting, typeof(SiteSettings));
        }
      }
    }
  }
}