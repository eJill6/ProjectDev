using JxBackendService.Model.ViewModel.ThirdParty;
using System.Collections.Generic;
using System.Data;

namespace JxBackendService.Interface.Service.ThirdPartyTransfer.Old
{
    public interface IOldProfitLossInfo
    {
        DataTable GetBatchDataFromLocalDB(string tableName);

        void BackupBetLogs(string tableName, List<string> successKeyIds);
    }
}