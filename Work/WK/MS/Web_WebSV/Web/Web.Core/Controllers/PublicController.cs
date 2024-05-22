using ControllerShareLib.Helpers.Security;
using JxBackendService.Common.Util;
using Microsoft.AspNetCore.Mvc;
using SLPolyGame.Web.Interface;

namespace Web.Controllers
{
    public class PublicController : Controller
    {
        protected readonly ISLPolyGameWebSVService _polyGameServiceClient = null;

        public PublicController(ISLPolyGameWebSVService polyGameServiceClient)
        {
            _polyGameServiceClient = polyGameServiceClient;
        }

        [HttpPost]
        public void ReportStatus()
        {
            string userKey = AuthenticationUtil.GetTokenModel().UserKey;

            if (userKey.IsNullOrEmpty())
            {
                return;
            }

            //_polyGameServiceClient.GetStatus();
        }

        /// <summary> 請退出遊戲並重新進入頁面 </summary>
        [HttpGet]
        public IActionResult ReconnectTips()
        {
            return View();
        }

        /// <summary> 錯誤訊息頁面 </summary>
        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        /// <summary> 遊戲進入中頁面 </summary>
        [HttpGet]
        public IActionResult EnterGameLoading()
        {
            return View();
        }

        /// <summary>監控是否還活著,並且在View中提供特定文字給sre做比對</summary>
        [HttpGet]
        public ActionResult HeartBeat()
        {
            return Content("misekongjian");
        }
    }
}