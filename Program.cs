using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using HistoryFilter.Filters;
using HistoryFilter.Settings;
using HistoryFilter.Util;
using log4net;

namespace HistoryFilter {
    static class Program {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        private static string LocalAppdata {
            get {
                string appdata = System.Environment.GetEnvironmentVariable("LocalAppData");
                if (string.IsNullOrEmpty(appdata))
                    return Path.Combine(System.Environment.GetEnvironmentVariable("AppData"), "..", "local");
                else
                    return appdata;
            }
        }

        public static string CoreFolder {
            get {
                string path = Path.Combine(LocalAppdata, "EvilSoft", "HistoryFilter");
                Directory.CreateDirectory(path);

                return path;
            }
        }

        private static string SettingsFile => Path.Combine(CoreFolder, "settings.json");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Logger.Info("Starting application");

            SettingsReader settingsReader = SettingsReader.Load(SettingsFile);
            var masks = settingsReader.GetSettings.Prefixes ?? new List<string>(0);

            List<IFilter> fileFilters = new List<IFilter> {
                new RecentItemsFilter(masks),
                new StructuredStorageFilter(masks)
            };

            for (int i = 0; i < 3; i++) {
                foreach (var filter in fileFilters) {
                    filter.Purge();
                }
            }

            ExceptionHandler.EnableLogUnhandledOnThread();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(settingsReader.GetSettings, fileFilters));

            Logger.Info("Application ended");
        }
    }
}
