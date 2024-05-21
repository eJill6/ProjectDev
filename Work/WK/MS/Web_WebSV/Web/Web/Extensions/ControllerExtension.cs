using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web.Helpers.Security;
using Web.Models.Base;

namespace Web.Extensions
{
    public static class ControllerExtension
    {
        /// <summary>
        /// Get user Id from authentication cookie.
        /// </summary>
        /// <param name="controller">Current controller.</param>
        /// <returns>User Id.</returns>
        public static T GetUserId<T>(this Controller controller)
        {
            if (controller.Request.IsAuthenticated)
            {
                //var cookie = controller.Request.Cookies[FormsAuthentication.FormsCookieName];
                int userId = AuthenticationUtil.GetTokenModel().UserId;

                return (T)Convert.ChangeType(userId, typeof(T));
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Get user Id from authentication cookie.
        /// </summary>
        /// <param name="controller">Current controller.</param>
        /// <returns>User Id.</returns>
        public static string GetUserIdStr(this Controller controller)
        {
            return GetUserId<string>(controller);
        }

        /// <summary>
        /// Get user Id from authentication cookie.
        /// </summary>
        /// <param name="controller">Current controller.</param>
        /// <returns>User Id.</returns>
        public static int GetUserId(this Controller controller)
        {
            return GetUserId<int>(controller);
        }

        /// <summary>
        /// Get user name.
        /// </summary>
        /// <param name="controller">Current controller.</param>
        /// <returns>User name.</returns>
        public static string GetUserName(this Controller controller)
        {
            if (controller.Request.IsAuthenticated)
            {
                return controller.User.Identity.Name;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetClientIPAddress(this Controller controller)
        {
            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (result != null && result != String.Empty)
            {
                //可能有代理
                if (result.IndexOf(".") == -1)     //没有“.”肯定是非IPv4格式
                    result = null;
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
                        //有“,”，估计多个代理。取第一个不是内网的IP。
                        result = result.Replace(" ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (IsIPAddress(temparyip[i])
                                && temparyip[i].Substring(0, 3) != "10."
                                && temparyip[i].Substring(0, 7) != "192.168"
                                && temparyip[i].Substring(0, 7) != "172.16.")
                            {
                                return temparyip[i];     //找到不是内网的地址
                            }
                        }
                    }
                    else if (IsIPAddress(result)) //代理即是IP格式
                        return result;
                    else
                        result = null;     //代理中的内容 非IP，取IP
                }
            }

            string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (null == result || result == String.Empty)
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (result == null || result == String.Empty)
                result = HttpContext.Current.Request.UserHostAddress;

            return result;
        }

        private static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;
            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }
    }
}