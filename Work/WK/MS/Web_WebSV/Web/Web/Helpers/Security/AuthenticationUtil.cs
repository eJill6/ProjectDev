using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Route;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.ViewModel;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using Web.Models.Base;
using Web.Services;

namespace Web.Helpers.Security
{
    public static class AuthenticationUtil
    {
        private static readonly string[] _cookieKeyNotToDelete = new string[] { };

        private static readonly string s_authenticationTokenKey = DependencyUtil.ResolveService<IConfigUtilService>().Get("AuthenticationTokenKey", "!QAZXSW@");

        //public static TicketUserData GetLoginUserFromTicket() => GetUserFromTicket(FormsAuthentication.FormsCookieName);

        public static string GetUserKey()
        {
            BasicUserInfo basicUserInfo = GetTokenModel();

            return basicUserInfo.UserKey;
            //string key = string.Empty;
            //var model = GetTokenModel();
            //if (model != null)
            //{
            //    key = model.Key;
            //}

            //return key;
        }

        public static BasicUserInfo GetTokenModel()
        {
            var routeUtilService = DependencyUtil.ResolveService<IRouteUtilService>();
            string token = routeUtilService.GetMiseWebTokenName();

            if (token.IsNullOrEmpty())
            {
                return new BasicUserInfo();
            }

            try
            {
                DESTool tool = new DESTool(s_authenticationTokenKey);
                BasicUserInfo basicUserInfo = tool.DESDeCode(token).Deserialize<BasicUserInfo>();

                return basicUserInfo;
            }
            catch (Exception ex)
            {
                DependencyUtil.ResolveService<ILogUtilService>().Error(ex);

                return new BasicUserInfo();
            }

            //var model = null as TokenModel;
            //var request = HttpContext.Current.Request;

            //HttpCookie tokenCookie = request.Cookies[CookieKeyHelper.Token];

            //if (tokenCookie != null)
            //{
            //    model = TokenProvider.GetToken(tokenCookie.Value);
            //}
            //else
            //{
            //    var header = request.Headers[CookieKeyHelper.HeaderToken];

            //    if (!string.IsNullOrWhiteSpace(header))
            //    {
            //        header = header.Replace(CookieKeyHelper.HeaderRemove, string.Empty);
            //        model = TokenProvider.GetToken(header);
            //    }
            //}

            //return model;
        }

        public static void LogOff()
        {
            FormsAuthentication.SignOut();
            var keyList = HttpContext.Current.Request.Cookies.AllKeys.Except(_cookieKeyNotToDelete);
            var filterKeyList = keyList.Where(x => x.StartsWith("Preference_"));
            keyList = keyList.Except(filterKeyList);
            HttpContext.Current.Response.Cookies.Clear();

            foreach (string key in keyList)
            {
                HttpCookie httpCookie = new HttpCookie(key)
                {
                    Expires = DateTime.Now.AddDays(-1)
                };

                HttpContext.Current.Response.Cookies.Add(httpCookie);
            }

            HttpContext.Current.Session.Clear();
        }

        //public static void SaveFormsAuthentication(TicketUserData ticketUserData)
        //{
        //    HttpResponse httpResponse = HttpContext.Current.Response;

        //    //if (string.IsNullOrEmpty(url))
        //    //{
        //    //    url = httpRequest.Url.AbsoluteUri.Substring(0, httpRequest.Url.AbsoluteUri.IndexOf(httpRequest.RawUrl));
        //    //}

        //    HttpCookie authCookie = FormsAuthentication.GetAuthCookie(ticketUserData.UserName, false);
        //    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

        //    var newTicket = new FormsAuthenticationTicket(
        //        ticket.Version,
        //        ticket.Name,
        //        ticket.IssueDate,
        //        GetTicketExpireDate(),
        //        ticket.IsPersistent,
        //        JsonConvert.SerializeObject(ticketUserData),
        //        ticket.CookiePath);

        //    authCookie.Value = FormsAuthentication.Encrypt(newTicket);
        //    httpResponse.Cookies.Add(authCookie);

        //    HttpCookie uniqueIDCookie = new System.Web.HttpCookie(CookieKeyHelper.UniqueID, Guid.NewGuid().ToString());
        //    httpResponse.Cookies.Add(uniqueIDCookie);//消息监听时使用

        //    var token = TokenProvider.GenerateTokenString(ticketUserData.UserKey, ticketUserData.UserName);
        //    var tokenCookie = new HttpCookie(CookieKeyHelper.Token, token);
        //    httpResponse.Cookies.Add(tokenCookie);

        //    //HttpCookie domainCookie = new HttpCookie(CookieKeyHelper.Domain, System.Web.HttpUtility.UrlEncode(IP.GetDomain(url)));
        //    //httpResponse.Cookies.Add(domainCookie);

        //    //if (!string.IsNullOrEmpty(ver))
        //    //{
        //    //    HttpCookie versionCookie = new HttpCookie(CookieKeyHelper.Version, ver);
        //    //    httpResponse.Cookies.Add(versionCookie);
        //    //}
        //}

        //private static DateTime GetTicketExpireDate() => DateTime.Now.AddMinutes(30);

        //private static TicketUserData GetUserFromTicket(string authCookieName)
        //{
        //    var request = HttpContext.Current.Request;

        //    if (request.IsAuthenticated)
        //    {
        //        if (HttpContext.Current.Items[authCookieName] != null)
        //        {
        //            return HttpContext.Current.Items[authCookieName] as TicketUserData;
        //        }

        //        HttpCookie cookie = request.Cookies[authCookieName];
        //        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
        //        TicketUserData ticketUserData = JsonConvert.DeserializeObject<TicketUserData>(ticket.UserData);

        //        HttpContext.Current.Items[authCookieName] = ticketUserData;

        //        return ticketUserData;
        //    }

        //    return new TicketUserData();
        //}

        public static string CreateMiseWebToken(BasicUserInfo basicUserInfo)
        {
            DESTool tool = new DESTool(s_authenticationTokenKey);
            string token = tool.DESEnCode(basicUserInfo.ToJsonString(isFormattingNone: true));

            return token;
        }

        public static TicketUserData GetLoginUserFromCache()
        {
            ICacheService cacheService = DependencyUtil.ResolveService<ICacheService>();
            string key = string.Format(CacheKeyHelper.UserToken, GetUserKey());

            var ticketUserData = cacheService.GetByRedisRawData<TicketUserData>(key);

            if (ticketUserData == null)
            {
                return new TicketUserData();
            }

            return ticketUserData;
        }
    }
}