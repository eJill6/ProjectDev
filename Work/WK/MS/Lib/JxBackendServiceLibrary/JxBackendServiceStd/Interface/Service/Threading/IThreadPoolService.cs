using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Interface.Service.Threading
{
    public interface IThreadPoolService
    {
        void SetMinThreads(int? minWorkerThreads);
    }
}