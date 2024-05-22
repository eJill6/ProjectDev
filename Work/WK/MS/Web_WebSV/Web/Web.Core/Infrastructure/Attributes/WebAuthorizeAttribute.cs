using JxBackendService.Common.Util.Route;
using JxBackendService.Model.MiseLive.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Web.Controllers;

namespace Web.Infrastructure.Attributes
{
    public abstract class WebAuthorizeAttribute : BaseWebAuthorizeAttribute
    {
        protected override JsonResult GetUnauthorizedAjaxActionResult()
        {
            //401有middleware會攔截並覆寫回傳結果
            return new JsonResult(new { });
        }

        protected override HttpStatusCode GetUnauthorizedAjaxStatusCode()
        {
            return HttpStatusCode.Unauthorized;
        }

        protected override IActionResult GetUnauthorizedPageActionResult()
        {
            return new RedirectToActionResult(
                nameof(PublicController.ReconnectTips),
                nameof(PublicController).RemoveControllerNameSuffix(),
                routeValues: null);
        }
    }
}