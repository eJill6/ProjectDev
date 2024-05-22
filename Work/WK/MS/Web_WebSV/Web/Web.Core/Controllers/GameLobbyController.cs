using ControllerShareLib.Interfaces.Service.Controller;
using ControllerShareLib.Models.Game;
using ControllerShareLib.Models.Game.GameLobby;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class GameLobbyController : BaseController
    {
        private readonly Lazy<IGameLobbyControllerService> _gameLobbyControllerService;

        public GameLobbyController()
        {
            _gameLobbyControllerService = ResolveService<IGameLobbyControllerService>();
        }

        public IActionResult IMJDBGameList()
        {
            return PartialView();
        }

        public IActionResult IMSEGameList()
        {
            return PartialView();
        }

        public IActionResult JDBFIGameList()
        {
            return PartialView();
        }

        public IActionResult IMPTGameList()
        {
            return PartialView();
        }

        public IActionResult IMPPGameList()
        {
            return PartialView();
        }

        public IActionResult PMSLGameList()
        {
            return PartialView();
        }

        #region 暫時註解原本寫法

        //public IActionResult GetIMJDBGameList(SubGameRequest request)
        //    => GetSlotSubGameList(request, GameLobbyType.IMJDB, "~/images/IMJDB/GameList");

        //public IActionResult GetIMSEGameList(SubGameRequest request)
        //    => GetSlotSubGameList(request, GameLobbyType.IMSE, "~/images/IMSE/GameList");

        //public IActionResult GetJDBFIGameList(SubGameRequest request)
        //    => GetSlotSubGameList(request, GameLobbyType.JDBFI, "~/images/JDBFI/GameList");

        //public IActionResult GetIMPTGameList(SubGameRequest request)
        //    => GetSlotSubGameList(request, GameLobbyType.IMPT, "~/images/IMPT/GameList");

        //public IActionResult GetIMPPGameList(SubGameRequest request)
        //    => GetSlotSubGameList(request, GameLobbyType.IMPP, "~/images/IMPP/GameList");

        //public IActionResult GetPMSLGameList(SubGameRequest request)
        //    => GetSlotSubGameList(request, GameLobbyType.PMSL, "~/images/PMSL/GameList");

        #endregion 暫時註解原本寫法

        [HttpPost]
        public IActionResult GetGameLobbyGameList(GameLobbySubGameRequest request)
        {
            return GetSlotSubGameList(request);
        }

        private IActionResult GetSlotSubGameList(GameLobbySubGameRequest request)
        {
            WebThirdPartyGamesViewModel viewModel = _gameLobbyControllerService.Value.GetWebThirdPartyGamesViewModel(request);

            return PartialView("~/Views/ThirdParty/Partial/_ThirdPartyGameListPartial.cshtml", viewModel);
        }

        #region 暫時註解原本寫法

        //public JsonResult IMJDBGame(string gamecode)
        //{
        //    return DoLogin(PlatformProduct.IMPP, gamecode);
        //}

        //public JsonResult IMSEGame(string gamecode)
        //{
        //    return DoLogin(PlatformProduct.IMPP, gamecode);
        //}

        //public JsonResult JDBFIGame(string gamecode)
        //{
        //    return DoLogin(PlatformProduct.JDBFI, gamecode);
        //}

        //public JsonResult IMPPGame(string gamecode)
        //{
        //    return DoLogin(PlatformProduct.IMPP, gamecode);
        //}

        //public JsonResult IMPTGame(string gamecode)
        //{
        //    return DoLogin(PlatformProduct.IMPT, gamecode);
        //}

        //public JsonResult PMSLGame(string gamecode)
        //{
        //    return DoLogin(PlatformProduct.PMSL, gamecode);
        //}

        #endregion 暫時註解原本寫法

        [HttpPost]
        public JsonResult LoginGameLobbyGame(LoginGameLobbyGameParam param)
        {
            BaseReturnDataModel<AppOpenUrlInfo> returnModel = _gameLobbyControllerService.Value.LoginGameLobbyGame(param);

            if (!returnModel.IsSuccess)
            {
                return PascalCaseJson(new BaseReturnDataModel<string>(returnModel.Message));
            }

            return PascalCaseJson(new BaseReturnDataModel<string>(ReturnCode.Success, returnModel.DataModel.Url));
        }

        [HttpGet]
        public JsonResult GetIMPTJackpotAmount()
        {
            string jackpotAmount = _gameLobbyControllerService.Value.GetJackpotAmount(GameLobbyType.IMPT);

            return Json(new { success = true, result = jackpotAmount });
        }
    }
}