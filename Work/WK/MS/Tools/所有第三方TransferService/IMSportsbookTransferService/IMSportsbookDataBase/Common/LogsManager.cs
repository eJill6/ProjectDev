using JxBackendServiceNF.Common.Util;

namespace IMSportsbookDataBase.Common
{
    public class LogsManager
    {
        public static void Info(object message)
        {
            LogUtil.Info(message);
        }

        public static void InfoToTelegram(object message)
        {
            LogUtil.Info(message);
        }

        public static void Error(object message)
        {
            LogUtil.Error(message);
        }
    }
}