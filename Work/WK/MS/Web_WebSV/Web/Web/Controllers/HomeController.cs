using JxBackendService.DependencyInjection;
using JxBackendService.Model.Enums.User;
using JxBackendService.Model.ViewModel;
using SLPolyGame.Web.Interface;
using System.Web.Mvc;
using Web.Extensions;
using Web.Helpers.Security;
using Web.Models.Base;
using Web.Services;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISLPolyGameWebSVService _slPolyGameWebSVService;

        public HomeController(IUserService userService,
            ICacheService cacheService) : base(cacheService, userService)
        {
            _slPolyGameWebSVService = DependencyUtil.ResolveService<ISLPolyGameWebSVService>();
        }

        /// <summary>
        /// 取得最新餘額json
        /// </summary>
        [HttpPost]
        public ActionResult GetRefreshBalanceInfo()
        {
            return Json(GetMoneyTransferResult(true));
        }

        [HttpGet]
        public ActionResult GameCenter()
        {
            SetTicketUserDataToViewBag();

            return View();
        }

        /// <summary>
        /// 首頁Menu
        /// </summary>
        [HttpGet]
        public JsonResult HomeTopMenu()
        {
            FrontsideMenuViewModel model = _slPolyGameWebSVService.GetFrontsideMenuViewModel();
            var logonMode = LogonMode.GetSingle(AuthenticationUtil.GetLoginUserFromCache().LogonMode);

            foreach (PagedFrontsideProductMenu productMenu in model.PagedFrontsideProductMenus)
            {
                foreach (FrontsideProductMenu menu in productMenu.FrontsideProductMenus)
                {
                    if (menu.IsRedirectUrl)
                    {
                        menu.Url = ToFullScreenUrlByDebugSetting(menu.Url, menu.IsHideHeaderWithFullScreen, menu.Title, logonMode);
                    }
                }
            }

            return Json(model, JsonRequestBehavior.AllowGet);
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