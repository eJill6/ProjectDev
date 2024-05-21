using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Base;

namespace JxBackendServiceN6.Service.ThirdPartyTransfer.Old
{
    /// <summary>
    /// 給舊版TransferService用的簡易版底層v2, 把轉帳功能的呼叫改用TPGameApiService, 不再使用 Action callback舊版程式碼
    /// </summary>
    public abstract class OldBaseTransferScheduleServiceV2 : BaseTransferScheduleService<BaseRemoteBetLog>
    {
        private readonly Lazy<ITPGameAccountReadService> _tpGameAccountReadService;

        protected ITPGameAccountReadService TPGameAccountReadService => _tpGameAccountReadService.Value;

        protected OldBaseTransferScheduleServiceV2()
        {
            _tpGameAccountReadService = DependencyUtil.ResolveJxBackendService<ITPGameAccountReadService>(
                SharedAppSettings.PlatformMerchant,
                EnvUser,
                DbConnectionTypes.Slave);
        }

        public override bool InitAppSettings(CancellationToken cancellationToken) => DoInitialJobOnStart(cancellationToken);

        protected abstract bool DoInitialJobOnStart(CancellationToken cancellationToken);

        protected override void InitSqlLite() => DoInitSqlLite();

        protected abstract void DoInitSqlLite();

        protected abstract void DoSaveRemoteBetLogToPlatformJob();

        protected override void SaveRemoteBetLogToPlatformJob(CancellationToken cancellationToken)
        {
            DoJobWithCancellationToken(
               cancellationToken,
               jobIntervalSeconds: SaveRemoteBetLogToPlatformJobIntervalSeconds,
               doJob: () =>
               {
                   DoSaveRemoteBetLogToPlatformJob();

                   try
                   {
                       return !RemoteFileSetting.HasNewRemoteFile; //有檔案的話要繼續下一筆,所以return false;(不等待)
                   }
                   finally
                   {
                       RemoteFileSetting.HasNewRemoteFile = false;
                   }
               });
        }

        protected override void DoDeleteExpiredProfitLoss()
        {
            var transferSqlLiteBackupRepository = DependencyUtil.ResolveService<ITransferSqlLiteBackupRepository>().Value;
            transferSqlLiteBackupRepository.DeleteExpiredDbFile();
        }

        #region no use overwrite methods

        protected override BaseReturnDataModel<List<BaseRemoteBetLog>> ConvertToBetLogs(string apiResult)
        {
            throw new NotSupportedException();
        }

        protected override LocalizationParam GetCustomizeMemo(BaseRemoteBetLog betLog)
        {
            throw new NotSupportedException();
        }

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            throw new NotSupportedException();
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(Dictionary<string, int> userMap, List<BaseRemoteBetLog> betLogs)
        {
            throw new NotSupportedException();
        }

        #endregion no use overwrite methods
    }
}