using System;
using System.ComponentModel.DataAnnotations;
using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Attributes
{
    /// <summary>
    /// 用來串Attribute參數用
    /// </summary>
    public enum RegularExpressionEnumTypes
    {
        Email = 0,

        ChinaMobileNo = 1,

        LoginPassword = 2,

        MoneyPassword = 3,

        PositiveInteger = 4,

        DynamicPassword = 5,

        UsdtNumber = 6,

        LoginUserName = 7,

        VIPLoginUserName = 8,

        ChineseRealName = 9,

        CantEnterSpacesBeforeAndAfter = 10,

        TOPayNumber = 11,

        FloatLimitFourDigits = 12,

        VIPAgentChannelName = 13,

        FlowFloatLimitTwoDigits = 14,

        LetterAndNumber = 15,

        BankCardUserName = 16,
    }

    public class RegularExpressionType : BaseValueModel<RegularExpressionEnumTypes, RegularExpressionType>
    {
        private Type ErrorMsgResourceType { get; set; }

        private string ErrorMsgResourcePropertyName { get; set; }

        public string Pattern { get; private set; }

        public string ErrorMsg => GetNameByResourceInfo(ErrorMsgResourceType, ErrorMsgResourcePropertyName);

        private RegularExpressionType()
        { }

        /// <summary> 電子郵件 </summary>
        public static readonly RegularExpressionType Email = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.Email,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.EmailRuleFail),
            Pattern = @"^\w+((\w+)|(\.\w+)|(\+\w+)|(\-\w+))[^_|.|-]@[A-Za-z0-9]+((\.|-|_)[A-Za-z0-9]+)*\.[A-Za-z]+$"
        };

        /// <summary> 電話號碼 </summary>
        public static readonly RegularExpressionType ChinaMobileNo = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.ChinaMobileNo,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.MobileFormatFail),
            Pattern = @"^1([3-9]{1})([0-9]{9})$"
        };

        /// <summary> 正整數 </summary>
        public static readonly RegularExpressionType PositiveInteger = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.PositiveInteger,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.FormatFail),
            Pattern = @"^\+?[0-9]*$"
        };

        /// <summary> 登入密碼 </summary>
        public static readonly RegularExpressionType LoginPassword = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.LoginPassword,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.LoginPasswordFormatFail),
            Pattern = @"^(?=.*\d)(?=.*[a-zA-Z]).{6,16}$"
        };

        /// <summary> 資金密碼(6-16位字符，英数与特殊符号) </summary>
        public static readonly RegularExpressionType MoneyPassword = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.MoneyPassword,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.MoneyPasswordFormatFail),
            Pattern = @"^(?=.*\d)(?=.*[a-zA-Z])(?!.*[^\x21-\x7e]).{6,16}$"
        };

        /// <summary> 動態密碼 </summary>
        public static readonly RegularExpressionType DynamicPassword = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.DynamicPassword,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.dynamicPasswordFormatFail),
            Pattern = @"^(?=.*[a-zA-Z0-9]).{6,16}$"
        };

        /// <summary> USDT號碼 </summary>
        public static readonly RegularExpressionType UsdtNumber = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.UsdtNumber,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.UsdtAddressWrong),
            Pattern = @"^T[0-9a-zA-Z]*$"
        };

        /// <summary> 登入帳號 </summary>
        public static readonly RegularExpressionType LoginUserName = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.LoginUserName,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.UserNameFormatIsNotValid),
            Pattern = @"^[a-zA-Z0-9_\u4e00-\u9fa5]+$" //汉字，英数，以及下划线
        };

        /// <summary> 真实姓名(中文) </summary>
        public static readonly RegularExpressionType ChineseRealName = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.ChineseRealName,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.RealNameFormatIncorrect),
            Pattern = @"^[\u4e00-\u9fa5]{2,20}$" //汉字
        };

        /// <summary> 前後不能有空白 </summary>
        public static readonly RegularExpressionType CantEnterSpacesBeforeAndAfter = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.CantEnterSpacesBeforeAndAfter,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.CantEnterSpacesBeforeAndAfter),
            Pattern = @"^\S.*\S$|(^\S{0,1}\S$)"
        };

        /// <summary> TOPay號碼 </summary>
        public static readonly RegularExpressionType TOPayNumber = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.TOPayNumber,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.TOPayAddressWrong),
            Pattern = @"^tp[0-9a-zA-Z]*$"
        };

        /// <summary> 浮點數且限制小數為四位以內 </summary>
        public static readonly RegularExpressionType FloatLimitFourDigits = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.FloatLimitFourDigits,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.AmountFormatIncorrect),
            Pattern = @"^[0-9]+(.[0-9]{0,4})?$"
        };

        /// <summary> 推广渠道 </summary>
        public static readonly RegularExpressionType VIPAgentChannelName = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.VIPAgentChannelName,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.VIPAgentChannelNameTooLong),
            Pattern = @"^.{1,16}$"
        };

        /// <summary> 浮點數且限制小數為兩位以內 </summary>
        public static readonly RegularExpressionType FlowFloatLimitTwoDigits = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.FlowFloatLimitTwoDigits,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.FlowFormatIncorrect),
            Pattern = @"^[0-9]+(.[0-9]{0,2})?$"
        };

        /// <summary> 至少一個英數字，只允許輸入英數字 </summary>
        public static readonly RegularExpressionType LetterAndNumber = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.LetterAndNumber,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.PlzLetterAndNumber),
            Pattern = @"^([a-zA-Z]+[0-9]+|[0-9]+[a-zA-Z]+)+[0-9a-zA-Z]*$"
        };

        /// <summary> 銀行卡用戶姓名 (只允許兩個以上中文字)</summary>
        public static readonly RegularExpressionType BankCardUserName = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.BankCardUserName,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.PleaseInputChinese),
            Pattern = @"^[\u4e00-\u9fa5|*][\u4e00-\u9fa5]+$"
        };
    }

    public class CustomizedRegularExpressionAttribute : RegularExpressionAttribute
    {
        private readonly RegularExpressionEnumTypes _regularExpressionType;

        public CustomizedRegularExpressionAttribute(RegularExpressionEnumTypes regularExpressionType)
            : base(RegularExpressionType.GetSingle(regularExpressionType).Pattern)
        {
            _regularExpressionType = regularExpressionType;
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = RegularExpressionType.GetSingle(_regularExpressionType).ErrorMsg;
            return base.FormatErrorMessage(name);
        }
    }
}