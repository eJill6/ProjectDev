using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums
{
    public class WebActionType : BaseIntValueModel<WebActionType>
    {
        public int CacheSeconds { get; private set; }

        public int LimitCount { get; private set; }

        public bool IsLogined { get; private set; }

        /// <summary>是否判斷身分令牌(用於未登入狀態下的同一個client判斷)</summary>
        public bool IsParseIdentityToken { get; private set; }

        private WebActionType() { }

        /// <summary>登入失敗</summary>
        public static readonly WebActionType LoginFail = new WebActionType()
        {
            Value = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.WebActionType_LoginFail),
            CacheSeconds = 600,
            LimitCount = 10,
            IsLogined = false,
        };

        /// <summary>修改登录密码</summary>
        public static readonly WebActionType ModifyLoginPassword = new WebActionType()
        {
            Value = 2,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.WebActionType_ModifyLoginPassword),
            CacheSeconds = 60,
            LimitCount = 10,
            IsLogined = true,
        };

        /// <summary>修改資金密碼</summary>
        public static readonly WebActionType ModifyMoneyPassword = new WebActionType()
        {
            Value = 3,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.WebActionType_ModifyMoneyPassword),
            CacheSeconds = 60,
            LimitCount = 10,
            IsLogined = true,
        };

        /// <summary>谷歌身份绑定</summary>
        public static readonly WebActionType BindAuthenticator = new WebActionType()
        {
            Value = 4,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.WebActionType_BindAuthenticator),
        };

        /// <summary>提現</summary>
        public static readonly WebActionType Withdraw = new WebActionType()
        {
            Value = 5,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.WebActionType_Withdraw),
        };

        /// <summary>下線轉帳</summary>
        public static readonly WebActionType TransferToChild = new WebActionType()
        {
            Value = 6,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.WebActionType_TransferToChild),
        };

        /// <summary>修改銀行卡</summary>
        public static readonly WebActionType ModifyBankCard = new WebActionType()
        {
            Value = 7,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.WebActionType_ModifyBankCard),
        };

        /// <summary>綁定USDT帳戶</summary>
        public static readonly WebActionType BindUSDTAccount = new WebActionType()
        {
            Value = 8,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.WebActionType_BindUSDTAccount),
        };

        /// <summary>註冊新帳號</summary>
        public static readonly WebActionType RegisterNewAccount = new WebActionType()
        {
            Value = 9,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.WebActionType_RegisterNewAccount),
            CacheSeconds = 60,
            LimitCount = 10,
            IsLogined = false,
            IsParseIdentityToken = true,
        };
    }

    public class SubActionType : BaseStringValueModel<SubActionType>
    {
        public HandleTypes HandleType { get; private set; }

        public int FailCountLimit { get; private set; }

        public string DefaultErrorMessage => GetNameByResourceInfo(typeof(ReturnCodeElement), DefaultErrorMessageResourcePropertyName);

        public string HandleOperationLogContent => GetNameByResourceInfo(typeof(OperationLogContentElement), HandleOperationLogContentPropertyName);

        /// <summary>預設顯示的錯誤訊息Resource Property Name</summary>
        private string DefaultErrorMessageResourcePropertyName { get; set; }

        /// <summary>達到上限後的錯誤處理操作紀錄Resource Property Name</summary>
        private string HandleOperationLogContentPropertyName { get; set; }
        
        
        private SubActionType() { }

        public static SubActionType MoneyPasswordWrong = new SubActionType()
        {
            Value = "MoneyPasswordWrong",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.SubActionType_MoneyPasswordWrong),
            DefaultErrorMessageResourcePropertyName = nameof(ReturnCodeElement.YourMoneyPasswordIsWrong),
            HandleOperationLogContentPropertyName = nameof(OperationLogContentElement.DisableAccountByMoneyPasswordWrong),
            HandleType = HandleTypes.DisableAccount,
            FailCountLimit = 5,
        };
    }

    public enum HandleTypes
    {
        /// <summary>凍結帳戶</summary>
        DisableAccount
    }
}
