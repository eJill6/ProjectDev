using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Helpers
{
    public static class GlobalCacheHelper
    {
        public static string StompServiceUrl { get; internal set; }
        public static int DefaultCacheExpireDays { get; internal set; } = 30;
    }
}