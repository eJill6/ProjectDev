using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Infrastructure.Filters;
using Web.Helpers;
using Web.Helpers.Security;
using Web.Models.Base;
using System;
using JxBackendService.Common.Util.Route;
using JxBackendService.Common.Util;
using System.Security.Claims;
using System.Collections.Generic;

namespace Web.Infrastructure.Attributes
{
    public class MiseWebTokenAuthorizeAttribute : WebAuthorizeAttribute
    {
        public MiseWebTokenAuthorizeAttribute()
        {
        }

        protected override bool DoAuthorizeCore(HttpContextBase httpContext)
        {
            JxBackendService.Model.ViewModel.BasicUserInfo basicUserInfo = AuthenticationUtil.GetTokenModel();

            if (!basicUserInfo.UserKey.IsNullOrEmpty())
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, basicUserInfo.UserId.ToString(), ClaimValueTypes.Integer32),
                    new Claim("UserKey", basicUserInfo.UserKey, ClaimValueTypes.String)
                };

                var identity = new ClaimsIdentity(claims, "MiseWebToken");
                httpContext.User = new ClaimsPrincipal(identity);
            }

            return !AuthenticationUtil.GetUserKey().IsNullOrEmpty();
        }
    }
}