using JxLottery.Models.Lottery;
using System;
using System.Collections.Generic;

namespace SLPolyGame.Web.Model
{
    /// <summary>
    /// PalyInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    //[Serializable]
    public class PalyInfo
    {
        /// <summary>
        /// 习惯返点是否变化
        /// </summary>
        public bool BateIsChange { get; set; }

        /// <summary>
        /// 交易ID
        /// </summary>
        public string PalyID { get; set; }

        /// <summary>
        /// 交易期号
        /// </summary>
        public string PalyCurrentNum { get; set; }

        /// <summary>
        /// 购买号码
        /// </summary>
        public string PalyNum { get; set; }

        /// <summary>
        /// 玩法ID
        /// </summary>
        public int? PlayTypeID { get; set; }

        /// <summary>
        /// 彩种类型
        /// </summary>
        public int? LotteryID { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 投注注数
        /// </summary>
        public int? NoteNum { get; set; }

        /// <summary>
        /// 单注注额
        /// </summary>
        public decimal? SingleMoney { get; set; }

        /// <summary>
        /// 投注总额
        /// </summary>
        public decimal? NoteMoney { get; set; }

        /// <summary>
        /// 投注时间
        /// </summary>
        public DateTime? NoteTime { get; set; }

        /// <summary>
        /// 输赢
        /// </summary>
        public bool IsWin { get; set; }

        /// <summary>
        /// 中奖额
        /// </summary>
        public decimal? WinMoney { get; set; }

        /// <summary>
        /// 是否派奖 0表示为开奖，1表示开奖成功，2表示开奖失败,3表示撤单,4正在被开奖
        /// </summary>
        public int IsFactionAward { get; set; }

        /// <summary>
        /// 玩法单选ID
        /// </summary>
        public int? PlayTypeRadioID { get; set; }

        /// <summary>
        /// 所选返点率
        /// </summary>
        public decimal? RebatePro { get; set; }

        /// <summary>
        /// 返点金额
        /// </summary>
        public string RebateProMoney { get; set; }

        /// <summary>
        /// 中奖注数
        /// </summary>
        public int? WinNum { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int? UserID { get; set; }

        /// <summary>
        /// 彩种
        /// </summary>
        public string LotteryType { get; set; }

        /// <summary>
        /// 玩法（四星，五星）
        /// </summary>
        public string PlayTypeName { get; set; }

        /// <summary>
        /// 单选玩法
        /// </summary>
        public string PlayTypeRadioName { get; set; }

        /// <summary>
        /// 當前期号
        /// </summary>
        public string CurrentLotteryNum { get; set; }

        /// <summary>
        /// 亏赢
        /// </summary>
        public decimal WinPossMoney { get; set; }

        /// <summary>
        /// 派獎狀態
        /// </summary>
        public string StFactionAward { get; set; }

        /// <summary>
        /// 该期封单时间
        /// </summary>
        public DateTime? CurrentLotteryTime { get; set; }

        /// <summary>
        /// 追号倍数
        /// </summary>
        public int? Multiple { get; set; }

        /// <summary>
        /// 用户返点
        /// </summary>
        public decimal UserRebatePro { get; set; }

        /// <summary>
        /// Currency Unit (1:元, 0.1:角, 0.01:分, 0.001:厘)
        /// </summary>
        public decimal? CurrencyUnit { get; set; }

        /// <summary>
        /// Ratio (倍數)
        /// </summary>
        public int? Ratio { get; set; }

        /// <summary>
        /// Source Type
        /// </summary>
        public string SourceType { get; set; }

        /// <summary>
        /// 追號明細Id
        /// </summary>
        public string NoticeID { get; set; }

        /// <summary>
        /// 賠率
        /// </summary>
        public string Odds { get; set; }

        /// <summary>
        /// 一般/專家
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// K3使用的狀態碼
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 用戶IP
        /// </summary>
        public string ClientIP { get; set; }

        /// <summary>
        /// 開獎結果
        /// </summary>
        public string ResultJson { get; set; }

        /// <summary>
        /// 房間編號
        /// </summary>
        public string RoomId { get; set; }

        /// <summary>
        /// 結果明細
        /// </summary>
        public Dictionary<string, WinGroupModel> Result { get; set; } = new Dictionary<string, WinGroupModel>();
    }
}