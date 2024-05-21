using Autofac;
using JxBackendService.Common.Util.Cache;
using JxBackendService.Model.Enums;
using JxBackendServiceN6.Common.Util;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnitTestN6;

namespace UnitTest.CacheTest
{
    [TestClass]
    public class ConcurrentLockTest : BaseUnitTest
    {
        private static readonly object s_cacheWriteLocker = new object();

        private static readonly string s_lockerKeyFormat = "@#$Locker_{0}";

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<UnitTestMobileApiEnvironmentService>().AsImplementedInterfaces();
        }

        private const int taskCount = 10000;

        private readonly Task[] tasks = new Task[taskCount];

        [TestMethod]
        public void GetLockTest()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //第一次沒有快取，會進入new object並且放入memory cache
            DoGetLocker();
            stopwatch.Stop();

            //等待上方log寫入,避免花費時間的log混雜在中間
            Task.Delay(2000).Wait();
            LogUtil.ForcedDebug($"ElapsedMilliseconds:{stopwatch.ElapsedMilliseconds}");

            stopwatch.Reset();
            stopwatch.Start();

            //第二次會直接拿到memory cache中的lock
            DoGetLocker();
            stopwatch.Stop();

            //等待上方log寫入,避免花費時間的log混雜在中間
            Task.Delay(2000).Wait();
            LogUtil.ForcedDebug($"ElapsedMilliseconds:{stopwatch.ElapsedMilliseconds}");
        }

        private void DoGetLocker()
        {
            for (int i = 0; i < taskCount; i++)
            {
                string key = string.Format(s_lockerKeyFormat, i);

                tasks[i] = Task.Run(() =>
                {
                    GetLocker(key);
                    LogUtil.ForcedDebug($"{DateTime.Now}, {key}");
                });
            }
        }

        private static object GetLocker(string key)
        {
            string lockerCacheKey = string.Format(s_lockerKeyFormat, key);
            object locker = null;

            lock (s_cacheWriteLocker)
            {
                locker = MemoryCacheUtil.GetCache(
                    lockerCacheKey,
                    isCloneInstance: false,
                    isForceRefresh: false,
                    cacheSeconds: 300,
                    isSlidingExpiration: true,
                    getCacheData: () =>
                    {
                        return new object();
                    });
            }

            return locker;
        }
    }
}