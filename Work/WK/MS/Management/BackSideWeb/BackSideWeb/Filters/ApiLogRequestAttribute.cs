using JxBackendService.Model.Enums.Net;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendServiceN6.Filters;
using JxBackendService.Interface.Service;
using JxBackendService.DependencyInjection;

namespace BackSideWeb.Filters
{
    public class ApiLogRequestAttribute : BaseApiLogRequestAttribute
    {
        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        public ApiLogRequestAttribute(bool isLogToDB = false) : base(isLogToDB)
        {
        }

        protected override int GetAffectedUserID(HttpContext httpContext)
        {
            int.TryParse(httpContext.Request.Form["userId"], out int userId);

            return userId;
        }

        protected override EnvironmentUser GetEnvironmentUser(HttpContext httpContext)
        {
            var environmentUser = httpContext.GetItemValue<EnvironmentUser>(HttpContextItemKey.EnvironmentUser);

            if (environmentUser != null)
            {
                return environmentUser;
            }

            return new EnvironmentUser()
            {
                Application = s_environmentService.Value.Application,
                LoginUser = new BasicUserInfo()
            };
        }
    }
}