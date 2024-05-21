using JxBackendService.Model.ThirdParty.Base;
using System.Collections.Generic;
using System;

namespace JxBackendService.Model.ThirdParty.WLBG
{
    public class WLBGBetLog : BaseRemoteBetLog
    {
        /// <summary> ⽤户账号 </summary>
        public string Uid { get; set; }

        /// <summary> 游戏类型 </summary>
        public string Game { get; set; }

        /// <summary> 游戏分类。 </summary>
        public string Category { get; set; }

        /// <summary> 系统实际盈利 </summary>
        public decimal Profit { get; set; }

        /// <summary> 结算时，⽤户的最新余额。单位为元，最多两位⼩数（其他⾦额字段也⼀样）。 </summary>
        public decimal Balance { get; set; }

        /// <summary> 投注额。⽤户实际投注⾦额。 </summary>
        public decimal Bet { get; set; }

        /// <summary> 有效投注额。排除⽤户刷⽔之后的投注额。 </summary>
        public decimal ValidBet { get; set; }

        /// <summary> 游戏税收 </summary>
        public decimal Tax { get; set; }

        /// <summary> 游戏开始时间。 </summary>
        public string GameStartTime { get; set; }

        /// <summary> 数据记录时间(结算时间)。 </summary>
        public string RecordTime { get; set; }

        /// <summary> 投注单号 </summary>
        public string GameId { get; set; }

        /// <summary> 记录的全局唯⼀id(每条记录对应唯⼀游戏单号，可⽤于排重) </summary>
        public string RecordId { get; set; }

        /// <summary> 返回投注详情(⽬前仅⽀持视讯百家乐)。 </summary>
        public string Detail { get; set; }

        /// <summary> 投注详情查询url链接。 </summary>
        public string DetailUrl { get; set; }

        public override string KeyId => GameId;

        public override string TPGameAccount => Uid;
    }

    public class WLBGBetRecordLogs
    {
        public List<string> Uid { get; set; }

        public List<string> Game { get; set; }

        public List<string> Profit { get; set; }

        public List<string> Balance { get; set; }

        public List<string> Bet { get; set; }

        public List<string> ValidBet { get; set; }

        public List<string> Tax { get; set; }

        public List<string> GameStartTime { get; set; }

        public List<string> RecordTime { get; set; }

        public List<string> GameId { get; set; }

        public List<string> RecordId { get; set; }

        public List<string> Category { get; set; }
    }

    public class WLBGBetRecordResult
    {
        /// <summary> 实际查询使⽤的开始时间（数据时间大於等於开始时间）</summary>
        public DateTime From { get; set; }

        /// <summary> 实际查询使⽤的结束时间（数据时间小於结束时间）</summary>
        public DateTime Until { get; set; }

        /// <summary> 当前返回携带的数据条数 </summary>
        public int Count { get; set; }

        /// <summary> 此时间范围内，是否有更多数据 </summary>
        public bool HasMore { get; set; }

        /// <summary> 游戏记录内容 </summary>
        public WLBGBetRecordLogs List { get; set; }
    }
}