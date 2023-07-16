using JxBackendService.Interface.Service.Util;
using JxBackendServiceN6.Common.Util;

namespace JxBackendServiceN6.Service.Util
{
    public class LogUtilService : ILogUtilService
    {
        public void Debug<T>(T value)
        {
            LogUtil.Debug(value);
        }

        public void Debug<T>(string loggerName, T value)
        {
            LogUtil.Debug(loggerName, value);
        }

        public void Error<T>(T value)
        {
            LogUtil.Error(value);
        }

        public void Error<T>(string loggerName, T value)
        {
            LogUtil.Error(loggerName, value);
        }

        public void ForcedDebug<T>(T value)
        {
            LogUtil.ForcedDebug(value);
        }

        public void ForcedDebug<T>(string loggerName, T value)
        {
            LogUtil.ForcedDebug(loggerName, value);
        }

        public void Info<T>(T value)
        {
            LogUtil.Info(value);
        }

        public void Info<T>(string loggerName, T value)
        {
            LogUtil.Info(loggerName, value);
        }

        public void Warn<T>(T value)
        {
            LogUtil.Warn(value);
        }

        public void Warn<T>(string loggerName, T value)
        {
            LogUtil.Warn(loggerName, value);
        }
    }
}