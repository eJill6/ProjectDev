using IPTool;
using IPToolModel;
using IPToolUnderlying;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Numerics;
using System.Text;
using System.Web;

namespace Web.Helpers.Security
{
    public class CustomIPContext_Model : IPContext_Model
    {
        public IList<string> FakeUsers { get; set; }
    }

    public class IPContextConverter : IConvertor
    {
        public IPContext_Model ConvertHttpContextIPHeaderToIpContext()
        {
            int doWrite;
            if (int.TryParse(ConfigurationManager.AppSettings["AddFakeheaderLog"], out doWrite) && doWrite == 1)
            {
                //DianosisLogHelper.LogInfoDianosis($"IP.GetHttpContextCurrent() - {IP.GetHttpContextCurrent().Request.ServerVariables["HTTP_X_FORWARDED_FOR"]}");
                //DianosisLogHelper.LogInfoDianosis($"HttpContext.Current - {HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]}");
            }

            return new IPContext_Model()
            {
                HTTP_X_FORWARDED_FOR = IP.GetHttpContextCurrent().Request.ServerVariables["X-Forwarded-For"],
                Proxy_Client_IP = IP.GetHttpContextCurrent().Request.Headers["Proxy-Client-IP"],
                WL_Proxy_Client_IP = IP.GetHttpContextCurrent().Request.Headers["WL-Proxy-Client-IP"],
                HTTP_CLIENT_IP = IP.GetHttpContextCurrent().Request.Headers["HTTP_CLIENT_IP"],
                X_Real_IP = IP.GetHttpContextCurrent().Request.Headers["X-Real-IP"],
                REMOTE_ADDR = IP.GetHttpContextCurrent().Request.ServerVariables["REMOTE_ADDR"],
                UserHostAddress = IP.GetHttpContextCurrent().Request.UserHostAddress
            };
        }
    }

    public abstract class AbstractCustomConverter : IConvertor
    {
        public abstract IPContext_Model ConvertHttpContextIPHeaderToIpContext();
    }

    public class CustomConverter : AbstractCustomConverter
    {
        private IConvertor _converter;

        private IPContext_Model iPContext_Model;

        private string _fakerKey = string.Empty;

        public CustomConverter(IConvertor converter, string fakerKey)
        {
            _converter = converter;
            _fakerKey = fakerKey;
        }

        public override IPContext_Model ConvertHttpContextIPHeaderToIpContext()
        {
            iPContext_Model = _converter.ConvertHttpContextIPHeaderToIpContext();

            IPContext_Model result = JsonConvert.DeserializeObject<IPContext_Model>(Encoding.UTF8.GetString(Convert.FromBase64String(FakeHeader.GetFakeHeader(_fakerKey))));

            if (false == string.IsNullOrEmpty(result.HTTP_X_FORWARDED_FOR))
            {
                iPContext_Model.HTTP_X_FORWARDED_FOR = result.HTTP_X_FORWARDED_FOR;
            }

            if (false == string.IsNullOrEmpty(result.Proxy_Client_IP))
            {
                iPContext_Model.Proxy_Client_IP = result.Proxy_Client_IP;
            }

            if (false == string.IsNullOrEmpty(result.WL_Proxy_Client_IP))
            {
                iPContext_Model.WL_Proxy_Client_IP = result.WL_Proxy_Client_IP;
            }

            if (false == string.IsNullOrEmpty(result.HTTP_CLIENT_IP))
            {
                iPContext_Model.HTTP_CLIENT_IP = result.HTTP_CLIENT_IP;
            }

            if (false == string.IsNullOrEmpty(result.X_Real_IP))
            {
                iPContext_Model.X_Real_IP = result.X_Real_IP;
            }

            if (false == string.IsNullOrEmpty(result.REMOTE_ADDR))
            {
                iPContext_Model.REMOTE_ADDR = result.REMOTE_ADDR;
            }

            if (false == string.IsNullOrEmpty(result.UserHostAddress))
            {
                iPContext_Model.UserHostAddress = result.UserHostAddress;
            }

            return iPContext_Model;
        }
    }

    public class FakeHeader
    {
        private static ConcurrentDictionary<string, string> fakeHeaders = new ConcurrentDictionary<string, string>();

        public static string GetFakeHeader(string username)
        {
            string header = string.Empty;
            fakeHeaders.TryGetValue(username, out header);

            return header;
        }

