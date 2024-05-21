using JxBackendService.Common.Util;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.ThirdParty
{
    public class WLResponseReasonType : BaseStringValueModel<WLResponseReasonType>
    {
        private WLResponseReasonType()
        {
            ResourceType = typeof(TPResponseCodeElement);
        }

        /// <summary>（下分）⽤户余额不⾜</summary>
        public static readonly WLResponseReasonType CreditNotEnough = new WLResponseReasonType()
        {
            Value = "credit_not_enough",
            ResourcePropertyName = nameof(TPResponseCodeElement.WL_CreditNotEnough)
        };

        /// <summary>⽤户被⽡⼒封禁</summary>
        public static readonly WLResponseReasonType UserBanned = new WLResponseReasonType()
        {
            Value = "user_banned",
            ResourcePropertyName = nameof(TPResponseCodeElement.WL_UserBanned)
        };

        /// <summary>对局类游戏不允许⽤户在游戏内时上下分。如⽃地主、抢庄⽜⽜等
        /// 商户可提⽰⽤户，退出这些游戏后再划拨</summary>
        public static readonly WLResponseReasonType GameLimit = new WLResponseReasonType()
        {
            Value = "game_limit",
            ResourcePropertyName = nameof(TPResponseCodeElement.WL_GameLimit)
        };

        /// <summary>（上分）商户额度不⾜。
        /// 每个商户能给⽤户上分的总量有限制。可以在后台看到⽬前的剩余额度
        /// 上分消耗额度，下分归还额度。额度不⾜时，需要联系⽡⼒加额度</summary>
        public static readonly WLResponseReasonType AgentCreditNotEnough = new WLResponseReasonType()
        {
            Value = "agent_credit_not_enough",
            ResourcePropertyName = nameof(TPResponseCodeElement.WL_AgentCreditNotEnough)
        };

        /// <summary>单号中的时间不对
        /// 划拨接⼜只⽀持5分钟内的订单，划拨查询接⼜只⽀持⼀个⽉内的订单，不应超过此范围</summary>
        public static readonly WLResponseReasonType IllegalTime = new WLResponseReasonType()
        {
            Value = "illegal_time",
            ResourcePropertyName = nameof(TPResponseCodeElement.WL_IllegalTime)
        };

        public static string CombineMessage(string reason)
        {
            string message = GetName(reason);

            if (message.IsNullOrEmpty())
            {
                return reason;
            }

            return $"{message}[{reason}]";
        }
    }
}