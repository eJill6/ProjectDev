using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace JxBackendServiceN6.Service.Web
{
    public class HttpContextService : IHttpContextService
    {
        private static readonly string s_userAgentHeaderName = "User-Agent";

        private readonly Lazy<IHttpContextAccessor> _httpContextAccessor;

        public HttpContextService()
        {
            _httpContextAccessor = DependencyUtil.ResolveService<IHttpContextAccessor>();
        }

        public string GetAbsoluteUri()
        {
            Uri uri = GetUri();

            if (uri != null)
            {
                return uri.AbsoluteUri;
            }

            return null;
        }

        public string GetSchemeAndHost()
        {
            HttpRequest request = _httpContextAccessor.Value.HttpContext.Request;

            return $"{request.Scheme}://{request.Host.Value}";
        }

        public Uri GetUri()
        {
            if (_httpContextAccessor.Value.HttpContext != null)
            {
                return new Uri(_httpContextAccessor.Value.HttpContext.Request.GetEncodedUrl());
            }

            return null;
        }

        public string GetUserAgent()
        {
            IHeaderDictionary headers = _httpContextAccessor.Value.HttpContext.Request.Headers;

            if (headers.ContainsKey(s_userAgentHeaderName))
            {
                return headers[s_userAgentHeaderName].ToString();
            }

            return string.Empty;
        }

        public bool HasHttpContext()
        {
            return _httpContextAccessor.Value.HttpContext != null;
        }

        public bool IsAjaxRequest()
        {
            return _httpContextAccessor.Value.HttpContext.Request.IsAjaxRequest();
        }
    }

    [MockService]
    public class HttpContextMockService : IHttpContextService
    {
        public string GetAbsoluteUri()
        {
            return "/viet5ssc";
        }

        public string GetSchemeAndHost()
        {
            throw new NotImplementedException();
        }

        public Uri GetUri()
        {
            throw new NotImplementedException();
        }

        public string GetUserAgent()
        {
            throw new NotImplementedException();
        }

        public bool HasHttpContext()
        {
            throw new NotImplementedException();
        }

        public bool IsAjaxRequest()
        {
            throw new NotImplementedException();
        }
    }
}