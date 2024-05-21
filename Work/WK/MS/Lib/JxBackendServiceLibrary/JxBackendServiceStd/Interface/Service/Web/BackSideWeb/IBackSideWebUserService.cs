using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Param.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace JxBackendService.Interface.Service.Web.BackSideWeb
{
    public interface IBackSideWebUserService : IHttpContextUserService
    {
        RedirectToActionResult Logout(Action doHttpSignOut);

        BackSideWebUser GetUser();

        BackSideWebUser GetUser(int userId);

        bool HasPermission(PermissionKeys permissionKey, AuthorityTypes authorityType);

        Dictionary<MenuType, List<PermissionKeyDetail>> GetUserMenuMap(int userId);

        void SetLoginCache(BackSideWebUser backSideWebUser);

        void SignIn(BackSideWebUser backSideWebUser, Action<ClaimsPrincipal, AuthenticationProperties> doHttpSignIn);
    }
}