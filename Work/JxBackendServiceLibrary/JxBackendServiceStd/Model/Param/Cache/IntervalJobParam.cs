using System;
using JxBackendService.Common.Util.Cache;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Model.Param.Cache
{
    public class IntervalJobParam
    {
        public CacheKey CacheKey { get; set; }

        /// <summary> cache存活秒數 </summary>
        public double CacheSeconds { get; set; }

        /// <summary> 設定最終暫停的秒數 </summary>
        public int? SuspendSeconds { get; set; }

        /// <summary> 當發生exception時是否要暫停功能 </summary>
        public bool IsSuspendWhenException { get; set; }

        /// <summary> cache存在秒數內，最多錯誤次數 </summary>
        public int? MaxExceptionTryCount { get; set; }

        public bool IsDoErrorHandle { get; set; }

        /// <summary> cache存在秒數內，最多可正常請求次數 </summary>
        public int? MaxNormalTryCount { get; set; }

        public EnvironmentUser EnvironmentUser { get; set; }
    }

    public class IntervalCacheResult
    {
        public int NormalTryCount { get; set; }

        public int ExceptionTryCount { get; set; }

        public DateTime? SuspendDate { get; set; }
    }
}