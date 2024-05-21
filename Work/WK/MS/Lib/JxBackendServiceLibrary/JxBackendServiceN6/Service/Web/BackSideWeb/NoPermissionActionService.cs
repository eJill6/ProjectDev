using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JxBackendServiceN6.Service.Web.BackSideWeb
{
    public class NoPermissionActionService : INoPermissionActionService
    {
        public IActionResult GetNoPermissionJsonResult()
        {
            var contentResult = new ContentResult
            {
                ContentType = $"{HttpWebRequestContentType.Json}; charset=utf-8",
                Content = new BaseReturnModel(MessageElement.NoPermission).ToJsonString(isCamelCaseNaming: true),
                StatusCode = (int)HttpStatusCode.BadRequest
            };

            return contentResult;
        }

        public IActionResult GetRedirectToNoPermissionPage()
        {
            return new RedirectToActionResult("NoPermission", "Home", routeValues: null);
        }

        public IActionResult GetRedirectToLoginPage()
        {
            return new RedirectToActionResult("Login", "Authority", routeValues: null);
        }
    }
}