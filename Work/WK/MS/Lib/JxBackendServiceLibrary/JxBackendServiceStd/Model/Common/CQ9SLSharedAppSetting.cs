using JxBackendService.Model.Paging;
using System;

namespace JxBackendService.Model.Common
{
    public class CQ9SLSharedAppSetting : SharedAppSettings
    {
        private CQ9SLSharedAppSetting()
        { }

        public static CQ9SLSharedAppSetting Instance = new CQ9SLSharedAppSetting();

        public int MaxSearchRangeMinutes => 180;

        public TimeSpan TimeZoneOffset => new TimeSpan(hours: -4, minutes: 0, seconds: 0);

        public int QueryBetLogPageSize => 20000;

        public int QueryPagedBetLogIntervalSeconds => 5;

        public string AuthorizationHeaderName => "Authorization";

        public CQ9SLQueryStringKey QueryStringKey => new CQ9SLQueryStringKey();

        #region API Paths

        /// <summary>建立 Player</summary>
        public string CreatePlayerPath => "/gameboy/player";

        /// <summary>Player登入</summary>
        public string PlayerLoginPath => "/gameboy/player/login";

        /// <summary>Player 取得遊戲大廳連結</summary>
        public string PlayerLobbyLinkPath => "/gameboy/player/lobbylink";

        /// <summary>Player 取得遊戲連結</summary>
        public string PlayerGameLinkPath => "/gameboy/player/gamelink";

        /// <summary>從玩家錢包取款</summary>
        public string PlayerWithdrawPath => "/gameboy/player/withdraw";

        /// <summary>存款至玩家錢包</summary>
        public string PlayerDepositPath => "/gameboy/player/deposit";

        /// <summary>查詢玩家錢包餘額</summary>
        public string GetPlayerBalancePath(string tpGameAccount) => $"/gameboy/player/balance/{tpGameAccount}";

        /// <summary>以交易單號查詢已存取款成功之單筆紀錄</summary>
        public string GetQueryTransactionRecordPath(string orderId) => $"/gameboy/transaction/record/{orderId}";

        /// <summary>登出代理單一玩家</summary>
        public string PlayerLogoutPath => "/gameboy/player/logout";

        /// <summary>查詢玩家帳號是否存在</summary>
        public string GetPlayerCheckAccountPath(string tpGameAccount) => $"/gameboy/player/check/{tpGameAccount}";

        /// <summary>以時間查詢玩家已派彩注單，搜尋區間不可超過24H，可跨日</summary>
        public string QueryBetLogPath => "/gameboy/order/view";

        #endregion API Paths

        public string APIUrl => Get("TPGame.CQ9SL.APIUrl");

        public string Authorization => Get("TPGame.CQ9SL.Authorization");

        public string Lang => "zh-cn";

        public string Gamehall => "cq9";

        public int OverlapMinutesPerRequest => 15;
    }

    public class CQ9SLQueryStringKey
    {
        public string StartTime => "starttime";

        public string EndTime => "endtime";

        public string Page => "page";

        public string PageSize => "pagesize";

        public string UserToken => "usertoken";

        public string Lang => "lang";

        public string App => "app";

        public string Gamehall => "gamehall";

        public string Gamecode => "gamecode";

        public string Gameplat => "gameplat";

        public string Account = "account";

        public string Password => "password";

        public string Mtcode => "mtcode";

        public string Amount => "amount";
    }
}