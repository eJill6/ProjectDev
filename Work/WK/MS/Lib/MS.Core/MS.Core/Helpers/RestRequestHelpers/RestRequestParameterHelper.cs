using MS.Core.Extensions;
using MS.Core.Utils;
using RestSharp;
using RestSharp.Serializers;
using System.Reflection;

namespace MS.Core.Helpers.RestRequestHelpers
{
    public class RestRequestParameterHelper
    {
        RestRequest http = null;
        public RestRequestParameterHelper(RestRequest http)
        {
            this.http = http;
        }
        #region Header設定

        public RestRequestParameterHelper AddHeader(string key, string value)
        {
            http.AddHeader(key, value);

            return this;
        }
        public RestRequestParameterHelper AddHeader(Dictionary<string, string> headers = null)
        {
            if (headers != null)
            {
                http.AddHeaders(headers);
            }

            return this;
        }
        #endregion
        #region Parameter設定

        /// <summary>
        /// key,value設定參數
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public RestRequestParameterHelper AddParameter(string key, object value)
        {
            switch (http.Method)
            {
                case Method.GET:
                    {
                        http.AddQueryParameter(key, value.ToString());
                    }
                    break;
                default:
                    {
                        http.AddParameter(key, value);
                    }
                    break;
            }

            return this;
        }
        /// <summary>
        /// TextPlain
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public RestRequestParameterHelper AddTextPlainBody(string body)
        {
            http.AddHeader("Content-Type", "text/plain");
            http.AddParameter("text/plain", body, ParameterType.RequestBody);
            return this;
        }

        public RestRequestParameterHelper AddParameter(object body)
        {
            if (body == null)
            {
                return this;
            }
            switch (http.Method)
            {
                case Method.GET:
                    {
                        foreach (PropertyInfo item in body.GetType().GetProperties())
                        {
                            object? value = item.GetValue(body);
                            
                            if(value is Enum)
                            {
                                value = ((int)value).ToString();
                            }
                            http.AddQueryParameter(item.Name.ToCamelCase(), value?.ToString() ?? string.Empty);

                        }
                    }
                    break;
                default:
                    {
                        if (body == null)
                        {
                            return this;
                        }

                        http.AddParameter("application/json", JsonUtil.ToJsonString(body, isCamelCaseNaming: true), ParameterType.RequestBody);
                    }
                    break;
            }

            return this;
        }

        /// <summary>
        /// 加入檔案內容
        /// </summary>
        /// <param name="name">名稱</param>
        /// <param name="body">檔案內容</param>
        /// <param name="fileName">檔案名稱</param>
        /// <param name="contentType">檔案類別</param>
        /// <returns>helper</returns>
        public RestRequestParameterHelper AddFileUpload(string name, byte[] body, string fileName, string contentType = "multipart/form-data")
        {
            http.AddFileBytes(name, body, fileName, contentType);
            return this;
        }

        /// <summary>
        /// key,value設定參數
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public RestRequestParameterHelper AddParameter(Dictionary<string, string> parameters = null)
        {
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    AddParameter(item.Key, item.Value);
                }
            }

            return this;
        }

        #endregion
    }

    public class JsonSerializer : ISerializer
    {
        public string ContentType { get; set; } = "application/json";

        public string? Serialize(object obj)
        {
            return JsonUtil.ToJsonString(obj, isCamelCaseNaming: true);
        }
    }

}
