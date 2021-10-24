using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Mvc;

namespace JxBackendService.Attributes
{
    public abstract class BaseFrontSideLogActionParamAttribute : ActionFilterAttribute
    {
        protected abstract string GetLoginUserName(ActionExecutingContext filterContext);

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            IDebugUserService debugUserService = DependencyUtil.ResolveService<IDebugUserService>();
            string userName = GetLoginUserName(filterContext);

            if (userName.IsNullOrEmpty() || !debugUserService.IsDebugUser(userName))
            {
                return;
            }

            try
            {
                string url = filterContext.HttpContext.Request.Url.ToString();
                var debugContent = new StringBuilder();
                debugContent.AppendLine($"url={url}");
                debugContent.AppendLine($"UserName={userName}");

                if (filterContext.ActionParameters.Any())
                {
                    debugContent.Append("ActionParameters=");

                    foreach (string key in filterContext.ActionParameters.Keys)
                    {
                        string jsonValue = filterContext.ActionParameters[key].ToJsonString();
                        debugContent.AppendLine($"{key}={jsonValue}");
                    }
                }

                debugUserService.ForcedDebug(userName, debugContent.ToString());
            }
            catch(Exception ex)
            {
                LogUtil.Error(ex);
            }
        }
    }
}

