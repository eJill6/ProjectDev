using ControllerShareLib.Helpers;
using ControllerShareLib.Interfaces.Service.Controller;
using ControllerShareLib.Models.Game;
using ControllerShareLib.Models.Game.GameLobby;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using M.Core.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace M.Core.Controllers
{
    public class GameLobbyController : BaseAuthApiController
    {
        private readonly string s_mobileApiBannerImagePath = "/images/third_party/mobile";

        private readonly Lazy<IGameLobbyControllerService> _gameLobbyControllerService;

        public GameLobbyController()
        {
            _gameLobbyControllerService = ResolveService<IGameLobbyControllerService>();
        }

        /// <summary>取得電子大廳資訊</summary>
        [HttpGet, AllowAnonymous]
        public AppResponseModel<MobileApiGameLobbyInfoViewModel> GetGameLobbyInfo(string gameLobbyTypeValue)
        {
            GameLobbyType gameLobbyType = GameLobbyType.GetSingle(gameLobbyTypeValue);

            var subGameService = DependencyUtil.ResolveJxBackendService<ISubGameService>(gameLobbyType, EnvLoginUser, DbConnectionTypes.Slave).Value;
            string fileName = subGameService.MobileApiBannerImageFileName;
            string aesFileName = subGameService.AESMobileApiBannerImageFileName;

            string mobileApiBannerImageFullUrl = WebResourceHelper.Content(
                resourceUrl: $"{GlobalCacheHelper.RazorShareContentPath}{s_mobileApiBannerImagePath}/{fileName}",
                isAppendVersion: true,
                isUseRequestHost: true);

            string aesMobileApiBannerImageFullUrl = WebResourceHelper.Content(
                resourceUrl: $"{GlobalCacheHelper.RazorShareContentPath}{s_mobileApiBannerImagePath}/{aesFileName}",
                isAppendVersion: true,
                isUseRequestHost: true);

            string? jackpotAmount = _gameLobbyControllerService.Value.GetJackpotAmount(gameLobbyType);

            return new AppResponseModel<MobileApiGameLobbyInfoViewModel>()
            {
                Success = true,
                Data = new MobileApiGameLobbyInfoViewModel
                {
                    BannerFullImageUrl = mobileApiBannerImageFullUrl,
                    AESBannerFullImageUrl = aesMobileApiBannerImageFullUrl,
                    JackpotAmount = jackpotAmount
                }
            };
        }

        /// <summary>電子大廳遊戲列表</summary>
        [HttpPost, AllowAnonymous]
        public AppResponseModel<MobileApiThirdPartyGamesViewModel> GetGameList(GameLobbySubGameRequest request)
        {
            MobileApiThirdPartyGamesViewModel mobileApiThirdPartyGamesViewModel = _gameLobbyControllerService.Value.GetMobileApiThirdPartyGamesViewModel(request);

            return new AppResponseModel<MobileApiThirdPartyGamesViewModel>()
            {
                Success = true,
                Data = mobileApiThirdPartyGamesViewModel
            };
        }

        /// <summary>登入子遊戲</summary>
        [HttpPost]
        public AppResponseModel<AppOpenUrlInfo> LoginGameLobbyGame(LoginGameLobbyGameParam param)
        {
            BaseReturnDataModel<AppOpenUrlInfo> returnModel = _gameLobbyControllerService.Value.LoginGameLobbyGame(param);

            return new AppResponseModel<AppOpenUrlInfo>(returnModel);
        }
    }
}