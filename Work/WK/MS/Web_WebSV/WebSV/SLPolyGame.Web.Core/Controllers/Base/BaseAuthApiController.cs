using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.ViewModel;
using SLPolyGame.Web.Core.Filters;

namespace SLPolyGame.Web.Core.Controllers.Base
{
    [WebSVApiAuthorize]
    public class BaseAuthApiController : BaseApiController
    {
        private readonly Lazy<IHttpContextUserService> _httpContextUserService;

        protected override EnvironmentUser EnvLoginUser
        {
            get
            {
                EnvironmentUser envLoginUser = base.EnvLoginUser;
                envLoginUser.LoginUser = _httpContextUserService.Value.GetBasicUserInfo();

                return envLoginUser;
            }
        }

        public BaseAuthApiController()
        {
            _httpContextUserService = DependencyUtil.ResolveService<IHttpContextUserService>();
        }
    }
}