using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums
{
    public class AccountType : BaseStringValueModel<AccountType>
    {
        private AccountType()
        {
            ResourceType = typeof(SelectItemElement);
        }

        /// <summary>秘色用户ID</summary>
        public static readonly AccountType MSUserAccountID = new AccountType()
        {
            Value = "MSUserAccountID",
            ResourcePropertyName = nameof(SelectItemElement.AccountType_MSUserAccountID)
        };

        /// <summary>第三方用户ID</summary>
        public static readonly AccountType TPGameAccountID = new AccountType()
        {
            Value = "TPGameAccountID",
            ResourcePropertyName = nameof(SelectItemElement.AccountType_TPGameAccountID)
        };
    };
}