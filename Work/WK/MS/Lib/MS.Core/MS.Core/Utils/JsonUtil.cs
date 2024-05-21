using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MS.Core.Utils
{
    public static class JsonUtil
    {
        private static readonly DefaultContractResolver _camelCaseNamingResolver = new CamelCasePropertyNamesContractResolver();

        //反序列化的辅助类
        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T CloneByJson<T>(T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (source == null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
        public static T CastByJson<T>(object value) where T : new()
        {
            if (value == null)
            {
                return default(T);
            }

            return Deserialize<T>(ToJsonString(value));
        }

        public static string ToJsonString<T>(T source, bool ignoreNull = false, bool ignoreDefault = false, bool isCamelCaseNaming = false)
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

            return JsonConvert.SerializeObject(source, jsonSerializerSettings);
        }
    }
}
