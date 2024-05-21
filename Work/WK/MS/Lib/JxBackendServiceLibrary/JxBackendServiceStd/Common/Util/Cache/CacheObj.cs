using System;

namespace JxBackendService.Common.Util.Cache
{
    /// <summary>相容原本Web使用的儲存快取的model，統一使用此model避免範型判斷的時候誤判</summary>
    public class CacheObj
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public int ExpireSeconds { get; set; }

        public bool IsSliding { get; set; }

        public DateTime InsertTime { get; set; }
    }
}