using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Web.Infrastructure.Attributes
{
    public class MiseWebTokenApiAuthorizeAttribute : MiseWebTokenAuthorizeAttribute
    {
        protected override IActionResult GetUnauthorizedPageActionResult()
        {
            return base.GetUnauthorizedAjaxActionResult();
        }

        protected override HttpStatusCode GetUnauthorizedPageStatusCode()
        {
            return base.GetUnauthorizedAjaxStatusCode();
        }
    }
}