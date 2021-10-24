using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums
{
    public class SMSActionType : BaseIntValueModel<SMSActionType>
    {

        private SMSActionType() { }

        /// <summary>找回密码</summary>
        public static readonly SMSActionType FindLoginPassword = new SMSActionType()
        {
            Value = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.SMSActionType_FindLoginPassword),
        };

        /// <summary>新增银行卡</summary>
        public static readonly SMSActionType AddBankCard = new SMSActionType()
        {
            Value = 2,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.SMSActionType_AddBankCard),
        };

        /// <summary>手机绑定</summary>
        public static readonly SMSActionType BindPhone = new SMSActionType()
        {
            Value = 3,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.SMSActionType_BindPhone),
        };

        /// <summary>手机解绑</summary>
        public static readonly SMSActionType UnBindPhone = new SMSActionType()
        {
            Value = 4,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.SMSActionType_UnBindPhone),
        };
    }
}
