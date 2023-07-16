using IPTool;
using IPToolModel;
using System;
using System.Numerics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace JxBackendService.Common.Util
{
    public static class IpUtil
    {
        /// <summary>
        /// Ip转化为数字
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static BigInteger? IpToNum(string ip)
        {
            BigInteger? numResult = null;
            if (IsValidIPAddress(ip))
            {
                numResult = IPHelper.ConvertIPToIPNumber(ip);
            }
            return numResult;
        }

        private static string GetHeader(string headerName, string namespaceName = "")
        {
            string headerValue = string.Empty;

            if (OperationContext.Current == null)
            {
                return headerValue;
            }

            MessageHeaders messageHeadersElement = OperationContext.Current.IncomingMessageHeaders;
            int isHeaderExist = messageHeadersElement.FindHeader(headerName, namespaceName);

            if (isHeaderExist > -1)
            {
                headerValue = messageHeadersElement.GetHeader<string>(headerName, namespaceName).Trim();
            }

            return headerValue;
        }

        public static string GetOperationContextIP()
        {
            string contextIP = string.Empty;

            //提供方法执行的上下文环境
            OperationContext context = OperationContext.Current;

            if (context == null)
            {
                return contextIP;
            }

            //获取传进的消息属性
            MessageProperties properties = context.IncomingMessageProperties;
            //获取消息发送的远程终结点IP和端口
            RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            contextIP = endpoint.Address;

            return contextIP;
        }

        public static string GetIPAddress(string headerName, string namespaceName = "")
        {
            string currentIP = GetHeader(headerName, namespaceName);

            if (string.IsNullOrEmpty(currentIP))
            {
                currentIP = GetOperationContextIP();
            }

            return currentIP;
        }

        public static IPInformation ConvertIPInformation(string ip)
        {
            IPInformation ipInformation = IPHelper.GetIPInformation(ip);

            return ipInformation;
        }

        /// <summary>給SV取得IP用的</summary>
        public static string GetDoWorkIpAddressFromHeader()
        {
            string ip = GetIPAddress("ip");

            return CheckAndGetIPTransformed(ip);
        }

        public static string GetFirstNotPrivateIP()
        {
            string ip = GetIPAddress("firstNotPrivateIP");

            return CheckAndGetIPTransformed(ip);
        }

        /// <summary>給SV取得IP用的</summary>
        public static IPInformation GetIPInformation(string headerName, string namespaceName = "")
        {
            string ip = GetHeader(headerName);
            //LogsManager.Info($"{nameof(GetIPInformation)}:{ip};");
            IPInformation iPInformation;
            if (false == string.IsNullOrEmpty(ip))
            {
                string strJson = Encoding.UTF8.GetString(Convert.FromBase64String(ip));
                //LogsManager.Info($"{nameof(strJson)},{strJson}");
                iPInformation = strJson.Deserialize<IPInformation>();
                return iPInformation;
            }
            //LogsManager.Info("iPInformation is null");

            ip = CheckAndGetIPTransformed(GetOperationContextIP());
            iPInformation = ConvertIPInformation(ip);

            return iPInformation;
        }

        /// <summary>給SV取得IP用的</summary>
        public static IPInformation GetDoWorkIPInformation()
        {
            IPInformation ip = GetIPInformation("ipInformation");
            //LogsManager.Info(JsonConvert.SerializeObject(ip));
            return ip;
        }

        public static IPInformation GetFirstNotPrivateIPInformation()
        {
            IPInformation ip = GetIPInformation("firstNotPrivateIPInformation");
            //LogsManager.Info(JsonConvert.SerializeObject(ip));
            return ip;
        }

        private static string CheckAndGetIPTransformed(string ip)
        {
            if (ip == "::1")
            {
                return "127.0.0.1";
            }
            else if (IsValidIPAddress(ip))
            {
                return ip;
            }
            else
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 中繼方法，先提供處理ipv6轉ipnum後查位址的方法
        /// </summary>
        /// <returns></returns>
        public static bool IsIPv6(string ip)
        {
            return IPHelper.IsIPv6(ip);
        }

        /// <summary>給web, api(非sv)取得IP用的</summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetDoWorkIP()
        {
            string userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }

            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.UserHostAddress;
            }

            if (!string.IsNullOrEmpty(userHostAddress) && IsIPAddress(userHostAddress))
            {
                return userHostAddress;
            }

            return "127.0.0.1";
        }

        public static object GetAddressSettingInfo()
        {
            return new
            {
                DoWorkIP = GetDoWorkIP(),
                HTTP_X_FORWARDED_FOR = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                Proxy_Client_IP = HttpContext.Current.Request.ServerVariables["Proxy-Client-IP"],
                WL_Proxy_Client_IP = HttpContext.Current.Request.ServerVariables["WL-Proxy-Client-IP"],
                HTTP_CLIENT_IP = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"],
                X_Real_IP = HttpContext.Current.Request.ServerVariables["X-Real-IP"],
                REMOTE_ADDR = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                UserHostAddress = HttpContext.Current.Request.UserHostAddress,
            };
        }

        private static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;
            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }

        private static bool IsValidIPAddress(string ip)
        {
            return IPHelper.IsIPAddress(ip);
        }
    }
}
