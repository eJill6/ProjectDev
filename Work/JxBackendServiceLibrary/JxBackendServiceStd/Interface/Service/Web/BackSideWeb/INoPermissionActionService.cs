using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Param.Security;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace JxBackendService.Interface.Service.Web.BackSideWeb
{
    public interface INoPermissionActionService
    {
        IActionResult GetNoPermissionJsonResult();

        IActionResult GetRedirectToNoPermissionPage();

        IActionResult GetRedirectToLoginPage();
    }
}