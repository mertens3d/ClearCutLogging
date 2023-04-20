using ClearCut.Support.Abstractions;
using ClearCut.Support.Settings.Concretions;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClearCut.Support.Settings.Services
{
    public class SettingsManager : ISettingsManager
    {
        private ILogger _logger;

        public SettingsManager(ILogger logger)
        {
            _logger = logger;
            FileInfo exeLocation = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            ExeDirectoryInfo = new DirectoryInfo(exeLocation.DirectoryName);
            //CreateExample();
        }

        private DirectoryInfo ExeDirectoryInfo { get; set; }

        public IEnvironementSettings GetEnvironmentSettings()
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
                            toReturn.SiteFolderSettings.Add(result);
                        }
                    }
                }
            }

            return toReturn;
        }

      //  private void CreateExample()
      //  {
      //      var sampleSetting = new SiteFolderSettings();
      //      sampleSetting.FriendlyName = "Example Site Name";
      //      sampleSetting.RootFolder = @"C:\inetpub\wwwroot\MyExampleFolder";
      //      sampleSetting.TargetedLogs = new List<ITargetedLogOptions>
      //{
      //  new TargetedLogOptions
      //  {
      //    LogChildDirectory = @"\App_Data\logs",
      //    LogFriendlyName = "Logs",
      //    LogFileFilter = "log.*.txt"
      //  },
      //  new TargetedLogOptions
      //  {
      //    LogChildDirectory = @"\App_Data\logs",
      //    LogFriendlyName = "Other Logs",
      //    LogFileFilter =  "Other.log.*.txt"
      //  },
      //};

      //      WriteSettings(sampleSetting);
      //  }

        private ISiteFolderSettings HarvestSiteSetting(FileInfo settingFile)
        {
            ISiteFolderSettings toReturn = null;

            if (settingFile != null && settingFile.Exists)
            {
                using (StreamReader file = File.OpenText(settingFile.FullName))
                {
                    var serializer = new JsonSerializer();
                    serializer.TypeNameHandling = TypeNameHandling.Auto;
                    serializer.NullValueHandling = NullValueHandling.Ignore;

                    try
                    {
                        var fileContents = file.ReadToEnd();
                        var result = (SiteFolderSettings)Newtonsoft.Json.JsonConvert.DeserializeObject(fileContents, typeof(SiteFolderSettings));
                        //var result = (SiteFolderSettings)serializer.Deserialize(fileContents, typeof(SiteFolderSettings));
                        if (result != null)
                        {
                            toReturn = result;
                        }
                    }
                    catch (JsonException jex)
                    {
                        _logger.Error(jex.Message, jex);
                    }
                }
            }

            return toReturn;
        }

        private void WriteSettings(SiteFolderSettings siteSetting)
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
                    serializer.Serialize(writer, siteSetting, typeof(SiteFolderSettings));
                }
            }
        }
    }
}