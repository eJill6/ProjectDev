using System.Web;

namespace Web.Cookie
{
    public interface IHttpCookieService
    {
        string Get(SystemCookie systemCookie);

        string Get(HttpCookieCollection httpCookieCollection, SystemCookie systemCookie);

        bool Save(SystemCookie systemCookie, string value);
    }
}