using JxBackendService.Interceptors.Base;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Model.Enums;
using System.Collections.Generic;

namespace JxBackendService.Interceptors
{
    public class TPGameTransferOutServiceInterceptor : BaseLogToDbInterceptor
    {
        protected override HashSet<string> InterceptMethodFilters
            => new HashSet<string>()
            {
                nameof(ITPGameTransferOutService.AllAmountBySingleProduct),
                nameof(ITPGameTransferOutService.ProcessTransferAllOutQueue),
            };

        protected override object[] ConvertArguments(object[] arguments)
        {
            for (int i = 0; i < arguments.Length; i++)
            {
                if (arguments[i] is PlatformProduct)
                {
                    arguments[i] = (arguments[i] as PlatformProduct).Value;
                    break;
                }
            }

            return arguments;
        }
    }
}