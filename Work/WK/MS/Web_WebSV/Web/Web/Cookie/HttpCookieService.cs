using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using System.Web;

namespace Web.Cookie
{
    public class SystemCookie : BaseStringValueModel<SystemCookie>
    {
        private SystemCookie()
        { }
    }

    public class HttpCookieService : IHttpCookieService
    {
        public HttpCookieService()
        {
        }

        public bool Save(SystemCookie systemCookie, string value)
        {
            if (HttpContext.Current == null)
            {
                return false;
            }

            string cookieName = systemCookie.Value;

            HttpCookie httpCookie = new HttpCookie(cookieName)
            {
                SameSite = SameSiteMode.Lax,
                Value = value
            };

            HttpContext.Current.Response.Cookies.Add(httpCookie);
            HttpContext.Current.Items[cookieName] = value.ToNonNullString();

            return true;
        }

        public string Get(SystemCookie systemCookie)
        {
            return Get(HttpContext.Current.Request.Cookies, systemCookie);
        }

        public string Get(HttpCookieCollection httpCookieCollection, SystemCookie systemCookie)
        {
            string cookieName = systemCookie.Value;
            string value = null;

            if (HttpContext.Current != null)
            {
                value = HttpContext.Current.Items[cookieName] as string;

                if (value != null)
                {
                    return value;
                }
            }

            HttpCookie httpCookie = httpCookieCollection[cookieName];

            if (httpCookie == null)
            {
                return null;
            }

            value = httpCookie.Value;

            return value;
        }
    }
}