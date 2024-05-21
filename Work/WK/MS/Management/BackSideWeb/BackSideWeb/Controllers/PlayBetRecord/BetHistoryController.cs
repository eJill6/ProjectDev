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
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using Microsoft.AspNetCore.Mvc;
using MS.Core.MMModel.Models.PlayBetRecord.Enum;

namespace BackSideWeb.Controllers.PlayBetRecord
{
    /// <summary>
    /// 秘色彩票投注
    /// </summary>
    public class BetHistoryController : BaseSearchGridController<PalyInfoParam>
    {
        private readonly Lazy<IPalyInfoRep> _palyInfoRep;

        public BetHistoryController()
        {
            _palyInfoRep = DependencyUtil.ResolveJxBackendService<IPalyInfoRep>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        protected override string[] PageJavaScripts => new string[]
        {
            "business/playBetRecord/betHistorySearchParam.min.js",
            "business/playBetRecord/betHistorySearchService.min.js"
        };

        protected override string ClientServiceName => "betHistorySearchService";

        public override ActionResult Index()
        {
            BetHistoryViewModel model = new BetHistoryViewModel()
            {
                LotteryIdSelectList = MsHelpers.GetMsLotterySelectListItem(),
                IsFactionAwardSelectList = EnumHelper.GetEnumAll<IsFactionAwardEnum>(),
                IsWinSelectList = EnumHelper.GetEnumAll<IsWinEnum>()
            };
            return View(model);
        }

        public override ActionResult GetGridViewResult(PalyInfoParam param)
        {
            PagedResultModel<BetHistoryViewModel> model = new PagedResultModel<BetHistoryViewModel>();
            param.EndDate = param.EndDate.AddDays(1);
            var result = _palyInfoRep.Value.GetPalyInfoReport(param);
            if (result != null)
            {
                model = MMHelpers.MapPagedResultModel<PalyInfo, BetHistoryViewModel>(result);
                foreach (var item in model.ResultList)
                {
                    string compressedPalyNum = item.PalyNum;
                    string decompressedPalyNum = ZipHelper.Decompress(compressedPalyNum);
                    item.PalyNum = decompressedPalyNum;
                }
                decimal? pageNoteMoney = 0;
                decimal? pageWinMoney = 0;
                pageNoteMoney = model.ResultList.Sum(r => r.NoteMoney);
                pageWinMoney = model.ResultList.Sum(r => r.WinMoney);
                @ViewBag.PageNoteMoney = pageNoteMoney.HasValue ? pageNoteMoney.Value.ToString("N2") : "0.00";
                @ViewBag.PageWinMoney = pageWinMoney.HasValue ? pageWinMoney.Value.ToString("N2") : "0.00";
            }
            return PartialView(model);
        }

        public IActionResult Detail(string keyContent)
        {
            SetLayout(LayoutType.Base);
            var result = _palyInfoRep.Value.GetPalyInfoDetail(keyContent);
            BetHistoryViewModel model = MMHelpers.MapModel<PalyInfo, BetHistoryViewModel>(result);
            string compressedPalyNum = model.PalyNum;
            model.PalyNum = ZipHelper.Decompress(compressedPalyNum);
            return View(model);
        }

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.BetHistory;
    }
}