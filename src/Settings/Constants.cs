namespace ClearCut.Support.Settings
{
    public struct Constants
    {
        public struct FileNames
        {
            public static string AppSettingsFileNameShort = "ClearCut.AppSettings.json";
            public static string ExampleJsonFileName = "ClearCut.SiteSettings.Example.json";
            public static string SettingsFilter = "ClearCut.SiteSettings.*.json";
        }

        public struct Settings
        {
            public struct Defaults
            {
                public static string WatchWordInfo = "INFO";
                public static double WindowHeight = 250;
                public static double WindowWidth = 800;
            }
        }

        public struct Ui
        {
            public static string TitlePrefix = "Clear Cut Log Watcher : ";
        }
    }
}