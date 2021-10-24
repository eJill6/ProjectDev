using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Finance.Apollo
{
    public class ApolloServiceType : BaseStringValueModel<ApolloServiceType>
    {
        /// <summary>是否為充值服務</summary>
        public bool IsRechargeService { get; private set; } = true;

        /// <summary>是否輸入金額</summary>
        public bool IsInputAmount { get; private set; }

        /// <summary>是否導轉至第三方,若為否則平台呈現收款資訊</summary>
        public bool IsRedirectToOuterPage { get; private set; }

        /// <summary>是否要驗證用戶綁定銀行卡</summary>
        public bool IsUserBankCardRequired { get; private set; }

        public int ConfigSettingItemKey { get; private set; }

        private ApolloServiceType()
        {
            ResourceType = typeof(SelectItemElement);
        }

        #region 充值
        /// <summary>銀行卡對卡</summary>
        public static readonly ApolloServiceType DirectPay = new ApolloServiceType()
        {
            Value = "DirectPay",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_DirectPay),
            IsInputAmount = true,
            IsRedirectToOuterPage = false,
            IsUserBankCardRequired = true,
            ConfigSettingItemKey = 0,
        };

        /// <summary>銀行實名制卡對卡</summary>
        public static readonly ApolloServiceType DirectPayRealName = new ApolloServiceType()
        {
            Value = "DirectPayRealName",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_DirectPayRealName),
            IsInputAmount = false,
            IsRedirectToOuterPage = true,
            ConfigSettingItemKey = 31,
        };

        /// <summary>網銀在線</summary>
        public static readonly ApolloServiceType WebATM = new ApolloServiceType()
        {
            Value = "WebATM",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_WebATM),
            IsInputAmount = true,
            IsRedirectToOuterPage = true,
            ConfigSettingItemKey = 1,
        };

        /// <summary>網銀在線(Wap)</summary>
        public static readonly ApolloServiceType WapBank = new ApolloServiceType()
        {
            Value = "WapBank",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_WapBank),
            IsInputAmount = true,
            IsRedirectToOuterPage = true,
            ConfigSettingItemKey = 11,
        };

        /// <summary>微信 (轉卡)</summary>
        public static readonly ApolloServiceType WeiXin = new ApolloServiceType()
        {
            Value = "WeiXin",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_WeiXin),
            IsInputAmount = true,
            IsRedirectToOuterPage = false,
            IsUserBankCardRequired = true,
            ConfigSettingItemKey = 10,
        };

        /// <summary>微信 (掃碼)</summary>
        public static readonly ApolloServiceType WeiXinScan = new ApolloServiceType()
        {
            Value = "WeiXinScan",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_WeiXinScan),
            IsInputAmount = true,
            IsRedirectToOuterPage = true,
            ConfigSettingItemKey = 3,
        };

        /// <summary>微信 (一鍵)</summary>
        public static readonly ApolloServiceType WeiXinH5 = new ApolloServiceType()
        {
            Value = "WeiXinH5",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_WeiXinH5),
            IsInputAmount = true,
            IsRedirectToOuterPage = true,
            ConfigSettingItemKey = 13,
        };

        /// <summary>微信 (固定金額)</summary>
        public static readonly ApolloServiceType WeiXinPdd = new ApolloServiceType()
        {
            Value = "WeiXinPdd",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_WeiXinPdd),
            IsInputAmount = false,
            IsRedirectToOuterPage = true,
            ConfigSettingItemKey = 23,
        };

        /// <summary>支付寶 (轉卡)</summary>
        public static readonly ApolloServiceType Alipay = new ApolloServiceType()
        {
            Value = "Alipay",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_Alipay),
            IsInputAmount = true,
            IsRedirectToOuterPage = false,
            IsUserBankCardRequired = true,
            ConfigSettingItemKey = 2,
        };

        /// <summary>支付寶 (掃碼)</summary>
        public static readonly ApolloServiceType AlipayScan = new ApolloServiceType()
        {
            Value = "AlipayScan",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_AlipayScan),
            IsInputAmount = true,
            IsRedirectToOuterPage = true,
            ConfigSettingItemKey = 5,
        };

        /// <summary>支付寶 (一鍵)</summary>
        public static readonly ApolloServiceType AlipayH5 = new ApolloServiceType()
        {
            Value = "AlipayH5",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_AlipayH5),
            IsInputAmount = true,
            IsRedirectToOuterPage = true,
            ConfigSettingItemKey = 15,
        };

        /// <summary>支付寶 (固定金額)</summary>
        public static readonly ApolloServiceType AlipayPdd = new ApolloServiceType()
        {
            Value = "AlipayPdd",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_AlipayPdd),
            IsInputAmount = false,
            IsRedirectToOuterPage = true,
            ConfigSettingItemKey = 22,
        };

        /// <summary>快捷支付</summary>
        public static readonly ApolloServiceType SpeedyPay = new ApolloServiceType()
        {
            Value = "SpeedyPay",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_SpeedyPay),
            IsInputAmount = true,
            IsRedirectToOuterPage = true,
            ConfigSettingItemKey = 6,
        };

        /// <summary>銀聯支付 (掃碼)</summary>
        public static readonly ApolloServiceType UnionPayScan = new ApolloServiceType()
        {
            Value = "UnionPayScan",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_UnionPayScan),
            IsInputAmount = true,
            IsRedirectToOuterPage = true,
            ConfigSettingItemKey = 9,
        };

        /// <summary>銀聯支付 (H5)</summary>
        public static readonly ApolloServiceType UnionPayH5 = new ApolloServiceType()
        {
            Value = "UnionPayH5",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_UnionPayH5),
            IsInputAmount = true,
            IsRedirectToOuterPage = true,
            ConfigSettingItemKey = 19,
        };

        /// <summary>雲閃付</summary>
        public static readonly ApolloServiceType UnionPayFlash = new ApolloServiceType()
        {
            Value = "UnionPayFlash",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_UnionPayFlash),
            IsInputAmount = true,
            IsRedirectToOuterPage = true,
            ConfigSettingItemKey = 29,
        };

        /// <summary>USDT充值</summary>
        public static readonly ApolloServiceType Usdt = new ApolloServiceType()
        {
            Value = "Usdt",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_Usdt),
            IsInputAmount = false,
            IsRedirectToOuterPage = true,
            ConfigSettingItemKey = 30,
        };
        #endregion

        #region 提款
        /// <summary>法幣提款申請</summary>
        public static readonly ApolloServiceType AutoTransfer = new ApolloServiceType()
        {
            Value = "AutoTransfer",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_AutoTransfer),
            IsRechargeService = false,
            IsInputAmount = true,
            IsRedirectToOuterPage = false,
            IsUserBankCardRequired = true,
        };

        /// <summary>USDT提款</summary>
        public static readonly ApolloServiceType UsdtTransfer = new ApolloServiceType()
        {
            Value = "UsdtTransfer",
            ResourcePropertyName = nameof(SelectItemElement.ApolloServiceType_UsdtTransfer),
            IsRechargeService = false,
            IsInputAmount = true,
            IsRedirectToOuterPage = false,
        };
        #endregion
    }
}
