using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Param.Security;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace JxBackendService.Interface.Service.Web.BackSideWeb
{
    public interface IBackSideWebLoginService
    {
        BaseReturnDataModel<BWLoginResultParam> Login(LoginDetailParam loginParam, Action<ClaimsPrincipal, AuthenticationProperties> doHttpSignIn);
    }
}