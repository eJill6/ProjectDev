using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums.ResetPassword;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class PasswordType : BaseIntValueModel<PasswordType>
    {
        public RegularExpressionType ValidRegularExpressionType { get; private set; }

        private string ShortNameResourcePropertyName { get; set; }

        private string RecommendPropertyName { get; set; }

        public string ShortName => GetNameByResourceInfo(ResourceType, ShortNameResourcePropertyName);

        public string RecommandName => GetNameByResourceInfo(ResourceType, RecommendPropertyName);

        public string AppInputTypeValue { get; private set; }

        public List<BaseResetPasswordType> ResetPasswordTypes { get; private set; }

        private string OperationContentPropertyName { get; set; }

        public string OperationContentName => GetNameByResourceInfo(typeof(OperationLogContentElement), OperationContentPropertyName);

        public bool IsOnlyUpdateByLoginUser { get; private set; }

        public WebActionType ModifyPasswordActionType { get; private set; }

        public AuditTypeValue AuditTypeValue { get; private set; }

        /// <summary>
        /// 用於後台重置密碼時對應的設定檔ConfigGroup: MoneyBankVerify_PasswordReset 的 ConfigSettings.ItemKey
        /// </summary>
        public int ResetPasswordCheckBankCardConfigSettingKey { get; private set; } 

        private PasswordType() { }

        /// <summary>
        /// 登入密碼
        /// </summary>        
        public static readonly PasswordType Login = new PasswordType()
        {
            Value = 0,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.PasswordType_Login),
            ShortNameResourcePropertyName = nameof(SelectItemElement.PasswordTypeShortName_Login),
            RecommendPropertyName = nameof(SelectItemElement.PasswordTypeRecommend_Login),
            ValidRegularExpressionType = RegularExpressionType.LoginPassword,
            AppInputTypeValue = "password",
            ResetPasswordTypes = ResetLoginPasswordType.ResetPasswordTypes,
            OperationContentPropertyName = nameof(OperationLogContentElement.ForgetPassword),
            ModifyPasswordActionType = WebActionType.ModifyLoginPassword,
            AuditTypeValue = AuditTypeValue.LoginPassword,
            ResetPasswordCheckBankCardConfigSettingKey = (int)BacksideResetPasswordSettingTypes.LoginPassword
        };

        /// <summary>
        /// 資金密碼
        /// </summary>
        public static readonly PasswordType Money = new PasswordType()
        {
            Value = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.PasswordType_Money),
            ShortNameResourcePropertyName = nameof(SelectItemElement.PasswordTypeShortName_Money),
            RecommendPropertyName = nameof(SelectItemElement.PasswordTypeRecommend_Money),
            ValidRegularExpressionType = RegularExpressionType.MoneyPassword,
            AppInputTypeValue = "moneypwd",
            ResetPasswordTypes = ResetMoneyPasswordType.ResetPasswordTypes,
            OperationContentPropertyName = nameof(OperationLogContentElement.ForgetMoneyPassword),
            IsOnlyUpdateByLoginUser = true,
            ModifyPasswordActionType = WebActionType.ModifyMoneyPassword,
            AuditTypeValue = AuditTypeValue.MoneyPassword,
            ResetPasswordCheckBankCardConfigSettingKey = (int)BacksideResetPasswordSettingTypes.MoneyPassword
        };

        /// <summary>
        /// 融舊
        /// 用App傳入的Type換回共用的Value
        /// </summary>
        /// <param name="appInputTypeValue"></param>
        /// <returns></returns>
        public static PasswordType GetSingleByAppInputTypeValue(string appInputTypeValue)
        {
            return PasswordType.GetAll().SingleOrDefault(pt => pt.AppInputTypeValue == appInputTypeValue);
        }
    }
}
