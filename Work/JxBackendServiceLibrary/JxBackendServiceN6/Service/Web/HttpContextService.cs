using Azure.Core;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;

namespace JxBackendServiceN6.Service.Web
{
    public class HttpContextService : IHttpContextService
    {
        private static readonly string s_UserAgentHeaderName = "User-Agent";

        private readonly IHttpContextAccessor _httpContextAccessor;

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

        public Uri GetUri()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                return new Uri(_httpContextAccessor.HttpContext.Request.GetEncodedUrl());
            }

            return null;
        }

        public string GetUserAgent()
        {
            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey(s_UserAgentHeaderName))
            {
                return _httpContextAccessor.HttpContext.Request.Headers[s_UserAgentHeaderName].ToString();
            }

            return string.Empty;
        }

        public bool IsAjaxRequest()
        {
            var httpContextAccessor = DependencyUtil.ResolveService<IHttpContextAccessor>();

            return httpContextAccessor.HttpContext.Request.IsAjaxRequest();
        }
    }

    [MockService]
    public class HttpContextMockService : IHttpContextService
    {
        public string GetAbsoluteUri()
        {
            return "/viet5ssc";
        }

        public Uri GetUri()
        {
            throw new NotImplementedException();
        }

        public string GetUserAgent()
        {
            throw new NotImplementedException();
        }

        public bool IsAjaxRequest()
        {
            throw new NotImplementedException();
        }
    }
}