using System.Threading.Tasks;

namespace JxBackendService.Common.Util
{
    public static class TaskUtil
    {
        public static void DelayAndWait(int millisecondsDelay)
        {
            Task.Delay(millisecondsDelay).Wait();
        }

        public static T GetAwaiterAndResult<T>(this Task<T> task)
        {
            return task.ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}