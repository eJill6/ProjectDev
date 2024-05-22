using JxBackendService.Common.Util;
using JxBackendServiceNF.Common.Util;

namespace AgDataBase.Common
{
    public static class LogsManager
    {
        public static void Info(object message)
        {
            LogUtil.Info(message);
        }

        public static void Error(object message)
        {
            LogUtil.Error(message);
        }
    }
}