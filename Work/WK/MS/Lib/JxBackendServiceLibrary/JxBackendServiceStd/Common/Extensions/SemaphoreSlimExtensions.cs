using System;
using System.Threading;

namespace JxBackendService.Common.Extensions
{
    public static class SemaphoreSlimExtensions
    {
        public static void DoJob(this SemaphoreSlim semaphoreSlim, Action job)
        {
            try
            {
                semaphoreSlim.Wait();

                job.Invoke();
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public static T DoJob<T>(this SemaphoreSlim semaphoreSlim, Func<T> job)
        {
            try
            {
                semaphoreSlim.Wait();

                return job.Invoke();
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}