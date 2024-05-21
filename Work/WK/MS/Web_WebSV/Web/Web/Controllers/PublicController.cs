using JxBackendService.Common.Util;
using SLPolyGame.Web.Interface;
using System.Web.Mvc;
using Web.Helpers.Security;

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
        public ActionResult ReconnectTips()
        {
            return View();
        }

        /// <summary> 錯誤訊息頁面 </summary>
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        /// <summary> 遊戲進入中頁面 </summary>
        [HttpGet]
        public ActionResult EnterGameLoading()
        {
            return View();
        }
    }
}