using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using HistoryFilter.Util;
using log4net;

namespace HistoryFilter.Filters {
    class RecentItemsFilter : IFilter {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RecentItemsFilter));
        private readonly Shell32.Shell _shell = new Shell32.Shell();
        private List<string> _masks;
        private bool _filterMissingDrives;

        public RecentItemsFilter(List<string> masks, bool filterMissingDrives) {
            _masks = masks;
            _filterMissingDrives = filterMissingDrives;
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
                // Typically happens with pendrives. Maybe we should treat this as a "Yes, please filter"? 
                // Have a separate option to always delete no
                if (!File.Exists(file)) {
                    Logger.Debug($"File {file} does not exist, skipping..");
                    continue;
                }

                string target;
                try {
                    target = LinkUtil.GetLnkTarget(_shell, file);
                }
                catch (COMException ex) {
                    if (LinkUtil.IsMissingDriveException(ex)) {
                        if (_filterMissingDrives) {
                            result.Add(file);
                        }

                        continue;
                    }
                    else {
                        throw;
                    }
                }

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

        public void SetFilterMissingDrives(bool doFilter) {
            _filterMissingDrives = doFilter;
        }
    }
}
