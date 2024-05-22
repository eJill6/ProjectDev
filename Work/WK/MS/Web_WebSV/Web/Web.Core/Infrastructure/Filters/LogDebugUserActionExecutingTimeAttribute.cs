using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.User;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Infrastructure.Filters
{
    /// <summary>
    /// 針對debugUser做的action耗時紀錄
    /// </summary>
    public class LogDebugUserActionExecutingTimeAttribute : LogMvcActionExecutingTimeAttribute
    {
        public LogDebugUserActionExecutingTimeAttribute()
        {
        }

        public LogDebugUserActionExecutingTimeAttribute(double warningMilliseconds) : base(warningMilliseconds)
        {
        }

        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            var debugUserService = DependencyUtil.ResolveService<IDebugUserService>().Value;

            if (!debugUserService.IsDebugUser(EnvLoginUser.LoginUser.UserId))
            {
                return;
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}