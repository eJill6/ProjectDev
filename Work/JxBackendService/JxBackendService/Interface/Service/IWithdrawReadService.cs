using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface IWithdrawReadService
    {
        AmountLimitInfo GetServiceAmountLimit();
    }
}
