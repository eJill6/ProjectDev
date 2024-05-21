using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Helpers
{
    public static class CookieKeyHelper
    {
        public static string UniqueID { get; private set; } = "UniqueID";
        public static string Token { get; private set; } = "Token";
        public static string HeaderToken { get; private set; } = "Authorization";
        public static string HeaderRemove { get; private set; } = "Bearer ";
        public static string Version { get; internal set; } = "Ver";
        public static string Domain { get; internal set; } = "Domain";
    }
}