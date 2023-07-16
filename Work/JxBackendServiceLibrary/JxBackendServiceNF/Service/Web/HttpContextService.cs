using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JxBackendService.Service.Web
{
    public class HttpContextService : IHttpContextService
    {
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
            if (HttpContext.Current != null && HttpContext.Current.Handler != null)
            {
                return HttpContext.Current.Request.Url;
            }

            return null;
        }

        public string GetUserAgent() => HttpContext.Current.Request.UserAgent;

        public bool IsAjaxRequest()
        {
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Headers == null)
            {
                return false;
            }

            return httpRequest.Headers["X-Requested-With"] == "XMLHttpRequest";
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