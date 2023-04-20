namespace ClearCut.Support.Abstractions
{
    public interface ITargetedLogOptions
    {
        public string LogChildDirectory { get; set; }

        public string LogFileFilter { get; set; }

        string LogFriendlyName { get; set; }
    }
}