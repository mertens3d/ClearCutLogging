namespace ClearCut.Main.Helpers
{
    public class ExternalProgramLauncher
    {
        public void OpenInGeneric(string tagValue) {

            var args = "\"" + tagValue + "\"";
            var executable = @"C:\BareTail\baretail.exe";
            System.Diagnostics.Process.Start(executable, args);
        }
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
            //var notepadPPProgram = @"C:\Program Files (x86)\Notepad++\notepad++.exe";


            System.Diagnostics.Process.Start(notepadPPProgram, notepadPPArgs);
        }
        public void OpenInVSCode(string tagValue)
        {
            //"C:\Users\xxxx\AppData\Local\Programs\Microsoft VS Code\Code.exe"
            var vsCodeArgs = "\"" + tagValue + "\"";
            var vsCodeProgram = @"Code.exe";

            System.Diagnostics.Process.Start(vsCodeProgram, vsCodeArgs);
        }
    }
}