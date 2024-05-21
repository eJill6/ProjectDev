using JxBackendService.Interceptors.Base;
using JxBackendService.Interface.Service;
using System.Collections.Generic;

namespace JxBackendService.Interceptors
{
    public class TPGameApiServiceInterceptor : BaseLogToDbInterceptor
    {
        protected override HashSet<string> InterceptMethodFilters
            => new HashSet<string>()
            {
               nameof(ITPGameApiService.CreateTransferOutInfo),
               nameof(ITPGameApiService.GetRemoteUserScore),
               nameof(ITPGameApiService.CreateTransferInInfo),
               nameof(ITPGameApiService.CreateAllAmountTransferOutInfo),
            };
    }
}