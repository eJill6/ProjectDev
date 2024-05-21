using System;

namespace JxBackendService.Interface.Service.GlobalSystem
{
    public interface ILogExecutingTimeService
    {
        void Start();

        void Stop(string logMessageTemplate, double? warningMilliseconds);

        void Stop(Func<double, string> getLogMessage, double? warningMilliseconds);
    }
}