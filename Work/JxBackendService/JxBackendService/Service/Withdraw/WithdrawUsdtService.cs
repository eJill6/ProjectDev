using JxBackendService.Interface.Service;
using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Withdraw
{
    public class WithdrawUsdtService : IWithdrawReadService
    {
        public AmountLimitInfo GetServiceAmountLimit() => new AmountLimitInfo()
        {
            MinAmountLimit = 20,
            MaxAmountLimit = 99999999
        };
    }
}
