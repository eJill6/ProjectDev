using JxBackendService.Service.Withdraw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class WithdrawTypes : BaseStringValueModel<WithdrawTypes>
    {
        private WithdrawTypes() { }

        public Type WithdrawServiceType { get; private set; }

        public static readonly WithdrawTypes Normal = new WithdrawTypes()
        {
            Value = "Normal",
            WithdrawServiceType = typeof(WithdrawNormalService)
        };

        public static readonly WithdrawTypes Usdt = new WithdrawTypes()
        {
            Value = "Usdt",
            WithdrawServiceType = typeof(WithdrawUsdtService)
        };
    }
}
