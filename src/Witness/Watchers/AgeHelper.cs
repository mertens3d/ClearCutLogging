using Serilog;
using System;
using System.IO;

namespace ClearCut.Support.Witness
{
    public class AgeHelper
    {
        private ILogger Logger;

        public AgeHelper(ILogger logger)
        {
            Logger = logger;
        }

        public TimeSpan CalculateTimeSpan(FileInfo lastFileInfo)
        {
            TimeSpan toReturn = TimeSpan.MaxValue;
            if (lastFileInfo != null && lastFileInfo.Exists)
            {
                var fileDate = lastFileInfo.LastWriteTime;
                var nowDate = DateTime.Now;
                toReturn = nowDate - fileDate;
            }
            else
            {
                Logger.Error("LastFileInfo is not valid");
            }

            return toReturn;
        }
        public string CalculateAge(TimeSpan diff)
        {
            string toReturn = string.Empty;

            if ((int) diff.TotalDays > 1)
            {
                toReturn = (int) diff.TotalDays + " days";
            }
            else if ((int) diff.TotalHours > 1)
            {
                toReturn = (int) diff.TotalHours + " hrs";
            }
            else if ((int) diff.TotalMinutes > 1)
            {
                toReturn = (int) diff.TotalMinutes + " mins";
            }
            else
            {
                toReturn = (int) diff.TotalSeconds + " secs";
            }

            return toReturn;
        }


    }
}