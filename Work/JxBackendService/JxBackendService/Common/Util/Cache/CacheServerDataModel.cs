using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Common.Util.Cache
{
    public class CacheServerDataModel<T>
    {
        public CacheServerDataModel(T data, int cacheSeconds, bool isSlidingExpiration)
        {
            Data = data;
            CacheSeconds = cacheSeconds;
            ExpiredTime = DateTime.Now.AddSeconds(cacheSeconds);
            IsSlidingExpiration = isSlidingExpiration;
        }

        public DateTime ExpiredTime { get; set; }

        public int CacheSeconds { get; set; }

        public bool IsSlidingExpiration { get; set; }

        public T Data { get; set; }
    }
}
