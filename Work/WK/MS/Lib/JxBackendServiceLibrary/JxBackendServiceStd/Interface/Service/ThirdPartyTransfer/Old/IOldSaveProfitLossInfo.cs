using JxBackendService.Interface.Service.User;
using JxBackendService.Model.ThirdParty.Base;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.ThirdPartyTransfer.Old
{
    public interface IOldSaveProfitLossInfo<T> : IEnvLoginUserService where T : BaseRemoteBetLog
    {
        void SaveDataToTarget(List<T> betLogs);
    }
}