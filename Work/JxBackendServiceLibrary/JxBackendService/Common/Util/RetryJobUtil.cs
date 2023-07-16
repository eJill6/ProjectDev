using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace JxBackendService.Common.Util
{
    public static class RetryJobUtil
    {
        public static void DoJobWithRetry(Action job, int maxTryCount, int retryIntervalSeconds)
        {
            GetJobResultWithRetry<object>(() =>
            {
                job.Invoke();
                return null;
            }, maxTryCount, retryIntervalSeconds);
        }

        public static T GetJobResultWithRetry<T>(Func<T> job, int maxTryCount, int retryIntervalSeconds)
        {
            for (int tryCount = 1; tryCount <= maxTryCount; tryCount++)
            {
                try
                {
                    return job.Invoke();
                }
                catch (Exception ex)
                {
                    if (tryCount == maxTryCount)
                    {
                        throw ex;
                    }

                    Thread.Sleep(retryIntervalSeconds * 1000);
                }
            }

            //會從回圈內拋出實際錯誤, 這邊只是讓編譯通過
            throw new InvalidProgramException();
        }
    }
}
