using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.Enums;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure.Attributes;

namespace Web.Core.Controllers.Api.Base
{
    [MiseWebTokenApiAuthorize()]
    [ApiController]
    [Route("mwt/{MiseWebToken}/[controller]/[action]")]
    public class BaseAuthApiController : ControllerBase
    {
        private readonly Lazy<IHttpContextUserService> _httpContextUserService;

        private readonly Lazy<IConfigUtilService> _configUtilService;

        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        protected JxApplication Application => s_environmentService.Value.Application;

        protected IConfigUtilService ConfigUtilService => _configUtilService.Value;

        protected IHttpContextUserService HttpContextUserService => _httpContextUserService.Value;

        public BaseAuthApiController()
        {
            _httpContextUserService = DependencyUtil.ResolveService<IHttpContextUserService>();
            _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
        }
    }
}