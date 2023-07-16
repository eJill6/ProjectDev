using NLog;

namespace JxBackendServiceN6.Common.Util
{
    public static class LogUtil
    {
        public static void Debug<T>(T value)
        {
            Debug(null, value);
        }

        public static void Debug<T>(string? loggerName, T value)
        {
            GetLogger(loggerName).Debug(value);
        }

        public static void ForcedDebug<T>(T value)
        {
            ForcedDebug(null, value);
        }

        public static void ForcedDebug<T>(string? loggerName, T value)
        {
            string content = null;
            
            if (value != null)
            {
                content = $"|{nameof(ForcedDebug)}|{value.ToString()}";
            }

            //避免運維把Error等級關掉，將等級Error改為Fatal
            GetLogger(loggerName).Fatal(content);
        }

        public static void Info<T>(T value)
        {
            Info(null, value);
        }

        public static void Info<T>(string? loggerName, T value)
        {
            GetLogger(loggerName).Info(value);
        }

        public static void Warn<T>(T value)
        {
            Warn(null, value);
        }

        public static void Warn<T>(string? loggerName, T value)
        {
            GetLogger(loggerName).Warn(value);
        }

        public static void Error<T>(T value)
        {
            Error(null, value);
        }

        public static void Error<T>(string? loggerName, T value)
        {
            //避免運維把Error等級關掉，將等級Error改為Fatal
            GetLogger(loggerName).Fatal(value);
        }

        //public static void Error(Exception ex)
        //{
        //    GetLogger().Error(ex);
        //}

        private static Logger GetLogger(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return LogManager.GetCurrentClassLogger();
            }
            else
            {
                return LogManager.GetLogger(name);
            }
        }

    }
}