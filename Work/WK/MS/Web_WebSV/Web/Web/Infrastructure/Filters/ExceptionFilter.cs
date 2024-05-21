using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using System;
using System.Web.Mvc;
using Web.Helpers.Security;
using Web.Models.Base;

namespace Web.Infrastructure.Filters
{
    public class ExceptionFilter : FilterAttribute, IExceptionFilter
    {
        private readonly Lazy<EnvironmentUser> _environmentUser = new Lazy<EnvironmentUser>(
            () =>
            {
                BasicUserInfo basicUserInfo = AuthenticationUtil.GetTokenModel();

                var environmentUser = new EnvironmentUser()
                {
                    Application = JxApplication.FrontSideWeb,
                    LoginUser = basicUserInfo
                };

                return environmentUser;
            });

        public static bool IsIgnoreJsonPrimitiveError { get; set; } = true;

        public void OnException(ExceptionContext filterContext)
        {
            var isFlashRequest = false;
            var type = filterContext.RequestContext.HttpContext.Request.Headers["X-Requested-With"];
            if (type != null)
            {
                isFlashRequest = type.Contains("ShockwaveFlash");
            }

            var isLoginExpired = false;
            var message = string.Empty;

            if (filterContext.Exception.Message.IndexOf("登录已过期") > -1)
            {
                if (filterContext.HttpContext.Request.IsAuthenticated)
                {
                    AuthenticationUtil.LogOff();
                }

                isLoginExpired = true;
                message = "登录已过期";
            }
            else
            {
                message = "应用程序发生错误，请重新登录或稍候重试！";

                //.net底層會觸發,有此判斷可能是避免被惡意打爆日誌
                if (!IsIgnoreJsonPrimitiveError || !filterContext.Exception.Message.Contains("Invalid JSON primitive"))
                {
                    //秘色先註解TGFilter,之後有需要再建起來
                    //var instantMessageFilterService = DependencyUtil.ResolveService<IInstantMessageFilterService>();
                    //var httpContextService = DependencyUtil.ResolveService<IHttpContextService>();
                    //bool isSendMessageToTelegram = instantMessageFilterService.IsAllowToSend(httpContextService.GetAbsoluteUri(), filterContext.Exception);

                    ErrorMsgUtil.ErrorHandle(filterContext.Exception, _environmentUser.Value, isSendMessageToTelegram: true);
                }
            }

            //需要加上对flash请求返回json的支持
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest() || isFlashRequest)
            {
                if (isLoginExpired)
                {
                    filterContext.HttpContext.Response.StatusCode = 403;
                }

                filterContext.Result = new JsonNetResult
                {
                    Data = new
                    {
                        success = false,
                        message
                    }
                };
            }
            else
            {
                var result = new ViewResult()
                {
                    ViewName = "Error",
                    ViewData = filterContext.Controller.ViewData
                };

                result.ViewBag.ErrorMsg = message;
                filterContext.Result = result;
            }

            filterContext.ExceptionHandled = true;
        }
    }
}