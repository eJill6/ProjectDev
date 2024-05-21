using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;

namespace SLPolyGame.Web.Common
{
    public static class ModelToJson
    {
        public static string ObjectToJson(object _obj, Type t)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(t);
                ser.WriteObject(ms, _obj);
                byte[] json = ms.ToArray();
                ms.Close();
                string jsonString = Encoding.UTF8.GetString(json, 0, json.Length);
                return jsonString;
            }
            catch (Exception ex)
            {
                return "数据序列化错误：" + ex.Message + "," + ex.StackTrace;
            }
        }

        public static string ListToJson<T>(T data)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(data.GetType());
                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, data);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                return "数据序列化错误：" + ex.Message + "," + ex.StackTrace;
            }
        }
    }
}
