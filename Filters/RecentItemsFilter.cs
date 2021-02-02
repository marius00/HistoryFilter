using System;
using System.Collections.Generic;
using System.IO;
using log4net;

namespace HistoryFilter.Filters {
    class RecentItemsFilter : IFilter {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RecentItemsFilter));
        private readonly Shell32.Shell _shell = new Shell32.Shell();         // Move this to class scope
        private List<string> _masks;

        public RecentItemsFilter(List<string> masks) {
            _masks = masks;
        }

        public List<string> GetMatchingFiles() {
            var path = Path.Combine(
                Environment.GetEnvironmentVariable("AppData"),
                "Microsoft",
                "Windows",
                "Recent"
            );

            List<string> result = new List<string>();
            var files = Directory.EnumerateFiles(path, "*.lnk");
            foreach (var file in files) {
                var target = GetLnkTarget(_shell, file);

                foreach (var filter in _masks) {
                    var doFilter = target.StartsWith(filter, StringComparison.OrdinalIgnoreCase);
                    if (doFilter) {
                        result.Add(file);
                        break;
                    }
                }
            }

            return result;
        }

        private static string GetLnkTarget(Shell32.Shell shell, string lnkPath) {
            lnkPath = Path.GetFullPath(lnkPath);
            var dir = shell.NameSpace(Path.GetDirectoryName(lnkPath));
            var itm = dir.Items().Item(Path.GetFileName(lnkPath));
            var lnk = (Shell32.ShellLinkObject)itm.GetLink;
            
            return lnk.Target.Path;
        }

        public void Purge() {
            if (_masks.Count == 0)
                return;

            foreach (var file in GetMatchingFiles()) {
                try {
                    File.Delete(file);
                }
                catch (Exception ex) {
                    Logger.Warn(ex.Message, ex);
                }
            }
        }

        public void SetMasks(List<string> masks) {
            _masks = masks;
        }
    }
}
