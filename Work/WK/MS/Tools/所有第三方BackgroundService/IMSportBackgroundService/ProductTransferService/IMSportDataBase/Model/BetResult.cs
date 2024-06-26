﻿using JxBackendService.Model.ViewModel.ThirdParty.Old;

namespace IMSportDataBase.Model
{
    /// <summary>
    /// 第三方注單明細
    /// </summary>
    public class BetLogResult<T> : ApiResult, IOldBetLogModel
    {
        public T Result { get; set; }

        public PageInfo Pagination { get; set; }

        public string RemoteFileSeq { get; set; }

        public Action WriteRemoteContentToOtherMerchant { get; set; }
    }

    public class PageInfo
    {
        public int CurrentPage { get; set; }

        public int TotalPage { get; set; }

        public int ItemPerPage { get; set; }

        public int TotalCount { get; set; }
    }

    public class BetResult
    {
        /// <summary>
        /// 產品供應商代碼 (IM 體育回傳是 IMSB_SB)
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// IMOne 系统内的 GameID
        /// </summary>
        public string GameId { get; set; }

        /// <summary>
        /// 注單編號
        /// </summary>
        public string BetId { get; set; }

        /// <summary>
        /// 注单创建时间 格式: YYYY-MM-DD HH:mm:ss +08:00
        /// </summary>
        public string WagerCreationDateTime { get; set; }

        /// <summary>
        /// 玩家账号
        /// </summary>
        public string PlayerId { get; set; }

        /// <summary>
        /// 产品提供商玩家账号
        /// </summary>
        public string ProviderPlayerId { get; set; }

        /// <summary>
        /// 货币代码
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 下注金额
        /// </summary>
        public string StakeAmount { get; set; }

        /// <summary>
        /// 会员流水
        /// </summary>
        public string TurnOver { get; set; }

        /// <summary>
        /// 实际扣除金额
        /// </summary>
        public decimal MemberExposure { get; set; }

        /// <summary>
        /// 可赢金额
        /// </summary>
        public decimal PayoutAmount { get; set; }

        /// <summary>
        /// 输赢
        /// </summary>
        public string WinLoss { get; set; }

        /// <summary>
        /// 下注结果
        /// </summary>
        public string ResultStatus { get; set; }

        /// <summary>
        /// 赔率类别: HK 香港盘 / EURO 欧洲盘 / MALAY 马来盘 / INDO 印尼盘
        /// </summary>
        public string OddsType { get; set; }

        /// <summary>
        /// 注单类别: Single 单一 / Combo 混合过关
        /// </summary>
        public string WagerType { get; set; }

        /// <summary>
        /// 平台类别: Web 电脑 / Mobile 手机
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// 0 = Not Settled 未结算, 1 = Settled 已结算
        /// </summary>
        public int IsSettled { get; set; }

        /// <summary>
        /// 0 = Pending 待定, 1 = Confirmed 确认, 2 = Cancelled 取消
        /// </summary>
        public int IsConfirmed { get; set; }

        /// <summary>
        /// 0 = Not cancel 未取消, 1 = Cancel 已取消
        /// </summary>
        public int IsCancelled { get; set; }

        /// <summary>
        /// BetTrade 状态: Cancel 取消 / Sold 兑现成功如果没有此单据则回传 Null。
        /// </summary>
        public string BetTradeStatus { get; set; }

        /// <summary>
        /// 此属性已已不再使用。会一直返回0。
        /// </summary>
        public decimal BetTradeCommission { get; set; }

        /// <summary>
        /// BetTrade 金额 (buy back amount)。如果没有cash out则回传 0。
        /// </summary>
        public decimal BetTradeBuybackAmount { get; set; }

        /// <summary>
        /// 混合过关类别: Doubles 2 串 1,
        /// Trebles 3 串 1,
        /// Trixie 3 串 4,
        /// Yankee 4 串 11,
        /// SuperYankee 5 串 26,
        /// Heinz 6 串 57,
        /// SuperHeinz 7 串 120,
        /// Goliath 8 串 247,
        /// Block9 9 串 502,
        /// Block10 10 串 1013,
        /// FourFolds 4 串 1,
        /// FiveFolds 5 串 1,
        /// SixFolds 6 串 1,
        /// SevenFolds 7 串 1,
        /// EightFolds 8 串 1,
        /// NineFolds 9 串 1,
        /// TenFolds 10 串 1
        /// WagerType = Single 则返回 None。
        /// </summary>
        public string ComboType { get; set; }

        /// <summary>
        /// 注单最后更新时间格式: YYYY-MM-DD HH:mm:ss +08:00
        /// </summary>
        public string LastUpdatedDate { get; set; }

        /// <summary>
        /// 細節內容
        /// </summary>
        public List<DetailItem> DetailItems { get; set; }
    }

    public class DetailItem
    {
        public string Market { get; set; }

        public string EventName { get; set; }

        public string EventDateTime { get; set; }

        public string CompetitionName { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        public string FavTeam { get; set; }

        public string BetType { get; set; }

        public string BetTypeDesc { get; set; }

        public string Perod { get; set; }

        public string Selection { get; set; }

        public string Odds { get; set; }

        public string HomeTeamHTScore { get; set; }

        public string AwayTeamHTScore { get; set; }

        public string HomeTeamFTScore { get; set; }

        public string AwayTeamFTScore { get; set; }

        public string WagerHomeTeamScore { get; set; }

        public string WagerAwayTeamScore { get; set; }

        public string Handicap { get; set; }

        public string IsWagerItemCancelled { get; set; }

        public string SportsName { get; set; }

        public string EventId { get; set; }

        public string Specifier { get; set; }
    }
}