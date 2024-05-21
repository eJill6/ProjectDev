using JxBackendService.Attributes;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.MiseLive.Response;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using System;
using System.Net;
using System.Web.Mvc;
using Web.Helpers.Security;
using Web.Models.Base;
using Web.Models.Results;

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
            var debugUserService = DependencyUtil.ResolveService<IDebugUserService>();

            if (!debugUserService.IsDebugUser(EnvLoginUser.LoginUser.UserId))
            {
                return;
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}