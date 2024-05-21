using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Extensions
{
    public static class ObjectExtension
    {
        public static string ToJsonString(this object value) => JsonConvert.SerializeObject(value);
    }
}