using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Web;
using Web.Infrastructure.Filters;
using Web.Helpers;
using Web.Helpers.Security;
using Web.Models.Base;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Web.Helpers;
using System.Text;
using System.Net.Http.Headers;

namespace Web.Infrastructure.Attributes.Api
{
    public abstract class BaseWebAuthorizeAttribute : AuthorizeAttribute
    {
        private static ConcurrentDictionary<string, int> _expiryTokenAttemptedDictionary = new ConcurrentDictionary<string, int>();

        protected BaseWebAuthorizeAttribute()
        {
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            HandleUnauthorizedActionResult(filterContext);
        }

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            var hasAnonymous = filterContext
                .ActionDescriptor
                .GetCustomAttributes<AnonymousAttribute>()
                .Any();

            if (hasAnonymous)
            {
                return;
            }

            var context = filterContext;
            var isTokenValid = CheckToken(context);

            if (!isTokenValid)
            {
                HandleUnauthorizedActionResult(filterContext);
                return;
            }

            base.OnAuthorization(filterContext);
        }

        private void HandleUnauthorizedActionResult(HttpActionContext context)
        {
            var response = context.Response = context.Response ?? new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Unauthorized;
            var content = new
            {
                code = HttpStatusCode.Unauthorized,
                error = "登录已过期！"
            };
            response.Content = new StringContent(Json.Encode(content), Encoding.UTF8, "application/json");
        }

        protected override bool IsAuthorized(HttpActionContext httpContext)
        {
            var isTokenValid = CheckToken(httpContext);

            if (!isTokenValid)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 1.從cookie取得token, 檢查是否過期，刷新token
        /// 2.當user重複使用過期的token則強制登出
        /// </summary>
        /// <returns>IsTokenValid</returns>
        private bool CheckToken(HttpActionContext context)
        {
            var header = string.Empty;
            var isFromHeader = false;
            if (context.Request.Headers.TryGetValues(CookieKeyHelper.HeaderToken, out var vaules))
            {
                header = vaules.FirstOrDefault();
                if (header != null)
                {
                    header = header.Replace(CookieKeyHelper.HeaderRemove, string.Empty);
                }
                isFromHeader = true;
            }
            else
            {
                var cookie = context.Request.Headers.GetCookies(CookieKeyHelper.Token).FirstOrDefault();
                if (cookie != null)
                {
                    header = cookie[CookieKeyHelper.Token].Value;
                }
            }

            if (!string.IsNullOrWhiteSpace(header))
            {
                var token = TokenProvider.GetToken(header);
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
                                var value = TokenProvider.GenerateTokenString(key, token.UserName);
                                var cookie = context.Request.Headers.GetCookies(CookieKeyHelper.Token).FirstOrDefault();
                                if (cookie != null)
                                {
                                    cookie[CookieKeyHelper.Token].Value = value;
                                }

                                return true;
                            }

                            if (_expiryTokenAttemptedDictionary[key] > 20)
                            {
                                _expiryTokenAttemptedDictionary.TryRemove(key, out var i);
                            }

                            _expiryTokenAttemptedDictionary[key]++;
                        }

                        return false;
                    }
                    else
                    {
                        if (_expiryTokenAttemptedDictionary.ContainsKey(key))
                        {
                            _expiryTokenAttemptedDictionary.TryRemove(key, out var i);
                        }

                        return true;
                    }
                }
            }
            return false;
        }
    }
}