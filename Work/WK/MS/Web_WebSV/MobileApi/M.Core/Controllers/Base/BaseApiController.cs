using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace M.Core.Controllers.Base
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BaseApiController : ControllerBase
    {
        private readonly Lazy<IConfigUtilService> _configUtilService;

        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        protected JxApplication Application => s_environmentService.Value.Application;

        protected IConfigUtilService ConfigUtilService => _configUtilService.Value;

        public BaseApiController()
        {
            _configUtilService = ResolveService<IConfigUtilService>();
        }

        protected virtual EnvironmentUser EnvLoginUser => new EnvironmentUser()
        {
            Application = Application,
            LoginUser = new BasicUserInfo()
        };

        protected AppResponseModel<T> ConvertToAppResponse<T>(T data) where T : class
        {
            return new AppResponseModel<T>()
            {
                Success = true,
                Data = data,
            };
        }

        protected Lazy<T> ResolveService<T>() => DependencyUtil.ResolveService<T>();
    }
}