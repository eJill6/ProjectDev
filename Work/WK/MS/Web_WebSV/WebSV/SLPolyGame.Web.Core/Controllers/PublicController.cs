using Microsoft.AspNetCore.Mvc;
using SLPolyGame.Web.Core.Controllers.Base;

namespace SLPolyGame.Web.Core.Controllers
{
    public class PublicController : BaseApiController
    {
        /// <summary>監控是否還活著,並且在View中提供特定文字給sre做比對</summary>
        [HttpGet]
        public ActionResult HeartBeat()
        {
            return Content("misekongjian");
        }
    }
}