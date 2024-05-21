using JxBackendService.Common.Util.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLPolyGame.Web.Helpers
{
    public static class CacheKeyHelper
    {
        public static string GetUserTokenKey(string key) => CacheKey.GetFrontSideUserInfoKey(key).Value;
    }
}