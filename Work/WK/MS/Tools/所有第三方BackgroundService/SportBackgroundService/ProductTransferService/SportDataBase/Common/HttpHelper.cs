using System.Text;
using System.Net;
using System.Web;
using JxBackendServiceN6.Service.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;

namespace ProductTransferService.SportDataBase.Common
{
    public class HttpHelper
    {
        public static void SetConnectionLimit(int ConnectionLimit)
        {
            if (ServicePointManager.DefaultConnectionLimit < ConnectionLimit)
                ServicePointManager.DefaultConnectionLimit = ConnectionLimit;
        }

        public static string HttpPost(string url, string parameters)
        {
            HttpWebRequest req;
            SetConnectionLimit(120); // default ConnectionLimit only allow 2 connections at the same time
            if (url.ToLower().StartsWith("https"))
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            req = (HttpWebRequest)WebRequest.Create(url);
            //req.Proxy = null;
            req.Method = "POST";
            req.Timeout = 30000;
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(parameters);

            try
            {
                using (Stream ss = req.GetRequestStream())
                {
                    ss.Write(bytes, 0, bytes.Length);
                    ss.Close();
                }
            }
            catch (Exception ex)
            {
                DependencyUtil.ResolveService<ILogUtilService>().Value.Error(ex);

                return null;
            }

            string returnJson = null;

            try
            {
                using (var rsp = req.GetResponse())
                {
                    using (var ss = rsp.GetResponseStream())
                    {
                        using (var rd = new StreamReader(ss))
                        {
                            returnJson = rd.ReadToEnd();
                            rd.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                DependencyUtil.ResolveService<ILogUtilService>().Value.Error(e);

                return null;
            }

            req = null;
            return returnJson;
        }
    }

    public class ParameterBuilder
    {
        private Dictionary<string, string> parameters = new Dictionary<string, string>();

        public ParameterBuilder Add(string name, string value)
        {
            if (parameters.ContainsKey(name))
            {
                parameters[name] = value;
            }
            else
                parameters.Add(name, value);
            return this;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var kv in parameters)
            {
                if (sb.Length != 0) sb.Append("&");
                sb.AppendFormat("{0}={1}", kv.Key, HttpUtility.UrlEncode(kv.Value));
            }
            return sb.ToString();
        }
    }
}