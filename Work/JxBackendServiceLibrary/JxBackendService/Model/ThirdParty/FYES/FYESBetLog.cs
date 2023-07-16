using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Resource.Element;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JxBackendService.Model.ThirdParty.FYES
{
    public class FYESBetLog : BaseRemoteBetLog
    {
        /// <summary> 订单号 </summary>
        public string OrderID { get; set; }

        /// <summary> 用户名 </summary>
        public string UserName { get; set; }

        /// <summary> 游戏名称 </summary>
        public string Category { get; set; }

        /// <summary> 订单类型 </summary>
        public string Type { get; set; }

        /// <summary> 订单状态 </summary>
        public string Status { get; set; }

        /// <summary> 投注金额 </summary>
        public decimal BetAmount { get; set; }

        /// <summary> 有效投注金额 </summary>
        public decimal BetMoney { get; set; }

        /// <summary> 盈亏金额 </summary>
        public decimal Money { get; set; }

        /// <summary> 盘口类型 </summary>
        public string OddsType { get; set; }

        /// <summary> 赔率 </summary>
        public decimal Odds { get; set; }

        /// <summary> 下单时间 </summary>
        public DateTime CreateAt { get; set; }

        /// <summary> 结果产生时间 </summary>
        public DateTime? ResultAt { get; set; }

        /// <summary> 派奖时间 </summary>
        public DateTime? RewardAt { get; set; }

        /// <summary> 订单数据的更新时间 </summary>
        public DateTime UpdateAt { get; set; }

        /// <summary> 订单更新的时间戳 </summary>
        public decimal Timestamp { get; set; }

        /// <summary> 投注IP </summary>
        public string IP { get; set; }

        /// <summary> 语言环境 </summary>
        public string Language { get; set; }

        /// <summary> 设备 </summary>
        public List<string> Platform { get; set; }

        /// <summary> 币种 </summary>
        public string Currency { get; set; }

        /// <summary> 是否是测试订单 </summary>
        public bool IsTest { get; set; }

        /// <summary> 游戏代码（Type为趣味游戏时专用） </summary>
        public string Code { get; set; }

        /// <summary> 串关细项 </summary>
        public List<FYESBetLogDetail> Details { get; set; }

        public override string KeyId => OrderID;

        public override string TPGameAccount => UserName;
    }

    public class FYESBetLogDetail
    {
        /// <summary> 子订单ID </summary>
        public string DetailID { get; set; }

        /// <summary> 游戏ID </summary>
        public string CateID { get; set; }

        /// <summary> 游戏名称 </summary>
        public string Category { get; set; }

        /// <summary> 联赛ID </summary>
        public string LeagueID { get; set; }

        /// <summary> 联赛名称 </summary>
        public string League { get; set; }

        /// <summary> 比赛ID </summary>
        public string MatchID { get; set; }

        /// <summary> 比赛标题 </summary>
        public string Match { get; set; }

        /// <summary> 盘口ID </summary>
        public string BetID { get; set; }

        /// <summary> 盘口名称 </summary>
        public string Bet { get; set; }

        /// <summary> 投注内容 </summary>
        public string Content { get; set; }

        /// <summary> 开奖结果 </summary>
        public string Result { get; set; }

        /// <summary> 盘口类型 </summary>
        public string OddsType { get; set; }

        /// <summary> 赔率 </summary>
        public decimal Odds { get; set; }

        /// <summary> 订单状态 </summary>
        public string Status { get; set; }

        /// <summary> 开始时间 </summary>
        public DateTime? StartAt { get; set; }

        /// <summary> 结束时间 </summary>
        public DateTime? EndAt { get; set; }

        /// <summary> 结果产生时间 </summary>
        public DateTime? ResultAt { get; set; }
    }

    public class GetBetLogResponseModel
    {
        public int RecordCount { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public List<FYESBetLog> list { get; set; }
    }

    public class FYESGetBetLogResponseModel : FYESBaseInfoModel<GetBetLogResponseModel>
    {
    }

    /// <summary> 订单状态 </summary>
    public class FYESBetLogStatus : BaseStringValueModel<FYESBetLogStatus>
    {
        public bool IsSettled { get; private set; }

        public static FYESBetLogStatus Processing = new FYESBetLogStatus()
        {
            Value = "None",
        };

        public static FYESBetLogStatus Cancel = new FYESBetLogStatus()
        {
            Value = "Cancel",
        };

        public static FYESBetLogStatus Win = new FYESBetLogStatus()
        {
            Value = "Win",
            IsSettled = true,
        };

        public static FYESBetLogStatus Lose = new FYESBetLogStatus()
        {
            Value = "Lose",
            IsSettled = true,
        };

        public static FYESBetLogStatus Revoke = new FYESBetLogStatus()
        {
            Value = "Revoke",
        };
    }

    /// <summary> 订单类型 </summary>
    public class FYESBetType : BaseStringValueModel<FYESBetType>
    {
        /// <summary> 电竞单关订单 </summary>
        public static FYESBetType Single = new FYESBetType()
        {
            Value = "Single",
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.FYESBetType_Single),
        };

        /// <summary> 电竞串关订单 </summary>
        public static FYESBetType Combo = new FYESBetType()
        {
            Value = "Combo",
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.FYESBetType_Combo),
        };

        /// <summary> 趣味游戏订单 </summary>
        public static FYESBetType Smart = new FYESBetType()
        {
            Value = "Smart",
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.FYESBetType_Smart),
        };

        /// <summary> 主播订单 </summary>
        public static FYESBetType Anchor = new FYESBetType()
        {
            Value = "Anchor",
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.FYESBetType_Anchor),
        };

        /// <summary> 虚拟电竞订单 </summary>
        public static FYESBetType VisualSport = new FYESBetType()
        {
            Value = "VisualSport",
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.FYESBetType_VisualSport),
        };
    }
}