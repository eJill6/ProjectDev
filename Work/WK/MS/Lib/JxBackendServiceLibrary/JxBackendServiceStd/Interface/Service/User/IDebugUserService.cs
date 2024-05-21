using System;

namespace JxBackendService.Interface.Service.User
{
    public interface IDebugUserService
    {
        void ForcedDebug(int userId, string debugContent);

        bool IsDebugUser(int userId);

        T LogExecuteMethodInfo<T>(int userId, string methodName, string param, Func<T> doWork);
    }
}