        private static CustomIPContext_Model GetCustomIPContext_Model()
        {
            CustomIPContext_Model customIPContext_Model = null;

            try
            {
                customIPContext_Model = JsonConvert.DeserializeObject<CustomIPContext_Model>(Encoding.UTF8.GetString(
                    Convert.FromBase64String(GetFakeIPHeaderStr())));
            }
            catch (Exception)
            {
                //LogHelper.LogError(ex);
            }

            return customIPContext_Model;
        }

        private static string GetFakeIPHeaderStr()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["FakeIPHeader"]);
        }

        private static bool IsExistFakeIPHeaderAndIsNotNullOrEmpty()
        {
            return false == (string.IsNullOrEmpty(ConfigurationManager.AppSettings["FakeIPHeader"]));
        }

        public static void AddOrUpdateOrRemoveFakeHeader(string username)
        {
            var fakerCookie = IP.GetHttpContextCurrent().Request.Cookies["faker"];
            bool isExistFaker = false == (fakerCookie == null);

            CustomIPContext_Model customIPContext_Model = GetCustomIPContext_Model();

            if (isExistFaker && IsExistFakeIPHeaderAndIsNotNullOrEmpty() && customIPContext_Model.FakeUsers.Contains(username))
            {
                if (fakeHeaders.ContainsKey(fakerCookie.Value))
                {
                    fakeHeaders.TryUpdate(fakerCookie.Value, fakeHeaders[fakerCookie.Value], GetFakeIPHeaderStr());
                }
                else
                {
                    fakeHeaders.TryAdd(fakerCookie.Value, GetFakeIPHeaderStr());
                }
            }
        }

        public static IPContext_Model GetIPContext()
        {
            var fakerCookie = IP.GetHttpContextCurrent().Request.Cookies["faker"];
            bool isExistFaker = false == (fakerCookie == null);

            IConvertor iPContextConverter;
            if (isExistFaker && fakeHeaders.ContainsKey(fakerCookie.Value))
            {
                iPContextConverter = new CustomConverter(new IPContextConverter(), fakerCookie.Value);
            }
            else
            {
                iPContextConverter = new IPContextConverter();
            }

            return iPContextConverter.ConvertHttpContextIPHeaderToIpContext();
        }
    }

    public static class IP
    {
        public static HttpContext GetHttpContextCurrent()
        {
            return HttpContext.Current;
        }

        private static void WriteLog(string message)
        {
            int doWrite;
            if (int.TryParse(ConfigurationManager.AppSettings["AddFakeheaderLog"], out doWrite) && doWrite == 1)
            {
                //DianosisLogHelper.LogInfoDianosis(message);
            }
        }

        /// <summary>
        /// 可参照资料
        /// https://www.cnblogs.com/diaosir/p/6890825.html
        /// https://blog.csdn.net/fengwind1/article/details/51992528
        /// </summary>
        /// <returns></returns>
        public static string GetDoWorkIP()
        {
            IPHelper ipHelper = new IPHelper();

            string errMessage = string.Empty;
            string ipMessage = string.Empty;
            string result = string.Empty;
            try
            {
                result = ipHelper.GetDoWorkIP(FakeHeader.GetIPContext(), ref errMessage, ref ipMessage);
                //                WriteLog($@"CustomTrace:{StackTracer.GetCustomFunctionInfo()},username:{GetHttpContextCurrent().User.Identity.Name ?? ""} act {nameof(GetDoWorkIP)}: {nameof(result)}-{result}
                //,{nameof(errMessage)}-{errMessage}
                //,{nameof(ipMessage)}-{ipMessage}");
            }
            catch (Exception)
            {
                //LogHelper.LogError(ex);
            }

            return result;
        }

        public static string GetFirstNotPrivateIP()
        {
            IPHelper ipHelper = new IPHelper();

            string errMessage = string.Empty;
            string ipMessage = string.Empty;
            string result = string.Empty;
            try
            {
                result = ipHelper.GetFirstNotPrivateIP(FakeHeader.GetIPContext(), ref errMessage, ref ipMessage);
                //                WriteLog($@"CustomTrace:{StackTracer.GetCustomFunctionInfo()},username:{GetHttpContextCurrent().User.Identity.Name ?? ""} act {nameof(GetFirstNotPrivateIP)}: {nameof(result)}-{result}
                //,{nameof(errMessage)}-{errMessage}
                //,{nameof(ipMessage)}-{ipMessage}");
            }
            catch (Exception)
            {
                //LogHelper.LogError(ex);
            }

            return result;
        }

        private static string ConvertIPToIPInformationBase64String(string aIP)
        {
            string result = string.Empty;
            IPInformation information = null;
            try
            {
                information = GetIPInformation(aIP);
                result = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(information)));
                //WriteLog($@"CustomTrace:{StackTracer.GetCustomFunctionInfo()},{nameof(GetDoWorkIPInformation)}: {nameof(result)}-{result}");
            }
            catch (Exception)
            {
                //LogHelper.LogError(ex);
            }
            finally
            {
                information = null;
            }

            return result;
        }

        public static string GetDoWorkIPInformation()
        {
            string aIP = GetDoWorkIP();
            string result = ConvertIPToIPInformationBase64String(aIP);

            return result;
        }

        public static string GetFirstNotPrivateIPInformation()
        {
            string aIP = GetFirstNotPrivateIP();
            string result = ConvertIPToIPInformationBase64String(aIP);

            return result;
        }

        public static bool IsIPAddress(string ip)
        {
            return IPHelper.IsIPAddress(ip);
        }

        /// <summary>
        /// 將ip 轉為ip number
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static BigInteger? ConvertIPToIPNumber(string ip)
        {
            return IPHelper.ConvertIPToIPNumber(ip);
        }

        /// <summary>
        /// 正規化ipv6表示為長格式
        /// </summary>
        /// <param name="ip"></param>
        public static string IPv6Normalization(string ip)
        {
            return IPHelper.IPv6Normalization(ip);
        }

        public static bool IsIPv4(string ip)
        {
            return IPHelper.IsIPv4(ip);
        }

        public static bool IsIPv6(string ip)
        {
            return IPHelper.IsIPv6(ip);
        }

        public static IPInformation GetIPInformation(string ip)
        {
            IPInformation ipInformation = IPHelper.GetIPInformation(ip);

            return ipInformation;
        }

        public static string GetHost(string url)
        {
            try
            {
                Uri myUri = new Uri(url);
                return myUri.Host;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetDomain(string url)
        {
            try
            {
                Uri myUri = new Uri(url);

                return myUri.Scheme + "://" + myUri.Host + ":" + myUri.Port.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetBaseDomain(string url)
        {
            url = GetHost(url);

            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }
            else if (url.ToLower() == "localhost")
            {
                return "127.0.0.1";
            }
            else if (IP.IsIPAddress(url))
            {
                return url;
            }
            else
            {
                List<string> list = new List<string>(".com|.co|.info|.net|.org|.me|.mobi|.us|.biz|.xxx|.ca|.co.jp|.com.cn|.net.cn|.org.cn|.mx|.tv|.ws|.ag|.com.ag|.net.ag|.org.ag|.am|.asia|.at|.be|.com.br|.net.br|.bz|.com.bz|.net.bz|.cc|.com.co|.net.co|.nom.co|.de|.es|.com.es|.nom.es|.org.es|.eu|.fm|.fr|.gs|.in|.co.in|.firm.in|.gen.in|.ind.in|.net.in|.org.in|.it|.jobs|.jp|.ms|.com.mx|.nl|.nu|.co.nz|.net.nz|.org.nz|.se|.tc|.tk|.tw|.com.tw|.idv.tw|.org.tw|.hk|.co.uk|.me.uk|.org.uk|.vg".Split('|'));
                string[] hs = url.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                if (hs.Length > 2)
                {
                    //传入的host地址至少有三段
                    int p2 = url.LastIndexOf('.');                 //最后一次“.”出现的位置
                    int p1 = url.Substring(0, p2).LastIndexOf('.');//倒数第二个“.”出现的位置
                    string s1 = url.Substring(p1);
                    if (!list.Contains(s1))
                        return s1.TrimStart('.');

                    //域名后缀为两段（有用“.”分隔）
                    if (hs.Length > 3)
                        return url.Substring(url.Substring(0, p1).LastIndexOf('.'));
                    else
                        return url.TrimStart('.');
                }
                else if (hs.Length == 2)
                {
                    return url.TrimStart('.');
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}