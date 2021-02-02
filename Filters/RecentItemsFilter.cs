using System;
using System.Collections.Generic;
using System.IO;
using HistoryFilter.Util;
using log4net;

namespace HistoryFilter.Filters {
    class RecentItemsFilter : IFilter {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RecentItemsFilter));
        private readonly Shell32.Shell _shell = new Shell32.Shell();
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
                var target = LinkUtil.GetLnkTarget(_shell, file);

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
