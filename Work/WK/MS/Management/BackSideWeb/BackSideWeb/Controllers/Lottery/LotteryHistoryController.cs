using AutoMapper;
using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Models.Enums;
using BackSideWeb.Models.Param;
using BackSideWeb.Models.ViewModel.PlayBetRecord;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository.Game.Lottery;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Enums.MiseOrder;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Models.Lottery.Enum;
using MS.Core.MMModel.Models.PlayBetRecord.Enum;

namespace BackSideWeb.Controllers.Lottery
{
    /// <summary>
    /// 开奖号码
    /// </summary>
    public class LotteryHistoryController : BaseSearchGridController<CurrentLotteryParam>
    {
        private readonly Lazy<ICurrentLotteryInfoRep> _rep;

        public LotteryHistoryController()
        {
            _rep = DependencyUtil.ResolveJxBackendService<ICurrentLotteryInfoRep>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        protected override string[] PageJavaScripts => new string[]
        {
            "business/lotteryHistory/lotteryHistorySearchParam.min.js",
            "business/lotteryHistory/lotteryHistorySearchService.min.js"
        };

        protected override string ClientServiceName => "lotteryHistorySearchService";

        public override ActionResult Index()
        {
            LotteryHistoryViewModel model = new LotteryHistoryViewModel()
            {
                LotteryIdSelectList = MsHelpers.GetMsLotterySelectListItem(),
                IsLotterySelectList = EnumHelper.GetEnumAll<IsLotteryEnum>()
            };
            return View(model);
        }

        public override ActionResult GetGridViewResult(CurrentLotteryParam param)
        {
            PagedResultModel<LotteryHistoryViewModel> model = new PagedResultModel<LotteryHistoryViewModel>();
            param.EndDate = param.EndDate.AddDays(1);
            var result = _rep.Value.GetCurrentLotteryInfoReport(param);
            if (result != null)
            {
                model = MMHelpers.MapPagedResultModel<CurrentLotteryInfo, LotteryHistoryViewModel>(result);
            }
            return PartialView(model);
        }

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.LotteryHistory;
    }
}