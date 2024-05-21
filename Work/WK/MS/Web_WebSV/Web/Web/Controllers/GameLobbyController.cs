using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using SLPolyGame.Web.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Helpers;
using Web.Models;
using Web.Models.Base;
using Web.Services;

namespace Web.Controllers
{
    public class GameLobbyController : BaseController
    {
        private static readonly int s_gamePageSize = 20;

        private readonly ISlotApiWebSVService _slotApiWebSVService;

        private readonly IThirdPartyApiWebSVService _thirdPartyApiWebSVService;

        /// <summary></summary>
        public GameLobbyController(IUserService userService,
            ICacheService cacheService,
            ISlotApiWebSVService slotApiWebSVService,
            IThirdPartyApiWebSVService thirdPartyApiWebSVService) : base(cacheService, userService)
        {
            _slotApiWebSVService = slotApiWebSVService;
            _thirdPartyApiWebSVService = thirdPartyApiWebSVService;
        }

        public ActionResult IMJDBGameList()
        {
            return PartialView();
        }

        public ActionResult IMSEGameList()
        {
            return PartialView();
        }

        public ActionResult JDBFIGameList()
        {
            return PartialView();
        }

        public ActionResult IMPTGameList()
        {
            return PartialView();
        }

        public ActionResult IMPPGameList()
        {
            return PartialView();
        }

        public ActionResult PMSLGameList()
        {
            return PartialView();
        }

        public ActionResult GetIMJDBGameList(SubGameRequest request)
            => GetSlotSubGameList(request, GameLobbyType.IMJDB);

        public ActionResult GetIMSEGameList(SubGameRequest request)
            => GetSlotSubGameList(request, GameLobbyType.IMSE);

        public ActionResult GetJDBFIGameList(SubGameRequest request)
            => GetSlotSubGameList(request, GameLobbyType.JDBFI);

        public ActionResult GetIMPTGameList(SubGameRequest request)
            => GetSlotSubGameList(request, GameLobbyType.IMPT);

        public ActionResult GetIMPPGameList(SubGameRequest request)
            => GetSlotSubGameList(request, GameLobbyType.IMPP);

        public ActionResult GetPMSLGameList(SubGameRequest request)
            => GetSlotSubGameList(request, GameLobbyType.PMSL);

        private ActionResult GetSlotSubGameList(SubGameRequest request, GameLobbyType gameLobbyType)
        {
            List<GameLobbyInfo> slotSubGames = _slotApiWebSVService.GetGameList(gameLobbyType.Value);
            ThirdPartyGamesViewModel viewModel = GamesListToViewModel(slotSubGames, request, gameLobbyType);

            return PartialView("~/Views/ThirdParty/Partial/_ThirdPartyGameListPartial.cshtml", viewModel);
        }

        private ThirdPartyGamesViewModel GamesListToViewModel(List<GameLobbyInfo> gamesList, SubGameRequest request, GameLobbyType gameLobbyType)
        {
            if (request.GameTabType == GameTabTypeValue.Hot)
            {
                gamesList = gamesList.FindAll(game => game.IsHot);
            }

            if (request.GameTabType == GameTabTypeValue.Favorite)
            {
                gamesList = gamesList.FindAll(game => request.GameIds.Contains(game.No));
            }

            if (!string.IsNullOrEmpty(request.GameName))
            {
                gamesList = gamesList.FindAll(game => game.ChineseName.Contains(request.GameName));
            }

            // TODO 舊邏輯 待優化
            gamesList = gamesList.FindAll(game => !string.IsNullOrEmpty(game.WebGameCode));

            List<GameLobbyInfo> paginationGames = gamesList.Skip((request.PageNumber - 1) * s_gamePageSize).Take(s_gamePageSize).ToList();

            return new ThirdPartyGamesViewModel
            {
                Data = paginationGames,
                GameTabType = request.GameTabType,
                SearchGameName = request.GameName,
                IsSelfOpenPage = gameLobbyType.IsSelfOpenPage,
                IsSquareGameImage = gameLobbyType.IsSquareGameImage,
                PageNumber = request.PageNumber,
                PageCount = CommonHelper.CalculatePageCount(s_gamePageSize, gamesList.Count)
            };
        }

        public JsonResult IMJDBGame(string gamecode)
        {
            return DoLogin(PlatformProduct.IMPP, gamecode);
        }

        public JsonResult IMSEGame(string gamecode)
        {
            return DoLogin(PlatformProduct.IMPP, gamecode);
        }

        public JsonResult JDBFIGame(string gamecode)
        {
            return DoLogin(PlatformProduct.JDBFI, gamecode);
        }

        public JsonResult IMPPGame(string gamecode)
        {
            return DoLogin(PlatformProduct.IMPP, gamecode);
        }

        public JsonResult IMPTGame(string gamecode)
        {
            return DoLogin(PlatformProduct.IMPT, gamecode);
        }

        public JsonResult PMSLGame(string gamecode)
        {
            return DoLogin(PlatformProduct.PMSL, gamecode);
        }

        private JsonResult DoLogin(PlatformProduct platformProduct, string gamecode)
        {
            string loginInfoJson = new LoginInfo
            {
                RemoteCode = gamecode
            }.ToJsonString();

            // 確認是否為手機網頁版
            bool isMobile = CommonHelper.IsMobile(out string clientMobileVer);

            BaseReturnDataModel<JxBackendService.Model.Param.ThirdParty.TPGameOpenParam> returnModel = _thirdPartyApiWebSVService.GetForwardGameUrl(
                platformProduct.Value,
                loginInfoJson,
                isMobile,
                correlationId: Guid.NewGuid().ToString());

            if (!returnModel.IsSuccess)
            {
                return new JsonNetResult(new BaseReturnDataModel<string>(returnModel.Message));
            }

            return new JsonNetResult(new BaseReturnDataModel<string>(ReturnCode.Success, returnModel.DataModel.Url));
        }

        [HttpPost]
        public JsonResult GetJackpotAmount()
        {
            string jackpotAmount = _slotApiWebSVService.GetJackpotAmount(GameLobbyType.IMPT.Value);

            return Json(new { success = true, result = jackpotAmount });
        }
    }
}