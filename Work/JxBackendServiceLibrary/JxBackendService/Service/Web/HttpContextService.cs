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
            if (HttpContext.Current != null && HttpContext.Current.Handler != null)
            {
                return HttpContext.Current.Request.Url.AbsoluteUri;
            }

            return null;
        }
    }

    [MockService]
    public class HttpContextMockService : IHttpContextService
    {
        public string GetAbsoluteUri()
        {
            return "/viet5ssc";
        }
    }
}