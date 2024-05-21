using System.Collections.Concurrent;
using System.Web;
using System.Web.Mvc;
using Web.Helpers;
using Web.Helpers.Security;

namespace Web.Infrastructure.Attributes
{
    public class WebAuthorizeAttribute : BaseWebAuthorizeAttribute
    {
        private static ConcurrentDictionary<string, int> _expiryTokenAttemptedDictionary = new ConcurrentDictionary<string, int>();

        protected override bool DoAuthorizeCore(HttpContextBase httpContext)
        {
            return CheckToken(httpContext);
        }

        protected override ActionResult GetUnauthorizedActionResult()
        {
            return new HttpUnauthorizedResult();
        }

        /// <summary>
        /// 1.從cookie取得token, 檢查是否過期，刷新token
        /// 2.當user重複使用過期的token則強制登出
        /// </summary>
        /// <returns>IsTokenValid</returns>
        private bool CheckToken(HttpContextBase context)
        {
            var tokenCookie = context.Request.Cookies[CookieKeyHelper.Token];
            var token = null as TokenModel;
            var isFromHeader = false;
            if (tokenCookie != null)
            {
                token = TokenProvider.GetToken(tokenCookie.Value);
            }
            else
            {
                var header = context.Request.Headers[CookieKeyHelper.HeaderToken];
                if (!string.IsNullOrWhiteSpace(header))
                {
                    isFromHeader = true;
                    header = header.Replace(CookieKeyHelper.HeaderRemove, string.Empty);
                    token = TokenProvider.GetToken(header);
                }
            }

            if (token != null)
            {
                var key = token.Key;

                if (token.IsExpired)
                {
                    if (!isFromHeader)
                    {
                        if (!_expiryTokenAttemptedDictionary.ContainsKey(key))
                        {
                            _expiryTokenAttemptedDictionary.GetOrAdd(key, 0);
                        }

                        if (_expiryTokenAttemptedDictionary[key] > 20)
                        {
                            int i;
                            _expiryTokenAttemptedDictionary.TryRemove(key, out i);
                            return false;
                        }

                        _expiryTokenAttemptedDictionary[key]++;
                        if (_expiryTokenAttemptedDictionary[key] <= 1)
                        {
                            var value = TokenProvider.GenerateTokenString(key, token.UserName);
                            tokenCookie = new HttpCookie(CookieKeyHelper.Token, value);
                            context.Response.Cookies.Set(tokenCookie);
                        }
                        return true;
                    }

                    return false;
                }
                else
                {
                    if (_expiryTokenAttemptedDictionary.ContainsKey(key))
                    {
                        int i;
                        _expiryTokenAttemptedDictionary.TryRemove(key, out i);
                    }

                    return true;
                }
            }
            return false;
        }
    }
}