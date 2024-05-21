using JxBackendService.Interceptors.Base;
using JxBackendService.Interface.Service;
using System.Collections.Generic;

namespace JxBackendService.Interceptors
{
    public class TPGameRemoteApiServiceInterceptor : BaseLogToDbInterceptor
    {
        private static readonly HashSet<string> s_interceptMethodFilters = new HashSet<string>()
        {            
            nameof(ITPGameRemoteApiService.GetRemoteTransferApiResult),
            nameof(ITPGameRemoteApiService.GetRemoteUserScoreApiResult),
        };

        protected override HashSet<string> InterceptMethodFilters => s_interceptMethodFilters;            
    }
}