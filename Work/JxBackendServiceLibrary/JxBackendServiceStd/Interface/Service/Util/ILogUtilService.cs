using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Interface.Service.Util
{
    public interface ILogUtilService
    {
        void Debug<T>(T value);

        void Debug<T>(string loggerName, T value);

        void ForcedDebug<T>(T value);

        void ForcedDebug<T>(string loggerName, T value);

        void Info<T>(T value);

        void Info<T>(string loggerName, T value);

        void Warn<T>(T value);

        void Warn<T>(string loggerName, T value);

        void Error<T>(T value);

        void Error<T>(string loggerName, T value);
    }
}