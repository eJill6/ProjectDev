using System;

namespace JxBackendService.Interface.Service.Web
{
    public interface IHttpContextService
    {
        string GetAbsoluteUri();

        Uri GetUri();

        bool IsAjaxRequest();

        string GetUserAgent();

        string GetSchemeAndHost();

        bool HasHttpContext();
    }
}