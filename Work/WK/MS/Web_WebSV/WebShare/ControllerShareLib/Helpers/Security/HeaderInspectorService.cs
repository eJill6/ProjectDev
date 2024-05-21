using ControllerShareLib.Interfaces.Service.Security;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.ViewModel;

namespace ControllerShareLib.Helpers.Security
{
    public class HeaderInspectorService : IHeaderInspectorService
    {
        private readonly Lazy<IHttpContextService> _httpContextService;

        private readonly Lazy<IIpUtilService> _ipUtilService;

        private readonly Lazy<IEnvironmentService> _environmentService;

        public HeaderInspectorService()
        {
            _httpContextService = DependencyUtil.ResolveService<IHttpContextService>();
            _ipUtilService = DependencyUtil.ResolveService<IIpUtilService>();
            _environmentService = DependencyUtil.ResolveService<IEnvironmentService>();
        }

        public Dictionary<string, string> CreateRequestHeader(BasicUserInfo basicUserInfo)
        {
            string ipAddress = string.Empty;
            string userAgent = string.Empty;

            if (_httpContextService.Value.HasHttpContext())
            {
                ipAddress = _ipUtilService.Value.GetIPAddress();
                userAgent = _httpContextService.Value.GetUserAgent();
            }

            var requestHeader = new Dictionary<string, string>
            {
                { "p1", basicUserInfo.UserId.ToString() },
                { "p2", basicUserInfo.UserKey },
                { "fromApplication", _environmentService.Value.Application.Value},
                { "ip", ipAddress },
                { "UserAgent", userAgent }
            };

            return requestHeader;
        }
    }
}