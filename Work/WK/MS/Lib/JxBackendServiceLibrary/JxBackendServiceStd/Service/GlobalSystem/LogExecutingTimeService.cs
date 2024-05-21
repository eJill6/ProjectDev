using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Diagnostics;

namespace JxBackendService.Service.GlobalSystem
{
    public class LogExecutingTimeService : BaseService, ILogExecutingTimeService
    {
        private readonly Stopwatch _stopWatch;

        public LogExecutingTimeService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _stopWatch = new Stopwatch();
        }

        public void Start()
        {
            _stopWatch.Start();
        }

        public void Stop(string logMessageTemplate, double? warningMilliseconds)
        {
            double elapsedTotalMilliseconds = StopAndReturnElapsedTotalMilliseconds();
            string logMessage = string.Format(logMessageTemplate, elapsedTotalMilliseconds);
            LogUtilService.ForcedDebug(logMessage);

            DoWarning(elapsedTotalMilliseconds, warningMilliseconds, logMessage);
        }

        public void Stop(Func<double, string> getLogMessage, double? warningMilliseconds)
        {
            double elapsedTotalMilliseconds = StopAndReturnElapsedTotalMilliseconds();
            string logMessage = getLogMessage.Invoke(elapsedTotalMilliseconds);
            LogUtilService.ForcedDebug(logMessage);

            DoWarning(elapsedTotalMilliseconds, warningMilliseconds, logMessage);
        }

        private double StopAndReturnElapsedTotalMilliseconds()
        {
            _stopWatch.Stop();
            return _stopWatch.Elapsed.TotalMilliseconds;
        }

        private void DoWarning(double elapsedTotalMilliseconds, double? warningMilliseconds, string logMessage)
        {
            if (warningMilliseconds.HasValue && elapsedTotalMilliseconds > warningMilliseconds.Value)
            {
                string errorMesage = $"回應時間過長 {logMessage}, WarningMilliseconds={warningMilliseconds}";
                ErrorMsgUtil.ErrorHandle(new TimeoutException(errorMesage), EnvLoginUser);
            }
        }
    }
}