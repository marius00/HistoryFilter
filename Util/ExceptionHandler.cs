using System;
using log4net;

namespace HistoryFilter.Util {
    static class ExceptionHandler {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ExceptionHandler));
        public static void EnableLogUnhandledOnThread() {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
        }

        private static void MyHandler(object sender, UnhandledExceptionEventArgs args) {
            Exception e = (Exception)args.ExceptionObject;
            Logger.Error(e.Message);
            Logger.Error(e.StackTrace);
        }
    }
}
