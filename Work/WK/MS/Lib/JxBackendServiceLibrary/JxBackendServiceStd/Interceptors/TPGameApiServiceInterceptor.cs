using JxBackendService.Interceptors.Base;
using JxBackendService.Interface.Service;
using System.Collections.Generic;

namespace JxBackendService.Interceptors
{
    public class TPGameApiServiceInterceptor : BaseLogToDbInterceptor
    {
        private static readonly HashSet<string> s_interceptMethodFilters = new HashSet<string>()
        {
            nameof(ITPGameApiService.CreateTransferOutInfo),
            nameof(ITPGameApiService.CreateTransferInInfo),
            nameof(ITPGameApiService.CreateAllAmountTransferOutInfo),
            nameof(ITPGameApiService.GetForwardGameUrl),
        };

        protected override HashSet<string> InterceptMethodFilters => s_interceptMethodFilters;
    }
}