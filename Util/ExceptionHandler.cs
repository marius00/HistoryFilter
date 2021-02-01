using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Repository.Hierarchy;

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
