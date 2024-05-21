using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.ThirdParty
{
    public class IMBGResponseCodeType : BaseIntValueModel<IMBGResponseCodeType>
    {
        private IMBGResponseCodeType()
        {
            ResourceType = typeof(TPResponseCodeElement);
        }

        /// <summary> userCode 有误，在平台服务器找不到对应的用户 </summary>
        public static readonly IMBGResponseCodeType IMBG_1012 = new IMBGResponseCodeType()
        {
            Value = 1012,
            ResourcePropertyName = nameof(TPResponseCodeElement.IMBG_1012)
        };

        /// <summary> 用户还在游戏中，下分失败 </summary>
        public static readonly IMBGResponseCodeType IMBG_1013 = new IMBGResponseCodeType()
        {
            Value = 1013,
            ResourcePropertyName = nameof(TPResponseCodeElement.IMBG_1013)
        };

        /// <summary> 用户的分数不足 </summary>
        public static readonly IMBGResponseCodeType IMBG_1014 = new IMBGResponseCodeType()
        {
            Value = 1014,
            ResourcePropertyName = nameof(TPResponseCodeElement.IMBG_1014)
        };

        /// <summary> 用户的可下分数不足 </summary>
        public static readonly IMBGResponseCodeType IMBG_1015 = new IMBGResponseCodeType()
        {
            Value = 1015,
            ResourcePropertyName = nameof(TPResponseCodeElement.IMBG_1015)
        };

        /// <summary> 订单号不存在 </summary>
        public static readonly IMBGResponseCodeType IMBG_1016 = new IMBGResponseCodeType()
        {
            Value = 1016,
            ResourcePropertyName = nameof(TPResponseCodeElement.IMBG_1016)
        };

        /// <summary> 用户被锁定，不能进行上下分操作 </summary>
        public static readonly IMBGResponseCodeType IMBG_1019 = new IMBGResponseCodeType()
        {
            Value = 1019,
            ResourcePropertyName = nameof(TPResponseCodeElement.IMBG_1019)
        };

        /// <summary> 该代理商的可用分数不足，上分失败 </summary>
        public static readonly IMBGResponseCodeType IMBG_1029 = new IMBGResponseCodeType()
        {
            Value = 1029,
            ResourcePropertyName = nameof(TPResponseCodeElement.IMBG_1029)
        };
    }
}