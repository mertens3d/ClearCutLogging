using ClearCut.Main.Helpers;
using ClearCut.Support.Witness.Watchers;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ClearCut.Main.Views
{
    public class ButtonsHandler
    {
        private ResultsList _userControl;

        private ExternalProgramHelpers _externalProgramHelper;

        private SiteFolderWatcher SiteWatcher;

        public ButtonsHandler(SiteFolderWatcher siteWatcher, ResultsList resultsListUserControl)
        {
            this.SiteWatcher = siteWatcher;
            _userControl = resultsListUserControl;
            _externalProgramHelper = new ExternalProgramHelpers();
        }

        public void SetButtonColor()
        {
            Style btnStyle;
            if (!SiteWatcher.IsEnabled)
            {
                btnStyle = _userControl.FindResource("btnOff") as Style;
            }
            else
            {
                btnStyle = _userControl.FindResource("btnOn") as Style;
            }
            if (btnStyle != null)
            {
                _userControl.idSuspendBtn.Style = btnStyle;
            }
        }

        internal void HandleClickClearLog(Button sender)
        {
            string logFileNameFull = sender.Tag.ToString();
            if (!string.IsNullOrEmpty(logFileNameFull))
            {
                var targetFile = new FileInfo(logFileNameFull);
                if (targetFile.Exists)
                {
                    try
                    {
                        File.WriteAllText(targetFile.FullName, String.Empty);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error clearing. The file may be locked");

                        //it's probably locked
                    }
                }
            }
        }

        internal void HandleNotepadPPAutoLoadClick(CheckBox sender, bool checkBoxState)
        {
            string tagValue = sender.Tag.ToString();
            if (!string.IsNullOrEmpty(tagValue))
            {
                //var bareTailArgs = "\"" + tagValue + "\"";
                //var bareTailProgram = @"C:\BareTail\baretail.exe";
                //System.Diagnostics.Process.Start(bareTailProgram, bareTailArgs);
                //MessageBox.Show(tagValue + " " + checkBoxState.ToString());
            }
        }

        internal void HandleBareTailClick(Button sender)
        {
            string tagValue = sender.Tag.ToString();

            if (!string.IsNullOrEmpty(tagValue))
            {
                _externalProgramHelper.OpenInBareTail(tagValue);
            }
        }

        internal void HandleVSCodeClick(Button sender)
        {
            string tagValue = sender.Tag.ToString();
            if (!string.IsNullOrEmpty(tagValue))
            {
                _externalProgramHelper.OpenInVSCode(tagValue);
            }
        }

        internal void HandleNotepadPPClick(Button sender)
        {
            string tagValue = sender.Tag.ToString();
            if (!string.IsNullOrEmpty(tagValue))
            {
                _externalProgramHelper.OpenInNotePadCPP(tagValue);
            }
        }
    }
}