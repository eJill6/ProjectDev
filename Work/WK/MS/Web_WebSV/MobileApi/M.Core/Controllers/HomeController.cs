using ControllerShareLib.Interfaces.Service.Controller;
using ControllerShareLib.Models.Base;
using ControllerShareLib.Models.Game.Menu;
using JxBackendService.Common.Extensions;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.ReturnModel;
using M.Core.Controllers.Base;
using M.Core.Interface.Services;
using M.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace M.Core.Controllers
{
    public class HomeController : BaseAuthApiController
    {
        private readonly Lazy<IHomeControllerService> _homeControllerService;
        private readonly Lazy<IMenuService> _menuService;

        public HomeController()
        {
            _homeControllerService = ResolveService<IHomeControllerService>();
            _menuService = DependencyUtil.ResolveService<IMenuService>();
        }

        /// <summary> 遊戲大廳首頁Menu </summary>
        [HttpGet, AllowAnonymous]
        public AppResponseModel<MobileApiGameCenterViewModel> HomeTopMenu()
        {
            MobileApiGameCenterViewModel mobileApiGameCenterViewModel =
                _homeControllerService.Value.GetMobileApiGameLobbyMenu(isUseRequestHost: true, isForceRefresh: false);

            return ConvertToAppResponse(mobileApiGameCenterViewModel);
        }

        /// <summary> 遊戲內頁Menu </summary>
        [HttpGet, AllowAnonymous]
        public AppResponseModel<List<MobileApiMenuInnerInfo>> GetMenuInnerInfos()
        {
            List<MobileApiMenuInnerInfo> mobileApiMenuInnerInfos = _homeControllerService.Value.GetMobileApiMenuInnerInfos(isUseRequestHost: true);

            return ConvertToAppResponse(mobileApiMenuInnerInfos);
        }

        /// <summary> 直播遊戲Menu </summary>
        [HttpGet, AllowAnonymous]
        public AppResponseModel<List<LiveGameTypeAndMenu>> GetLiveGameTypeAndMenus()
        {
            List<LiveGameTypeAndMenu> liveGameTypeAndMenus = _menuService.Value.GetLiveGameTypeAndMenus();

            return ConvertToAppResponse(liveGameTypeAndMenus);
        }

        /// <summary>
        /// 取得最新餘額
        /// </summary>
        [HttpPost]
        public AppResponseModel<MoneyTransferModel> GetRefreshBalanceInfo()
        {
            SLPolyGame.Web.Model.UserInfo userInfo = GetUserInfo();

            var moneyTransferModel = new MoneyTransferModel()
            {
                AvailableScoresDecimal = userInfo.Available,
                AvailableScores = userInfo.Available.ToCurrency(hasThousandComma: true),
            };

            return ConvertToAppResponse(moneyTransferModel);
        }
    }
}