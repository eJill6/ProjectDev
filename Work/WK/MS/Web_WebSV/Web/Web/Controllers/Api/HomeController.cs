using SLPolyGame.Web.Model;
using System.Web.Http;
using Web.Extensions;
using Web.Models.Base;
using Web.Services;

namespace Web.Controllers.Api
{
    public class HomeController : BaseApiController
    {
        public HomeController(IUserService userService,
            ICacheService cacheService) : base(cacheService, userService)
        {
        }

        /// <summary>
        /// 取得最新餘額json
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetRefreshBalanceInfo()
        {
            return Json(GetMoneyTransferResult(true));
        }

        private MoneyTransferModel GetMoneyTransferResult(bool isForcedRefresh)
        {
            var model = new MoneyTransferModel();

            UserInfo userInfo = GetUserInfo(isForcedRefresh);

            bool hasThousandComma = true;

            model.AvailableScoresDecimal = userInfo.Available;
            model.AvailableScores = model.AvailableScoresDecimal.ToCurrency(hasThousandComma);

            return model;
        }
    }
}