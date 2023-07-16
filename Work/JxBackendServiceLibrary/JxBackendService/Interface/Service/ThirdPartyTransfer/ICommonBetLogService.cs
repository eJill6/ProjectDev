using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.ThirdParty;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.ThirdPartyTransfer
{
    public interface ICommonBetLogService
    {
        void SaveBetLogToPlatform(PlatformProduct product, List<InsertTPGameProfitlossParam> insertTPGameProfitlossParams);
    }
}
