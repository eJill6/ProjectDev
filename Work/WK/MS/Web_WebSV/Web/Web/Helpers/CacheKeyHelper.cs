using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Helpers
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
        public static string UserToken { get; internal set; } = "WebUserToken:{0}";
        public static string MMUserToken { get; internal set; } = "MMServiceUserToken:{0}";
    }
}