using BackSideWeb.Controllers;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Route;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Interface.Service.MessageQueue;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BackSideWeb.Filters
{
    public class CheckUserPasswordExpirationAttribute : ActionFilterAttribute
    {
        private static readonly int s_mqDelaySeconds = 1;

        private readonly Lazy<IBackSideWebUserService> _backSideWebLoginUserService;

        private readonly Lazy<IHttpContextService> _httpContextService;

        private readonly Lazy<IJxCacheService> _jxCacheService;

        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        public CheckUserPasswordExpirationAttribute()
        {
            _backSideWebLoginUserService = DependencyUtil.ResolveService<IBackSideWebUserService>();
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
            _httpContextService = DependencyUtil.ResolveService<IHttpContextService>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IBWChangePasswordService bwChangePasswordService = DependencyUtil.ResolveJxBackendService<IBWChangePasswordService>(
                GetEnvironmentUser(), DbConnectionTypes.Master).Value;

            if (context.Controller is not ChangePasswordController
                && bwChangePasswordService.IsPasswordExpiredCheckWithCache())
            {
                RedirectToChangePasswordPage(context);

                return;
            }
        }

        private void RedirectToChangePasswordPage(ActionExecutingContext context)
        {
            if (_httpContextService.Value.IsAjaxRequest())
            {
                IMessageQueueDelayJobService messageQueueDelayJobService = DependencyUtil.ResolveJxBackendService<IMessageQueueDelayJobService>(
                    GetEnvironmentUser(), DbConnectionTypes.Slave).Value;

                messageQueueDelayJobService.AddDelayJobParam(
                    new BWUserChangePasswordMessage { UserID = GetEnvironmentUser().LoginUser.UserId },
                    delaySeconds: s_mqDelaySeconds);

                context.Result = new JsonResult(new BaseReturnModel(ReturnCode.OperationFailed));
            }
            else
            {
                context.Result = new RedirectToActionResult(
                    nameof(ChangePasswordController.Index),
                    nameof(ChangePasswordController).RemoveControllerNameSuffix(),
                    routeValues: null);
            }
        }

        private EnvironmentUser GetEnvironmentUser()
        {
            BackSideWebUser backSideWebUser = _backSideWebLoginUserService.Value.GetUser();

            return new EnvironmentUser()
            {
                Application = s_environmentService.Value.Application,
                LoginUser = backSideWebUser
            };
        }
    }
}