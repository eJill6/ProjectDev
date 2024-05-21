using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace SLPolyGame.Web.Common
{
    public static class JsonUtil
    {
        //JSON序列化的辅助类

        public static string Serialize<T>(T data)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);

            //System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(data.GetType());
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    serializer.WriteObject(ms, data);
            //    return Encoding.UTF8.GetString(ms.ToArray());
            //}
        }

        //反序列化的辅助类
        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);

            //T obj = Activator.CreateInstance<T>();
            //using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            //{
            //    System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            //    return (T)serializer.ReadObject(ms);
            //}
        }
    }
}