using System;
using System.Collections.Generic;
using System.ServiceModel;
using JxBackendService.Model.ViewModel;
using SLPolyGame.Web.Behavior;
using SLPolyGame.Web.Model;

namespace SLPolyGame.Web
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ISLPolyGameService”。
    [ServiceContract]
    public interface ISLPolyGameService
    {
        [OperationContract]
        [MOperationBehavior]
        string CancelOrder(Model.PalyInfo model);

        [OperationContract]
        [MOperationBehavior]
        SLPolyGame.Web.Model.UserInfo GetUserInfoByUserID(int UserID);

        [OperationContract]
        SLPolyGame.Web.Model.MessageEntity<SLPolyGame.Web.Model.UserAuthInformation> ValidateLogin(LoginRequestParam param);

        [OperationContract]
        [MOperationBehavior]
        DateTime GetServerCurrentTime();

        [OperationContract]
        [MOperationBehavior]
        SLPolyGame.Web.Model.SysSettings GetSysSettings();

        [OperationContract]
        [MOperationBehavior]
        SLPolyGame.Web.Model.PalyInfo InsertPlayInfo(Model.PalyInfo model);

        [OperationContract]
        [MOperationBehavior]
        SLPolyGame.Web.Model.PalyInfo GetPalyIDPalyBet(string value);

        [OperationContract]
        [MOperationBehavior]
        CursorPagination<CurrentLotteryInfo> GetCursorPaginationDrawResult(int lotteryId, DateTime start, DateTime end, int count, string cursor);

        [OperationContract]
        [MOperationBehavior]
        CursorPagination<PalyInfo> GetFollowBet(string palyId, int lottertId);

        [OperationContract]
        [MOperationBehavior]
        CursorPagination<PalyInfo> GetSpecifyOrderList(int userId, int lotteryId, string status, DateTime searchDate, string cursor, int pageSize);

        [OperationContract]
        [MOperationBehavior]
        UserInfo GetUserInfo();

        [OperationContract]
        List<WinningListItem> GetLatestWinningListItems(string period);

        [OperationContract]
        List<string> GetLatestWinningList(string period);

        [OperationContract]
        SLPolyGame.Web.Model.PalyInfo[] GetPlayBetsByAnonymous(string startTime, string endTime, string gameId);

        [OperationContract]
        SLPolyGame.Web.Model.PalyInfo GetPlayBetByAnonymous(string playId);

        [OperationContract, MOperationBehavior]
        FrontsideMenuViewModel GetFrontsideMenuViewModel();

        [OperationContract, MOperationBehavior]
        bool IsFrontsideMenuActive(string productCode);
    }
}