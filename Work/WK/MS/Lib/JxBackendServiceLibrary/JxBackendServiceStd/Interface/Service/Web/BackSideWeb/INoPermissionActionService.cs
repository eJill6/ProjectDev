using Microsoft.AspNetCore.Mvc;

namespace JxBackendService.Interface.Service.Web.BackSideWeb
{
    public interface INoPermissionActionService
    {
        IActionResult GetNoPermissionJsonResult();

        IActionResult GetRedirectToNoPermissionPage();

        IActionResult GetRedirectToLoginPage();
    }
}