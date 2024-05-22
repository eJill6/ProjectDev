using ControllerShareLib.Interfaces.Service;
using JxBackendService.Attributes.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace M.Core.Filters
{
    public class MobileApiAuthorize : Attribute, IAsyncAuthorizationFilter
    {
        private readonly Lazy<IMiseWebTokenService> _miseWebTokenService;

        public MobileApiAuthorize()
        {
            _miseWebTokenService = DependencyUtil.ResolveService<IMiseWebTokenService>();
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HasAllowAnonymous())
            {
                return;
            }

            BasicUserInfo basicUserInfo = _miseWebTokenService.Value.GetTokenModel();

            if (basicUserInfo == null || basicUserInfo.UserKey.IsNullOrEmpty())
            {
                context.Result = new UnauthorizedResult();

                return;
            }

            if (!_miseWebTokenService.Value.IsCacheTokenValid(basicUserInfo))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            _miseWebTokenService.Value.AddHttpContextUser(context.HttpContext, basicUserInfo);
            await Task.CompletedTask;
        }
    }
}