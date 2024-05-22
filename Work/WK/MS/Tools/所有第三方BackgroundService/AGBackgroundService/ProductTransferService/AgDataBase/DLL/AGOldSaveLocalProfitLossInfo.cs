using JxBackendService.Model.ViewModel;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using ProductTransferService.AgDataBase.DBUtility;
using ProductTransferService.AgDataBase.Model;
using System.Data;

namespace ProductTransferService.AgDataBase.DLL
{
    public class AGOldSaveLocalProfitLossInfo : OldProfitLossInfo, IAGOldSaveProfitLossInfo
    {
        public AGOldSaveLocalProfitLossInfo(EnvironmentUser envLoginUser)
        {
        }

        public void SaveDataToTarget(List<BaseAGInfoModel> betLogs)
        {
            throw new NotSupportedException();
        }

        protected override IDbConnection GetSqliteConnection() => SQLiteDBHelper.GetSQLiteConnection(AGProfitLossInfo.DbFullName);

        protected override object GetSqliteLock() => SQLiteDBHelper.WriteLockobj;
    }
}