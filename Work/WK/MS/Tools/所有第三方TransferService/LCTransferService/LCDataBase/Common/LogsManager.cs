using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;

namespace LCDataBase.Common
{
    public class LogsManager
    {
        public static void Info(object message)
        {
            var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            logUtilService.Info(message);
        }

        public static void InfoToTelegram(object message)
        {
            var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            logUtilService.Info(message);
        }

        public static void Error(object message)
        {
            var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            logUtilService.Error(message);
        }
    }
}