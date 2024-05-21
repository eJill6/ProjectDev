using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Menu;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLPolyGame.Web.Interface
{
    public interface ISLPolyGameWebSVService
    {
        Task<string> CancelOrder(PalyInfo model);

        Task<UserInfo> GetUserInfoByUserID(int UserID);

        Task<MessageEntity<UserAuthInformation>> ValidateLogin(LoginRequestParam param);

        Task<DateTime> GetServerCurrentTime();

        Task<SysSettings> GetSysSettings();

        Task<PalyInfo> InsertPlayInfo(PalyInfo model);

        Task<PalyInfo> GetPalyIDPalyBet(string value);

        Task<CursorPagination<CurrentLotteryInfo>> GetCursorPaginationDrawResult(int lotteryId, DateTime start, DateTime end, int count, string cursor);

        Task<CursorPagination<PalyInfo>> GetFollowBet(string palyId, int lottertId);

        Task<CursorPaginationTotalData<PalyInfo>> GetSpecifyOrderList(int userId, int? lotteryId, string status, DateTime searchDate, string cursor, int pageSize, string roomId);

        Task<UserInfo> GetUserInfo();

        Task<List<WinningListItem>> GetLatestWinningListItems(string period);

        Task<List<string>> GetLatestWinningList(string period);

        Task<PalyInfo[]> GetPlayBetsByAnonymous(string startTime, string endTime, string gameId);

        Task<PalyInfo> GetPlayBetByAnonymous(string playId);

        Task<List<MenuInnerInfo>> GetMenuInnerInfos();

        Task<WebGameCenterViewModel> GetWebGameCenterViewModel();

        Task<bool> IsFrontsideMenuActive(FrontSideMainMenu frontSideMainMenu);

        Task<int> GetOrCreateGeneratorId(string machineName);

        Task<UserInfo> GetUserInfoWithoutAvailable(int userId);
    }
}