using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.ViewModel;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using Web.SLPolyGameService;

namespace Web.Services.WebSV.WCF
{
    public class SLPolyGameWCFService : ISLPolyGameWebSVService
    {
        private readonly SLPolyGameService.ISLPolyGameService _slPolyGameService;

        public SLPolyGameWCFService()
        {
            _slPolyGameService = DependencyUtil.ResolveService<ISLPolyGameService>();
        }

        public string CancelOrder(PalyInfo model)
            => _slPolyGameService.CancelOrder(model);

        public CursorPagination<CurrentLotteryInfo> GetCursorPaginationDrawResult(int lotteryId, DateTime start, DateTime end, int count, string cursor)
            => _slPolyGameService.GetCursorPaginationDrawResult(lotteryId, start, end, count, cursor);

        public CursorPagination<PalyInfo> GetFollowBet(string palyId, int lottertId)
            => _slPolyGameService.GetFollowBet(palyId, lottertId);

        public JxBackendService.Model.ViewModel.FrontsideMenuViewModel GetFrontsideMenuViewModel()
            => _slPolyGameService.GetFrontsideMenuViewModel().CastByJson<JxBackendService.Model.ViewModel.FrontsideMenuViewModel>();

        public List<string> GetLatestWinningList(string period)
            => _slPolyGameService.GetLatestWinningList(period);

        public List<WinningListItem> GetLatestWinningListItems(string period)
            => _slPolyGameService.GetLatestWinningListItems(period);

        public PalyInfo GetPalyIDPalyBet(string value)
            => _slPolyGameService.GetPalyIDPalyBet(value);

        public PalyInfo GetPlayBetByAnonymous(string playId)
            => _slPolyGameService.GetPlayBetByAnonymous(playId);

        public PalyInfo[] GetPlayBetsByAnonymous(string startTime, string endTime, string gameId)
            => _slPolyGameService.GetPlayBetsByAnonymous(startTime, endTime, gameId).ToArray();

        public DateTime GetServerCurrentTime()
            => _slPolyGameService.GetServerCurrentTime();

        public CursorPagination<PalyInfo> GetSpecifyOrderList(int userId, int lotteryId, string status, DateTime searchDate, string cursor, int pageSize)
            => _slPolyGameService.GetSpecifyOrderList(userId, lotteryId, status, searchDate, cursor, pageSize);

        public SysSettings GetSysSettings()
            => _slPolyGameService.GetSysSettings();

        public UserInfo GetUserInfo()
            => _slPolyGameService.GetUserInfo();

        public UserInfo GetUserInfoByUserID(int UserID)
            => _slPolyGameService.GetUserInfoByUserID(UserID);

        public PalyInfo InsertPlayInfo(PalyInfo model)
            => _slPolyGameService.InsertPlayInfo(model);

        public bool IsFrontsideMenuActive(string productCode)
            => _slPolyGameService.IsFrontsideMenuActive(productCode);

        public MessageEntity<UserAuthInformation> ValidateLogin(LoginRequestParam param)
            => _slPolyGameService.ValidateLogin(param);
    }
}