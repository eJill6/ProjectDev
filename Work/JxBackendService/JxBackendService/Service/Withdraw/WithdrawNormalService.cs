using JxBackendService.Interface.Service;
using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Withdraw
{
    public class WithdrawNormalService : IWithdrawReadService
    {
        /// <summary>
        /// 一般提現目前取上下限額還沒有實作
        /// </summary>
        /// <returns></returns>
        public AmountLimitInfo GetServiceAmountLimit()
        {
            throw new NotImplementedException();
        }
    }
}
