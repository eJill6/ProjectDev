using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Threading;
using JxBackendService.Interface.Service.Util;
using System;
using System.Threading;

namespace JxBackendService.Service.Threading
{
    public class ThreadPoolService : IThreadPoolService
    {
        private readonly Lazy<ILogUtilService> _logUtilService;

        public ThreadPoolService()
        {
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        public void SetMinThreads(int? minWorkerThreads)
        {
            ThreadPool.GetMinThreads(out int workerThreads, out int completionPortThreads);
            _logUtilService.Value.ForcedDebug($"Get Origin MinThreads Result, workerThreads={workerThreads},completionPortThreads={completionPortThreads}");

            if (!minWorkerThreads.HasValue)
            {
                _logUtilService.Value.ForcedDebug($"Use Default MinThreads Result, workerThreads={workerThreads},completionPortThreads={completionPortThreads}");

                return;
            }

            ThreadPool.SetMinThreads(minWorkerThreads.Value, completionPortThreads);
            ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
            _logUtilService.Value.ForcedDebug($"Get Modified MinThreads Result, workerThreads={workerThreads},completionPortThreads={completionPortThreads}");
        }
    }
}