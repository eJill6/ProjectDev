using JxBackendService.Model.ViewModel;

namespace ControllerShareLib.Helpers
{
    public static class CacheKeyHelper
    {
        public static string LatestWinningsKey { get; private set; } = "WEB:LATEST_WINNINGS";

        public static string LatestWinningsWeekKey { get; private set; } = "WEB:LATEST_WINNINGS_WEEK";

        public static string LatestWinningsItemKey { get; private set; } = "WEB:LATEST_WINNINGS_ITEM";

        public static string LatestWinningsItemWeekKey { get; private set; } = "WEB:LATEST_WINNINGS_ITEM_WEEK";

        public static string LatestWinningsFakeWeekKey { get; internal set; } = "WEB:LATEST_WINNINGS_FAKE_WEEK";

        public static string LatestWinningsFakeTokenKey { get; internal set; } = "WEB:LATEST_WINNINGS_FAKE_TOKEN";

        public static string LatestWinningsMonthKey { get; internal set; } = "WEB:LATEST_WINNINGS_MONTH";

        public static string LotteryTypeKey { get; internal set; } = "WEB:LotteryTypeKey";

        public static string PlayTypeInfoKey { get; internal set; } = "WEB:PlayTypeInfoKey";

        public static string PlayTypeRadioKey { get; internal set; } = "WEB:PlayTypeRadioKey";

        public static string Balance { get; internal set; } = "WebTrans#BalanceInfo#{0}";

        public static string UserInfo { get; internal set; } = "WEB#GetOldNewRebateModel:{0}";

        public static string UserInfoWithoutAvailable { get; internal set; } = "WEB#WithoutAvalible:{0}";

        public static string UserToken { get; internal set; } = "WebUserToken:{0}";

        public static string MMUserToken { get; internal set; } = "MMServiceUserToken:{0}";

        public static string RouteToken(string token) => $"RouteToken:{token}";

        public static string LocalUserInfoToken(string userKey) => $"LocalUserInfoToken:{userKey}";

        public static string ApiRebatePro(int userId, int lotteryId, int playTypeId, int playTypeRadioId) => $"ApiRebatePro:{userId}-{lotteryId}-{playTypeId}-{playTypeRadioId}";

        public static string ApiRebatePros { get; internal set; } = "ApiRebatePros:{0}";

        public static string LiveGameManageInfos { get; internal set; } = "LiveGameManageInfos";

        public static string LotteryMenus { get; internal set; } = "LotteryMenus";

        public static string IssueHistoryFirstPage { get; set; } = "IssueHistoryFirstPage_{0}_{1}";

        public static string SVUnauthorized(BasicUserInfo basicUserInfo) => $"SVResponse:{basicUserInfo.UserKey}:{basicUserInfo.UserId}";

    }
}