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
    }

    public class RegularExpressionType : BaseValueModel<RegularExpressionEnumTypes, RegularExpressionType>
    {
        private Type ErrorMsgResourceType { get; set; }

        private string ErrorMsgResourcePropertyName { get; set; }

        public string Pattern { get; private set; }

        public string ErrorMsg => GetNameByResourceInfo(ErrorMsgResourceType, ErrorMsgResourcePropertyName);

        private RegularExpressionType() { }

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
            Pattern = @"^1(3[0-9]|5[0-9]|7[6-8]|8[0-9])[0-9]{8}$"
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
            Pattern = @"^(?=.*\d)(?=.*[a-zA-Z]).{6,50}$"
        };

        /// <summary> 資金密碼 </summary>
        public static readonly RegularExpressionType MoneyPassword = new RegularExpressionType()
        {
            Value = RegularExpressionEnumTypes.MoneyPassword,
            ErrorMsgResourceType = typeof(MessageElement),
            ErrorMsgResourcePropertyName = nameof(MessageElement.MoneyPasswordFormatFail),
            Pattern = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*[^\x21-\x7e]).{10,16}$"
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
