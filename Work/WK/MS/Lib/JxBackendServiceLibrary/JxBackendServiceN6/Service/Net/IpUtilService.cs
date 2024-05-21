using IPTool;
using IPToolModel;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Attributes;
using JxBackendService.Service.Net;
using Microsoft.AspNetCore.Http;

namespace JxBackendServiceN6.Service.Net
{
    public class IpUtilService : BaseIpUtilService
    {
        private readonly Lazy<IHttpContextAccessor> _httpContextAccessor;

        public IpUtilService()
        {
            _httpContextAccessor = DependencyUtil.ResolveService<IHttpContextAccessor>();
        }

        protected override string GetIPFromWebHeader()
        {
            var ipHelper = new IPHelper();
            string ip = string.Empty;

            try
            {
                string errMessage = string.Empty;
                string ipMessage = string.Empty;
                ip = ipHelper.GetDoWorkIP(CreateIPContextModel(), ref errMessage, ref ipMessage);

                if (!errMessage.IsNullOrEmpty())
                {
                    LogUtilService.Error($"GetDoWorkIP errMessage:{errMessage}");
                }
            }
            catch (Exception ex)
            {
                LogUtilService.Error(ex);
            }

            return ip;
        }

        protected override string GetIPFromSVHeader()
        {
            return _httpContextAccessor.Value.HttpContext.Request.Headers["ip"];//此為SV特殊header,若沒有拿到則使用IPTool去取得ip
        }

        private IPContext_Model CreateIPContextModel()
        {
            IHeaderDictionary headers = _httpContextAccessor.Value.HttpContext.Request.Headers;

            return new IPContext_Model()
            {
                HTTP_X_FORWARDED_FOR = headers["X-Forwarded-For"],
                Proxy_Client_IP = headers["Proxy-Client-IP"],
                WL_Proxy_Client_IP = headers["WL-Proxy-Client-IP"],
                HTTP_CLIENT_IP = headers["HTTP_CLIENT_IP"],
                X_Real_IP = headers["X-Real-IP"],
                REMOTE_ADDR = headers["REMOTE_ADDR"],
                UserHostAddress = _httpContextAccessor.Value.HttpContext.Connection.RemoteIpAddress?.ToString()
            };
        }
    }
}

[MockService]
public class IpUtilMockService : IIpUtilService
{
    public string GetIPAddress()
    {
        return "61.220.213.94";
    }
}