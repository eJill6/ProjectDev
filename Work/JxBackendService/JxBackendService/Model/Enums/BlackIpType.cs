using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class BlackIpType : BaseIntValueModel<BlackIpType>
    {
        private BlackIpType() { }

        public static readonly BlackIpType Login = new BlackIpType()
        {
            Value = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BlackIpType_Login)
        };

        public static readonly BlackIpType CardToCard = new BlackIpType()
        {
            Value = 2,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BlackIpType_DepositByCardToCard)
        };

        public static readonly BlackIpType DepositByThirdParty = new BlackIpType()
        {
            Value = 3,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BlackIpType_DepositByThirdParty)
        };

        public static readonly BlackIpType DepositByTransferToCard = new BlackIpType()
        {
            Value = 4,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BlackIpType_DepositByTransferToCard)
        };
    }
}
