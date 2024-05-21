using Microsoft.AspNetCore.Mvc;

namespace RazorShareLib.Controllers
{
    public class CheckController : ControllerBase
    {
        /// <summary>給app確認服務是否運行</summary>
        [Route("/check.html")]
        [HttpGet]
        public IActionResult OK()
        {
            return Content("OK!", "text/html");
        }
    }
}