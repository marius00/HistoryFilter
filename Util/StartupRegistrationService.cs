using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HistoryFilter.Util {
    static class StartupRegistrationService {


        public static bool IsInstalled() {
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name + ".exe";
            string startUpFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var files = Directory.EnumerateFiles(startUpFolderPath).Where(m => m.EndsWith(".lnk"));
            foreach (var file in files) {
                var target = GetLnkTarget(file);
                if (target.EndsWith(assemblyName))
                    return true;
            }

            return false;
        }

        public static void Install() {
            string startUpFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var shortcut = Path.Combine(startUpFolderPath, "HistoryFilter.lnk");

            IWshRuntimeLibrary.IWshShell wsh = new IWshRuntimeLibrary.WshShellClass();
            IWshRuntimeLibrary.IWshShortcut sc = (IWshRuntimeLibrary.IWshShortcut)wsh.CreateShortcut(shortcut);
            sc.TargetPath = Application.ExecutablePath;
            sc.WorkingDirectory = Application.StartupPath;
            sc.Description = "Start HistoryFilter on system start";
            sc.Save();
        }

        public static void Uninstall() {
            string startUpFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var shortcut = Path.Combine(startUpFolderPath, "HistoryFilter.lnk");
            File.Delete(shortcut);
        }


        private static string GetLnkTarget(string lnkPath) {
            var shl = new Shell32.Shell();         // Move this to class scope
            lnkPath = Path.GetFullPath(lnkPath);
            var dir = shl.NameSpace(Path.GetDirectoryName(lnkPath));
            var itm = dir.Items().Item(Path.GetFileName(lnkPath));
            var lnk = (Shell32.ShellLinkObject)itm.GetLink;
            return lnk.Target.Path;
        }
    }
}
