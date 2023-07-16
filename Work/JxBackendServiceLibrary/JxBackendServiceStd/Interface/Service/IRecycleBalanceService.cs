using JxBackendService.Interface.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.RecycleBalance;
using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface IRecycleBalanceService
    {
        BaseReturnModel RecycleByAllProducts(IInvocationUserParam invocationUserParam, RoutingSetting routingSetting);

        BaseReturnModel RecycleBySingleProduct(IInvocationUserParam invocationUserParam, PlatformProduct product, RoutingSetting routingSetting);
    }
}