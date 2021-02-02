using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }
}