using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace JxBackendService.Common.Util
{
    public static class JsonUtil
    {
        private static readonly DefaultContractResolver _camelCaseNamingResolver = new CamelCasePropertyNamesContractResolver();

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

        public static object Deserialize(this string json, Type type)
        {
            if (json.IsNullOrEmpty())
            {
                return null;
            }

            return JsonConvert.DeserializeObject(json, type);
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

        public static string ToJsonString<T>(this T source, bool ignoreNull = false, bool ignoreDefault = false,
            bool isCamelCaseNaming = false, bool isFormattingNone = false)
        {
            if (source == null)
            {
                return null;
            }

            var ignoreNullSetting = ignoreNull ? NullValueHandling.Ignore : NullValueHandling.Include;
            var ignoreDefaultSetting = ignoreDefault ? DefaultValueHandling.Ignore : DefaultValueHandling.Include;

            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = ignoreNullSetting,
                DefaultValueHandling = ignoreDefaultSetting,
            };

            if (isCamelCaseNaming)
            {
                jsonSerializerSettings.ContractResolver = _camelCaseNamingResolver;
            }

            if (isFormattingNone)
            {
                jsonSerializerSettings.Formatting = Formatting.None;
            }

            return JsonConvert.SerializeObject(source, jsonSerializerSettings);
        }
    }
}