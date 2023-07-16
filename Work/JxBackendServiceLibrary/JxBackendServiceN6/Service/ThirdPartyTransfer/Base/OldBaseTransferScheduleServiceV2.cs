﻿using Autofac;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.GlobalSystem;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Base;
using System.Threading;

namespace JxBackendServiceN6.Service.ThirdPartyTransfer.Old
{
    /// <summary>
    /// 給舊版TransferService用的簡易版底層v2, 把轉帳功能的呼叫改用TPGameApiService, 不再使用 Action callback舊版程式碼
    /// </summary>
    public abstract class OldBaseTransferScheduleServiceV2 : BaseTransferScheduleService<BaseRemoteBetLog> //拉取注單還是依賴原本舊程式碼,先傳入底層model讓編譯通過
    {
        protected readonly ITPGameAccountReadService _tpGameAccountReadService;

        protected abstract List<InsertTPGameProfitlossParam> GetInsertTPGameProfitlossParams();

        protected abstract bool UpdateSQLiteToSavedStatus(string keyId, SaveBetLogFlags saveBetLogFlag);

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

        protected override void SaveBetLogToSQLiteJob(CancellationToken cancellationToken)
        {
            DoJobWithCancellationToken(
               cancellationToken,
               jobIntervalSeconds: SaveBetLogToSQLiteJobIntervalSeconds,
               doJob: () =>
               {
                   DoSaveBetLogToSQLiteJob();

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

        protected abstract void DoSaveBetLogToSQLiteJob();

        /// <summary>
        /// 把注單資料送回平台資料庫
        /// </summary>
        /// <param name="state"></param>
        protected override void SaveBetLogToPlatformJob(CancellationToken cancellationToken)
        {
            DoJobWithCancellationToken(
              cancellationToken,
              jobIntervalSeconds: SaveBetLogToPlatformJobIntervalSeconds,
              doJob: () =>
              {
                  try
                  {
                      List<InsertTPGameProfitlossParam> tpGameProfitlosses = GetInsertTPGameProfitlossParams();

                      if (tpGameProfitlosses.Any())
                      {
                          List<int> validUserIds = tpGameProfitlosses.Where(w => !w.IsIgnore).Select(s => s.UserID).ToList();
                          Dictionary<int, UserScore> userScoreMap = GetUserScoreMap(validUserIds);

                          TPGameApiService.SaveProfitlossToPlatform(new SaveProfitlossToPlatformParam()
                          {
                              TPGameProfitlosses = tpGameProfitlosses,
                              UpdateSQLiteToSavedStatus = (keyId, saveBetLogFlag) => UpdateSQLiteToSavedStatus(keyId, saveBetLogFlag),
                              UserScoreMap = userScoreMap
                          });
                      }
                  }
                  catch (Exception ex)
                  {
                      LogUtilService.Error($"从{Product.Value}把注單資料送回平台資料庫(SaveBetLogToPlatformJob)出现异常，信息：" + ex.Message + ",堆栈:" + ex.StackTrace);
                  }

                  return true;
              });
        }

        protected Dictionary<int, UserScore> GetUserScoreMap(List<int> userIds)
        {
            var userScoreMap = new Dictionary<int, UserScore>();

            foreach (int userId in userIds)
            {
                if (userScoreMap.ContainsKey(userId))
                {
                    continue;
                }

                BaseReturnDataModel<UserScore> baseReturnDataModel = TPGameApiReadService
                    .GetRemoteUserScore(new InvocationUserParam() { UserID = userId }, isRetry: false);

                if (baseReturnDataModel.IsSuccess)
                {
                    userScoreMap.Add(userId, baseReturnDataModel.DataModel);
                }

                Thread.Sleep(500);
            }

            return userScoreMap;
        }

        protected override void DoDeleteExpiredProfitLoss()
        {
            var oldProfitLossInfo = DependencyUtil.ResolveService<IOldProfitLossInfo>();
            oldProfitLossInfo.ClearExpiredData();
        }

        #region no use overwrite methods

        protected override BaseReturnDataModel<List<BaseRemoteBetLog>> ConvertToBetLogs(string apiResult)
        {
            throw new System.NotImplementedException();
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(List<BaseRemoteBetLog> betLogs)
        {
            throw new System.NotImplementedException();
        }

        protected override LocalizationParam GetCustomizeMemo(BaseRemoteBetLog betLog)
        {
            throw new System.NotImplementedException();
        }

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse dataModel)
        {
            throw new System.NotImplementedException();
        }

        #endregion no use overwrite methods
    }
}