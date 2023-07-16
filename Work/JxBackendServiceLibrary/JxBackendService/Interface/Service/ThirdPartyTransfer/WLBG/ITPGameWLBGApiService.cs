using System.Collections.Generic;

namespace JxBackendService.Interface.Service.ThirdPartyTransfer.WLBG
{
    public interface ITPGameWLBGApiService
    {
        Dictionary<string, string> GetApiGameListResult();
    }
}