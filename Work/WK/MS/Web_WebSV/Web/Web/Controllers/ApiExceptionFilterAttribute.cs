using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using Web.Helpers.Security;
using Web.Infrastructure;
using Web.Models.Base;
using Web.Models.ViewModel;

namespace Web.Controllers
{
    public class ApiExceptionFilterAttribute : ActionFilterAttribute, IExceptionFilter
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

        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            ExceptionInfoViewModel exceptionInfo = ExceptionFilterUtil.ConvertToExceptionInfo(actionExecutedContext.Exception, _environmentUser.Value);

            if (exceptionInfo.IsLoginExpired)
            {
                actionExecutedContext.Response = new HttpResponseMessage
                {
                    Content = new StringContent(FilterContextHelper.CreateUnauthorizedData().ToJsonString())
                };

                return Task.CompletedTask;
            }

            actionExecutedContext.Response = new HttpResponseMessage
            {
                Content = new StringContent(new { success = false, exceptionInfo.message, data = new { exceptionInfo.code, exceptionInfo.details } }.ToJsonString())
            };

            return Task.CompletedTask;
        }
    }
}