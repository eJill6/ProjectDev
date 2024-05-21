using JxBackendService.Interceptors.Base;
using JxBackendService.Interface.Service.MiseLive;
using System.Collections.Generic;

namespace JxBackendService.Interceptors
{
    public class MiseLiveApiServiceInterceptor : BaseLogToDbInterceptor
    {
        private static readonly HashSet<string> s_interceptMethodFilters = new HashSet<string>()
        {
            nameof(IMiseLiveApiService.TransferIn),
            nameof(IMiseLiveApiService.TransferOut),
            nameof(IMiseLiveApiService.GetUserBalance),
        };

        protected override HashSet<string> InterceptMethodFilters => s_interceptMethodFilters;
    }
}