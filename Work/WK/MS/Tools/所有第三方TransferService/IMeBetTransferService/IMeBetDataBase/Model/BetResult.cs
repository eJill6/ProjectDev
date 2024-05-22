using System;
using JxBackendService.Model.ViewModel.ThirdParty.Old;

namespace IMeBetDataBase.Model
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
        /// 游戏名称
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// 中文游戏名称
        /// </summary>
        public string ChineseGameName { get; set; }

        /// <summary>
        /// 下注类别
        /// </summary>
        public string BetType { get; set; }

        /// <summary>
        /// 注單編號
        /// </summary>
        public string BetId { get; set; }

        /// <summary>
        /// 供应商所提供的有别于 BetId 的额外注单号
        /// </summary>
        public string ExternalBetId { get; set; }

        /// <summary>
        /// 注单的局号
        /// </summary>
        public string RoundId { get; set; }

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
        public decimal BetAmount { get; set; }

        /// <summary>
        /// 和局以外的下注金额
        /// </summary>
        public decimal ValidBet { get; set; }

        /// <summary>
        /// 玩家给的打赏(贡献)金额。如打赏功能不适用于相关游戏，则它的值会是 0。
        /// </summary>
        public decimal Tips { get; set; }

        /// <summary>
        /// 输赢
        /// </summary>
        public string WinLoss { get; set; }

        /// <summary>
        /// 供应商支付的奖金额
        /// </summary>
        public decimal ProviderBonus { get; set; }

        /// <summary>
        /// 供应商支付的奖金额
        /// </summary>
        public decimal ProviderTourFee { get; set; }

        /// <summary>
        /// 赛后供应商回退玩家的相关金额
        /// </summary>
        public decimal ProviderTourRefund { get; set; }

        /// <summary>
        /// 注单状态: Open, Settled, Cancelled.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        ///
        /// 下注平台
        /// 选项:Desktop; Mobile; Mini Games; Download; N/A;
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// 收到下注的时间戳
        /// </summary>
        public string BetDate { get; set; }

        /// <summary>
        /// 注单最后更新时间格式: YYYY-MM-DD HH:mm:ss +08:00
        /// </summary>
        public string ReportingDate { get; set; }

        /// <summary>
        /// IMOne 系统在收到下注单是的创建时间戳
        /// </summary>
        public string DateCreated { get; set; }

        /// <summary>
        /// 注单最后更新时间格式: YYYY-MM-DD HH:mm:ss +08:00
        /// </summary>
        public string LastUpdatedDate { get; set; }
    }
}