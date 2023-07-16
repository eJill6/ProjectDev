using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Param.Security;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace JxBackendService.Interface.Service.Web
{
    public interface IHttpContextUserService
    {
        string GetUserKey();

        int GetUserId();

        BasicUserInfo GetBasicUserInfo();
    }
}