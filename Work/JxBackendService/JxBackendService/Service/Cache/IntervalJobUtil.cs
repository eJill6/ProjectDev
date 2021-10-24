using System;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Param.Cache;

namespace JxBackendService.Service.Cache
{
    public static class IntervalJobUtil
    {
        public static void DoIntervalWork(IntervalJobParam intervalJobParam, Action action)
        {
            DoIntervalWork(intervalJobParam, () =>
            {
                action.Invoke();
                return true;
            });
        }

        public static void DoIntervalWork(IntervalJobParam intervalJobParam, Action action, Action afterSuspend)
        {
            DoIntervalWork(intervalJobParam, () =>
            {
                action.Invoke();
                return true;
            }, afterSuspend);
        }

        public static T DoIntervalWork<T>(IntervalJobParam intervalJobParam, Func<T> work)
        {
            return DoIntervalWork(intervalJobParam, work, null);
        }

        public static T DoIntervalWork<T>(IntervalJobParam intervalJobParam, Func<T> work, Action afterSuspend)
        {
            var jxCacheService = DependencyUtil.ResolveServiceForModel<IJxCacheService>(intervalJobParam.EnvironmentUser.Application);

            // 取得cache設定值
            IntervalCacheResult currentResult = jxCacheService.GetCache<IntervalCacheResult>(new SearchCacheParam()
            {
                Key = intervalJobParam.CacheKey,
                CacheSeconds = intervalJobParam.CacheSeconds,
                IsCloneInstance = false
            });

            bool isSuspend = IsSuspend(intervalJobParam, currentResult);

            if (isSuspend)
            {
                if (intervalJobParam.SuspendSeconds.HasValue && !currentResult.SuspendDate.HasValue)
                {
                    // 設定暫停使用功能的時間性
                    currentResult.SuspendDate = DateTime.Now.AddSeconds(intervalJobParam.SuspendSeconds.Value);
                    jxCacheService.SetCache(new SetCacheParam()
                    {
                        Key = intervalJobParam.CacheKey,
                        CacheSeconds = intervalJobParam.SuspendSeconds.Value
                    }, currentResult);

                    if (afterSuspend != null)
                    {
                        afterSuspend.Invoke();
                    }
                }

                return default(T);
            }

            try
            {
                T returnValue = work.Invoke();

                // 成功執行,清除cache
                if (intervalJobParam.IsSuspendWhenException)
                {
                    if (currentResult != null)
                    {
                        currentResult.ExceptionTryCount = 0;
                    }
                }

                jxCacheService.SetCacheForInterval(intervalJobParam.CacheKey, intervalJobParam.CacheSeconds, currentResult, true);

                return returnValue;
            }
            catch (Exception ex)
            {
                // 設定有例外時需設定暫停功能時，寫入cache限制
                if (intervalJobParam.IsSuspendWhenException)
                {
                    jxCacheService.SetCacheForInterval(intervalJobParam.CacheKey, intervalJobParam.CacheSeconds, currentResult, false);
                }

                if (intervalJobParam.IsDoErrorHandle)
                {
                    ErrorMsgUtil.ErrorHandle(ex, intervalJobParam.EnvironmentUser);
                    return default(T);
                }

                throw ex;
            }
        }

        private static bool IsSuspend(IntervalJobParam intervalJobParam, IntervalCacheResult currentResult)
        {
            if (currentResult == null)
            {
                return false;
            }
            // 確認當下時間，是否還在限制的期間
            if (currentResult.SuspendDate.HasValue && DateTime.Now <= currentResult.SuspendDate)
            {
                return true;
            }
            // 是否設定最大例外次數和最多正常呼叫次數，若都沒設定就進行暫停
            if (!intervalJobParam.MaxExceptionTryCount.HasValue && !intervalJobParam.MaxNormalTryCount.HasValue)
            {
                return true;
            }
            // 是否超過設定Exception次數
            if (intervalJobParam.MaxExceptionTryCount.HasValue &&
                currentResult.ExceptionTryCount >= intervalJobParam.MaxExceptionTryCount.Value)
            {
                return true;
            }
            // 是否超過一定時間內限制次數
            if (intervalJobParam.MaxNormalTryCount.HasValue &&
                currentResult.NormalTryCount >= intervalJobParam.MaxNormalTryCount.Value)
            {
                return true;
            }

            return false;
        }

        private static void SetCacheForInterval(this IJxCacheService jxCacheService, CacheKey cacheKey, int cacheSeconds, IntervalCacheResult currentResult, bool isNormalTry)
        {
            if (currentResult == null)
            {
                currentResult = new IntervalCacheResult();

                if (isNormalTry)
                {
                    // 正常呼叫次數累加
                    currentResult.NormalTryCount++;
                }
                else
                {
                    // 例外錯誤次數累加
                    currentResult.ExceptionTryCount++;
                }

                jxCacheService.SetCache(new SetCacheParam()
                {
                    Key = cacheKey,
                    CacheSeconds = cacheSeconds
                }, currentResult);
            }
            else
            {
                //因為這邊使用memory,所以可以直接操作物件,重新set的話會讓cache生命延長
                if (isNormalTry)
                {
                    currentResult.NormalTryCount++;
                }
                else
                {
                    currentResult.ExceptionTryCount++;
                }
            }
        }
    }
}
