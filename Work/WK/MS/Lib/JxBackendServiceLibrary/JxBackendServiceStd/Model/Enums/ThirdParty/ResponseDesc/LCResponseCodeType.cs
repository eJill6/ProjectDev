using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.ThirdParty
{
    public class LCResponseCodeType : BaseIntValueModel<LCResponseCodeType>
    {
        private LCResponseCodeType()
        {
            ResourceType = typeof(TPResponseCodeElement);
        }

        /// <summary> 渠道白名单错误（请联系客服添加服务器白名单） </summary>
        public static readonly LCResponseCodeType LC_5 = new LCResponseCodeType()
        {
            Value = 5,
            ResourcePropertyName = nameof(TPResponseCodeElement.LC_5)
        };

        /// <summary> 账号禁用 </summary>
        public static readonly LCResponseCodeType LC_20 = new LCResponseCodeType()
        {
            Value = 20,
            ResourcePropertyName = nameof(TPResponseCodeElement.LC_20)
        };

        /// <summary> 更新玩家金币失败(用户游戏中，不允许转帐) </summary>
        public static readonly LCResponseCodeType LC_26 = new LCResponseCodeType()
        {
            Value = 26,
            ResourcePropertyName = nameof(TPResponseCodeElement.LC_26)
        };

        /// <summary> 数据库异常 </summary>
        public static readonly LCResponseCodeType LC_27 = new LCResponseCodeType()
        {
            Value = 27,
            ResourcePropertyName = nameof(TPResponseCodeElement.LC_27)
        };

        /// <summary> ip 禁用 </summary>
        public static readonly LCResponseCodeType LC_28 = new LCResponseCodeType()
        {
            Value = 28,
            ResourcePropertyName = nameof(TPResponseCodeElement.LC_28)
        };

        /// <summary> 更新玩家金币失败(用户游戏中，不允许转帐) </summary>
        public static readonly LCResponseCodeType LC_33 = new LCResponseCodeType()
        {
            Value = 33,
            ResourcePropertyName = nameof(TPResponseCodeElement.LC_33)
        };

        /// <summary> 订单重复 </summary>
        public static readonly LCResponseCodeType LC_34 = new LCResponseCodeType()
        {
            Value = 34,
            ResourcePropertyName = nameof(TPResponseCodeElement.LC_34)
        };

        /// <summary> 获取玩家信息失败（请调用登录接口创建账号） </summary>
        public static readonly LCResponseCodeType LC_35 = new LCResponseCodeType()
        {
            Value = 35,
            ResourcePropertyName = nameof(TPResponseCodeElement.LC_35)
        };

        /// <summary> 余额不足导致下分失败 </summary>
        public static readonly LCResponseCodeType LC_38 = new LCResponseCodeType()
        {
            Value = 38,
            ResourcePropertyName = nameof(TPResponseCodeElement.LC_38)
        };

        /// <summary> 禁止同一账号登录带分、上分、下分并发请求，后一个请求被拒 </summary>
        public static readonly LCResponseCodeType LC_39 = new LCResponseCodeType()
        {
            Value = 39,
            ResourcePropertyName = nameof(TPResponseCodeElement.LC_39)
        };

        /// <summary> 代理商金额不足 </summary>
        public static readonly LCResponseCodeType LC_1002 = new LCResponseCodeType()
        {
            Value = 1002,
            ResourcePropertyName = nameof(TPResponseCodeElement.LC_1002)
        };

        /// <summary> 玩家正在游戏中，不能上下分 </summary>
        public static readonly LCResponseCodeType LC_1023 = new LCResponseCodeType()
        {
            Value = 1023,
            ResourcePropertyName = nameof(TPResponseCodeElement.LC_1023)
        };
    }
}