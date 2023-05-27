using ClearCut.Support.Abstractions;
using ClearCut.Support.Settings.Concretions;
using Foundation;
using Newtonsoft.Json;
using Serilog;
using System.IO;
using System.Linq;

namespace ClearCut.Support.Settings.Services
{
    public class SettingsManager : ISettingsManager
    {
        private IApplicationSettings _applicationSettings;
        private IEnvironementSettings _environmentSettings;
        private ILogger _logger;
        public IApplicationSettings ApplicationSettings => _applicationSettings ?? (_applicationSettings = GetApplicationSettings());
        public IEnvironementSettings EnvironmentSettings => _environmentSettings ?? (_environmentSettings = GetEnvironmentSettings());
        private DirectoryInfo ExeDirectoryInfo { get; set; }

        public SettingsManager(ILogger logger)
        {
            _logger = logger;
            _logger.Information(nameof(SettingsManager));
            FileInfo exeLocation = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            ExeDirectoryInfo = new DirectoryInfo(exeLocation.DirectoryName);
            //CreateExample();
        }
       

        private IApplicationSettings GetApplicationSettings()
        {
            IApplicationSettings toReturn = null;

            if (ExeDirectoryInfo != null)
            {
                var foundAppSettingsFile = new FileInfo(AppSettingsFileNameLong);// ExeDirectoryInfo.GetFiles(Constants.FileNames.AppSettingsFileNameShort, SearchOption.AllDirectories).FirstOrDefault();

                if (foundAppSettingsFile != null && foundAppSettingsFile.Exists)
                {
                    var result = HarvestApplicationSettings(foundAppSettingsFile);
                    if (result != null)
                    {
                        toReturn = result;
                    }
                   
                }
                
                if(toReturn == null)
                {
                    toReturn = new ApplicationSettings();
                }
            }

            return toReturn;
        }
        private IEnvironementSettings GetEnvironmentSettings()
        {
            var toReturn = new EnvironementSettings();

            if (ExeDirectoryInfo != null)
            {
                    _logger.Information($"ExeDirectoryInfo: {ExeDirectoryInfo.FullName}");
                    _logger.Information($"{nameof(Constants.FileNames.SettingsFilter)}: {Constants.FileNames.SettingsFilter}");
                var settingsFiles = ExeDirectoryInfo.GetFiles(Constants.FileNames.SettingsFilter, SearchOption.AllDirectories).ToList();

                if (settingsFiles != null && settingsFiles.Any())
                {
                    _logger.Information($"Setting count: {settingsFiles.Count}");

                    foreach (var settingFile in settingsFiles)
                    {
                    _logger.Information($"settingFile: {settingFile.FullName}");
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
        private IApplicationSettings HarvestApplicationSettings(FileInfo settingFile)
        {
            IApplicationSettings toReturn = null;

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
                        var result = (IApplicationSettings)JsonConvert.DeserializeObject(fileContents, typeof(ApplicationSettings));
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
                        var result = (SiteFolderSettings)JsonConvert.DeserializeObject(fileContents, typeof(SiteFolderSettings));
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
        private string AppSettingsFileNameLong => string.Concat(ExeDirectoryInfo, @"\" , Constants.FileNames.AppSettingsFileNameShort);
        public void SaveApplicationSettings()
        {
            if (ExeDirectoryInfo != null && _applicationSettings != null) {
                var serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.TypeNameHandling = TypeNameHandling.Auto;
                serializer.Formatting = Formatting.Indented;
                using (var file = File.CreateText(AppSettingsFileNameLong))
                using (var writer = new JsonTextWriter(file))
                {
                    serializer.Serialize(writer, ApplicationSettings, typeof(ApplicationSettings));
                }
            }
            else
            {
                _logger.ErrorOnce($"Either {nameof(ExeDirectoryInfo)} or {nameof(_applicationSettings)} is NULL");
            }
        }
        private void WriteSettings(SiteFolderSettings siteSetting)
        {
            if (ExeDirectoryInfo != null && siteSetting != null)
            {
                var serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.TypeNameHandling = TypeNameHandling.Auto;
                serializer.Formatting = Formatting.Indented;

                using (var file = File.CreateText(ExeDirectoryInfo.Parent + @"\" + Constants.FileNames.ExampleJsonFileName))
                using (var writer = new JsonTextWriter(file))
                {
                    serializer.Serialize(writer, siteSetting, typeof(SiteFolderSettings));
                }
            }
        }
    }
}