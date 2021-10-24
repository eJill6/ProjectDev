using JxBackendService.Model.ThirdParty.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository
{
    public interface ITransferSqlLiteRepository
    {
        void TryCreateDataBase();

        void TryCreateTableProfitLoss();

        void TryCreateTableLastSearchToken();

        void SaveProfitloss<BetLogType>(List<BetLogType> betLogs) where BetLogType : BaseRemoteBetLog;

        string GetLastSearchToken();

        List<BetLogType> GetBatchProfitlossNotSavedToRemote<BetLogType>() where BetLogType : BaseRemoteBetLog;

        int SaveProfitlossToPlatformSuccess(string keyId);

        int SaveProfitlossToPlatformFail(string keyId);
        
        int SaveProfitlossToPlatformIgnore(string keyId);
               
        int UpdateNextSearchToken(string newToken);
        
        void DeleteExpiredProfitLoss();
    }
}
