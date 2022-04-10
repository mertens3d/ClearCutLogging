using ClearCut.Support.Witness.Watchers;
using System.Windows;
using System.Windows.Controls;

namespace ClearCut.Main.Views
{
    public class ButtonsHandler
    {
        private ResultsList _userControl;
        private SiteWatcher SiteWatcher;
        public ButtonsHandler(SiteWatcher siteWatcher, ResultsList resultsListUserControl)
        {
            this.SiteWatcher = siteWatcher;
            _userControl = resultsListUserControl;
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

        internal void HandleBareTailClick(Button sender)
        {
            string tagValue = sender.Tag.ToString();
            if (!string.IsNullOrEmpty(tagValue))
            {
                var bareTailArgs = "\"" + tagValue + "\"";
                var bareTailProgram = @"C:\BareTail\baretail.exe";
                System.Diagnostics.Process.Start(bareTailProgram, bareTailArgs);
            }
        }

        internal void HandleNotepadPPClick(Button sender)
        {
            string tagValue = sender.Tag.ToString();
            if (!string.IsNullOrEmpty(tagValue))
            {
                var notepadPPArgs = "-n999999 \"" + tagValue + "\""  ;
                var notepadPPProgram = @"C:\Program Files\Notepad++\notepad++.exe";
                System.Diagnostics.Process.Start(notepadPPProgram, notepadPPArgs);
            }
        }
    }
}