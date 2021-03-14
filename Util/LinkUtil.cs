using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace HistoryFilter.Util {
    static class LinkUtil {
        public static string GetLnkTarget(Shell32.Shell shell, string lnkPath) {
            lnkPath = Path.GetFullPath(lnkPath);
            var dir = shell.NameSpace(Path.GetDirectoryName(lnkPath));
            var itm = dir.Items().Item(Path.GetFileName(lnkPath));
            var lnk = (Shell32.ShellLinkObject) itm.GetLink;

            return lnk.Target.Path;
        }

        // System.Runtime.InteropServices.COMException (0x8007000F): The system cannot find the drive specified. (Exception from HRESULT: 0x8007000F)
        public static bool IsMissingDriveException(COMException ex) {
            return ex.Message.Contains("0x8007000F");
        }
    }
}