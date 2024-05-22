using Castle.Core.Internal;
using ControllerShareLib.Helpers.Security;
using ControllerShareLib.Interfaces.Service;
using ControllerShareLib.Interfaces.Service.Controller;
using ControllerShareLib.Models.Base;
using JxBackendService.Common.Extensions;
using JxBackendService.Model.Enums.User;
using JxBackendService.Model.ViewModel.Menu;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly Lazy<IHomeControllerService> _homeControllerService;

        public HomeController()
        {
            _homeControllerService = ResolveService<IHomeControllerService>();
        }

        /// <summary>
        /// 取得最新餘額json
        /// </summary>
        [HttpPost]
        public IActionResult GetRefreshBalanceInfo()
        {
            return PascalCaseJson(GetMoneyTransferResult(true));
        }

        [HttpGet]
        public IActionResult GameCenter()
        {
            SetTicketUserDataToViewBag();

            return View();
        }

        /// <summary>
        /// 遊戲大廳首頁Menu
        /// </summary>
        public JsonResult HomeTopMenu()
        {
            WebGameCenterViewModel webGameCenterViewModel = _homeControllerService.Value.GetWebGameLobbyMenu(isUseRequestHost: false);
            var logonMode = LogonMode.GetSingle(AuthenticationUtil.GetLoginUserFromCache().LogonMode);

            foreach (WebMenuTypeViewModel webMenuTypeViewModel in webGameCenterViewModel.WebMenuTypeViewModels)
            {
                foreach (FrontsideProductMenu menu in webMenuTypeViewModel.FrontsideProductMenus)
                {
                    if (!menu.GameLobbyTypeValue.IsNullOrEmpty())
                    {
                        menu.GameLobbyUrl = ToFullScreenUrlByDebugSetting(menu.GameLobbyUrl, menu.IsHideHeaderWithFullScreen, menu.Title, logonMode);
                    }
                }
            }

            return PascalCaseJson(webGameCenterViewModel);
        }

        private MoneyTransferModel GetMoneyTransferResult(bool isForcedRefresh)
        {
            var model = new MoneyTransferModel();

            SLPolyGame.Web.Model.UserInfo userInfo = GetUserInfo(isForcedRefresh);

            bool hasThousandComma = true;

            model.AvailableScoresDecimal = userInfo.Available;
            model.AvailableScores = model.AvailableScoresDecimal.ToCurrency(hasThousandComma);

            return model;
        }
    }
}