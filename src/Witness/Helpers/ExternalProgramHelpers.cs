namespace ClearCut.Main.Helpers
{
    public class ExternalProgramHelpers
    {
        public void OpenInBareTail(string tagValue)
        {
            var bareTailArgs = "\"" + tagValue + "\"";
            var bareTailProgram = @"C:\BareTail\baretail.exe";
            System.Diagnostics.Process.Start(bareTailProgram, bareTailArgs);
        }

        public void OpenInNotePadCPP(string tagValue)
        {
            var notepadPPArgs = "-n999999 \"" + tagValue + "\"";
            var notepadPPProgram = @"C:\Program Files\Notepad++\notepad++.exe";
            System.Diagnostics.Process.Start(notepadPPProgram, notepadPPArgs);
        }
    }
}