using JxBackendService.DependencyInjection;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SLPolyGame.Web.Core.Controllers.Base;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;

namespace SLPolyGame.Web.Core.Controllers.Web
{
    public class SLPolyGameServiceController : BaseAuthApiController, ISLPolyGameWebSVService
    {
        private readonly Lazy<ISLPolyGameWebSVService> _slPolyGameWebSVService;

        public SLPolyGameServiceController()
        {
            _slPolyGameWebSVService = DependencyUtil.ResolveService<ISLPolyGameWebSVService>();
        }

        [HttpPost]
        public async Task<string> CancelOrder(PalyInfo model)
            => await _slPolyGameWebSVService.Value.CancelOrder(model);

        [HttpGet, AllowAnonymous]
        public async Task<CursorPagination<CurrentLotteryInfo>> GetCursorPaginationDrawResult(int lotteryId, DateTime start, DateTime end, int count, string? cursor)
            => await _slPolyGameWebSVService.Value.GetCursorPaginationDrawResult(lotteryId, start, end, count, cursor);

        [HttpGet, AllowAnonymous]
        public async Task<CursorPagination<PalyInfo>> GetFollowBet(string palyId, int lottertId)
            => await _slPolyGameWebSVService.Value.GetFollowBet(palyId, lottertId);

        [HttpGet, AllowAnonymous]
        public async Task<List<MenuInnerInfo>> GetMenuInnerInfos()
            => await _slPolyGameWebSVService.Value.GetMenuInnerInfos();

        [HttpGet, AllowAnonymous]
        public async Task<WebGameCenterViewModel> GetWebGameCenterViewModel()
            => await _slPolyGameWebSVService.Value.GetWebGameCenterViewModel();

        [HttpGet, AllowAnonymous]
        public async Task<List<string>> GetLatestWinningList(string period)
            => await _slPolyGameWebSVService.Value.GetLatestWinningList(period);

        [HttpGet, AllowAnonymous]
        public async Task<List<WinningListItem>> GetLatestWinningListItems(string period)
            => await _slPolyGameWebSVService.Value.GetLatestWinningListItems(period);

        [HttpGet]
        public async Task<PalyInfo> GetPalyIDPalyBet(string value)
            => await _slPolyGameWebSVService.Value.GetPalyIDPalyBet(value);

        [HttpGet, AllowAnonymous]
        public async Task<PalyInfo> GetPlayBetByAnonymous(string playId)
            => await _slPolyGameWebSVService.Value.GetPlayBetByAnonymous(playId);

        [HttpGet, AllowAnonymous]
        public async Task<PalyInfo[]> GetPlayBetsByAnonymous(string startTime, string endTime, string? gameId)
            => await _slPolyGameWebSVService.Value.GetPlayBetsByAnonymous(startTime, endTime, gameId);

        [HttpGet]
        public async Task<DateTime> GetServerCurrentTime()
            => await _slPolyGameWebSVService.Value.GetServerCurrentTime();

        [HttpGet]
        public async Task<CursorPaginationTotalData<PalyInfo>> GetSpecifyOrderList(int userId, int? lotteryId, string? status, DateTime searchDate, string? cursor, int pageSize, string? roomId = null)
            => await _slPolyGameWebSVService.Value.GetSpecifyOrderList(userId, lotteryId, status, searchDate, cursor, pageSize, roomId);

        [HttpGet]
        public async Task<SysSettings> GetSysSettings()
            => await _slPolyGameWebSVService.Value.GetSysSettings();

        [HttpGet]
        public async Task<Model.UserInfo> GetUserInfo()
            => await _slPolyGameWebSVService.Value.GetUserInfo();

        [HttpGet]
        public async Task<Model.UserInfo> GetUserInfoByUserID(int UserID)
            => await _slPolyGameWebSVService.Value.GetUserInfoByUserID(UserID);

        /// <summary> 下单 </summary>
        [HttpPost]
        public async Task<Model.PalyInfo> InsertPlayInfo(Model.PalyInfo model)
            => await _slPolyGameWebSVService.Value.InsertPlayInfo(model);

        [HttpPost]
        public async Task<bool> IsFrontsideMenuActive(FrontSideMainMenu frontSideMainMenu)
            => await _slPolyGameWebSVService.Value.IsFrontsideMenuActive(frontSideMainMenu);

        [HttpPost, AllowAnonymous]
        public async Task<MessageEntity<UserAuthInformation>> ValidateLogin(LoginRequestParam param)
            => await _slPolyGameWebSVService.Value.ValidateLogin(param);

        [HttpGet, AllowAnonymous]
        public async Task<int> GetOrCreateGeneratorId(string machineName)
            => await _slPolyGameWebSVService.Value.GetOrCreateGeneratorId(machineName);

        [HttpGet, AllowAnonymous]
        public async Task<Model.UserInfo> GetUserInfoWithoutAvailable(int userId)
            => await _slPolyGameWebSVService.Value.GetUserInfoWithoutAvailable(userId);
    }
}