using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Common.Util
{
    public static class JsonUtil
    {

        public static string Serialize<T>(T source)
        {
            if (source == null)
            {
                return string.Empty;
            }

            return JsonConvert.SerializeObject(source);
        }

        //反序列化的辅助类
        public static T Deserialize<T>(this string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T CloneByJson<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (source == null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }

        public static T CastByJson<T>(this object value) where T : new()
        {
            if (value == null)
            {
                return default(T);
            }

            return Deserialize<T>(value.ToJsonString());
        }

        public static string ToJsonString<T>(this T source, bool ignoreNull = false, bool ignoreDefault = false)
        {
            if (source == null)
            {
                return null;
            }

            var ignoreNullSetting = ignoreNull ? Newtonsoft.Json.NullValueHandling.Ignore : Newtonsoft.Json.NullValueHandling.Include;
            var ignoreDefaultSetting = ignoreDefault ? Newtonsoft.Json.DefaultValueHandling.Ignore : Newtonsoft.Json.DefaultValueHandling.Include;

            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = ignoreNullSetting,
                DefaultValueHandling = ignoreDefaultSetting,
            };

            return JsonConvert.SerializeObject(source, jsonSerializerSettings);
        }
    }
}
