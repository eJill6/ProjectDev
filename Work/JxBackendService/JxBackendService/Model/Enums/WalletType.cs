using JxBackendService.Resource.Element;
using JxBackendService.Service.Withdraw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class WalletType : BaseIntValueModel<WalletType>
    {
        private WalletType()
        {
            ResourceType = typeof(SelectItemElement);
        }

        public static readonly WalletType Center = new WalletType()
        {
            Value = 0,
            ResourcePropertyName = nameof(SelectItemElement.WalletType_CenterWallet),
        };

        public static readonly WalletType Agent = new WalletType()
        {
            Value = 1,
            ResourcePropertyName = nameof(SelectItemElement.WalletType_AgentWallet),
        };
    }
}
