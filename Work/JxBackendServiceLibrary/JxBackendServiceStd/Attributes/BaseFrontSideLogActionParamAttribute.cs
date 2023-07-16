using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.User;
using JxBackendService.Interface.Service.Util;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Text;

namespace JxBackendService.Attributes
{
    public abstract class BaseFrontSideLogActionParamAttribute : ActionFilterAttribute
    {
        protected abstract int GetLoginUserId(ActionExecutingContext filterContext);

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            IDebugUserService debugUserService = DependencyUtil.ResolveService<IDebugUserService>();
            int userId = GetLoginUserId(filterContext);

            if (debugUserService.IsDebugUser(userId))
            {
                return;
            }

            try
            {
                Uri uri = filterContext.HttpContext.Request.ToUri();
                var debugContent = new StringBuilder();
                debugContent.AppendLine($"url={uri}");
                debugContent.AppendLine($"UserName={userId}");

                if (filterContext.ActionArguments.Any())
                {
                    debugContent.Append("ActionParameters=");

                    foreach (string key in filterContext.ActionArguments.Keys)
                    {
                        string jsonValue = filterContext.ActionArguments[key].ToJsonString();
                        debugContent.AppendLine($"{key}={jsonValue}");
                    }
                }

                debugUserService.ForcedDebug(userId, debugContent.ToString());
            }
            catch (Exception ex)
            {
                DependencyUtil.ResolveService<ILogUtilService>().Error(ex);
            }
        }
    }
}