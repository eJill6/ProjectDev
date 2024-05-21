using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.Enums.Net;
using JxBackendService.Model.ViewModel;
using JxBackendServiceN6.Filters;

namespace SLPolyGame.Web.Core.Filters
{
    /// <summary>
    /// ApiLogRequestAttribute
    /// </summary>
    public class ApiLogRequestAttribute : BaseApiLogRequestAttribute
    {
        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        public ApiLogRequestAttribute(bool isLogToDB = false) : base(isLogToDB)
        {
        }

        protected override int GetAffectedUserID(HttpContext httpContext)
        {
            return GetEnvironmentUser(httpContext).LoginUser.UserId;
        }

        protected override EnvironmentUser GetEnvironmentUser(HttpContext httpContext)
        {
            var environmentUser = httpContext.GetItemValue<EnvironmentUser>(HttpContextItemKey.EnvironmentUser);

            if (environmentUser != null)
            {
                return environmentUser;
            }

            var httpContextUserService = DependencyUtil.ResolveService<IHttpContextUserService>().Value;
            BasicUserInfo basicUserInfo = httpContextUserService.GetBasicUserInfo();

            if (basicUserInfo == null)
            {
                basicUserInfo = new BasicUserInfo();
            }

            environmentUser = new EnvironmentUser()
            {
                Application = s_environmentService.Value.Application,
                LoginUser = basicUserInfo
            };

            httpContext.SetItemValue(HttpContextItemKey.EnvironmentUser, environmentUser);

            return environmentUser;
        }
    }
}