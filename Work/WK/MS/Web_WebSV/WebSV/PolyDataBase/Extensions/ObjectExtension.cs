using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyDataBase.Extensions
{
    public static class ObjectExtension
    {
        public static string ToJsonString(this object value) => JsonConvert.SerializeObject(value);
    }
}