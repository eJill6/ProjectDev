using Autofac;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Merchant;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace JxBackendService.Service.ThirdPartyTransfer.Old
{
    /// <summary>
    /// 給舊版TransferService用的簡易版底層
    /// </summary>
    public abstract class OldBaseTransferScheduleService<ApiParamType> : ServiceBase
    {
        protected readonly ITPGameApiReadService _tpGameApiReadService;

        protected readonly ITPGameApiService TPGameApiService;

        protected readonly ITPGameAccountReadService TPGameAccountReadService;

        private readonly ITPGameTransferOutQueueService _tpGameTransferOutQueueService;

        private readonly IMerchantSettingService _merchantSettingService;

        private static readonly ConcurrentQueue<OldTransferParamJob<ApiParamType>> _waitTransferJobs = new ConcurrentQueue<OldTransferParamJob<ApiParamType>>();

        private static readonly ConcurrentDictionary<OldTransferParamJob<ApiParamType>, object> _processingTransferJobs
            = new ConcurrentDictionary<OldTransferParamJob<ApiParamType>, object>();

        private static readonly object locker = new object();

        protected EnvironmentUser EnvUser => new EnvironmentUser()
        {
            Application = Application,
            LoginUser = new BasicUserInfo()
        };

        protected abstract JxApplication Application { get; }

        protected abstract PlatformProduct Product { get; }

        protected abstract List<InsertTPGameProfitlossParam> GetInsertTPGameProfitlossParams();

        protected abstract bool UpdateSQLiteToSavedStatus(string keyId, SaveBetLogFlags saveBetLogFlag);

        protected abstract BaseReturnDataModel<UserScore> GetRemoteUserScore(string tpGameAccount);

        protected PlatformMerchant Merchant => SharedAppSettings.PlatformMerchant;

        protected bool IsWork { get; private set; }

        protected virtual bool IsTransferInJobEnabled => true;

        protected virtual bool IsTransferOutJobEnabled => true;

        protected virtual bool IsTransferAllOutJobEnabled => true;

        protected virtual bool IsRecheckProcessingStatusOrderJobEnabled => true;

        protected virtual bool IsSaveBetLogToPlatformJobEnabled => true;

        protected virtual int RecheckProcessingStatusOrderIntervalSeconds => 180;

        protected readonly int SaveBetLogToSQLiteJobIntervalSeconds =
            new Lazy<int>(() => ScheduleSettingUtil.GetSaveBetLogToSQLiteJobIntervalSeconds()).Value;

        protected virtual int SaveBetLogToPlatformJobIntervalSeconds => 60;

        protected abstract bool DoInitialJobOnStart();

        protected abstract ApiParamType ConvertMoneyInInfoToApiParam(TPGameMoneyInInfo tpGameMoneyInInfo, string tpGameAccount);

        protected abstract ApiParamType ConvertMoneyOutInfoToApiParam(TPGameMoneyOutInfo tpGameMoneyOutInfo, string tpGameAccount);

        protected abstract Action<object> TransferCallback { get; }

        protected abstract Action<object> CheckProcessingOrderAndSaveToRemoteDBCallback { get; }

        protected virtual void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        { }

        protected bool IsComputeAdmissionBetMoney => _merchantSettingService.IsComputeAdmissionBetMoney;

        protected OldBaseTransferScheduleService()
        {
            try
            {
                string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;

                // 加上autofac
                ContainerBuilder builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, null);
                AppendServiceToContainerBuilder(builder);
                DependencyUtil.SetContainer(builder.Build());
                TPGameApiService = DependencyUtil.ResolveJxBackendService<ITPGameApiService>(Product, Merchant, EnvUser, DbConnectionTypes.Master);
                _tpGameApiReadService = DependencyUtil.ResolveJxBackendService<ITPGameApiReadService>(Product, Merchant, EnvUser, DbConnectionTypes.Slave);
                _tpGameTransferOutQueueService = DependencyUtil.ResolveJxBackendService<ITPGameTransferOutQueueService>(EnvUser, DbConnectionTypes.Master);
                _merchantSettingService = DependencyUtil.ResolveJxBackendService<IMerchantSettingService>(Merchant, EnvUser, DbConnectionTypes.Slave);
                TPGameAccountReadService = DependencyUtil.ResolveJxBackendService<ITPGameAccountReadService>(
                    SharedAppSettings.PlatformMerchant,
                    EnvUser,
                    DbConnectionTypes.Slave);
            }
            catch (Exception ex)
            {
                //ErrorMsgUtil.ErrorHandle(ex, EnvUser);
                LogUtil.Error(ex);
                throw ex;
            }
        }

        protected override void OnStart(string[] args)
        {
            IsWork = true;

            if (!DoInitialJobOnStart())
            {
                IsWork = false;
                return;
            }

            ThreadPool.QueueUserWorkItem(new WaitCallback(DoTransferWork), string.Empty);
            //ThreadPool.QueueUserWorkItem(new WaitCallback(DoTransferWork), string.Empty);
            //ThreadPool.QueueUserWorkItem(new WaitCallback(DoTransferWork), string.Empty);

            if (IsRecheckProcessingStatusOrderJobEnabled)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(RecheckProcessingStatusOrder), string.Empty);
            }

            if (IsSaveBetLogToPlatformJobEnabled)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(SaveBetLogToPlatformJob), string.Empty);
            }

            if (IsTransferAllOutJobEnabled)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(TransferAllOutJob), string.Empty);
            }

            LogUtil.ForcedDebug("服务已啟動");
        }

        private string GetTPGameAccount(int userId)
        {
            BaseReturnDataModel<string> returnModel = TPGameAccountReadService.GetTPGameAccountByLocalAccount(userId, Product);

            if (returnModel.IsSuccess)
            {
                return returnModel.DataModel;
            }

            throw new SystemException($"GetTPGameAccount Fail, userId={userId}");
        }

        /// <summary>
        /// 重新確認處理中的訂單
        /// </summary>
        /// <param name="state"></param>
        private void RecheckProcessingStatusOrder(object state)
        {
            while (IsWork)
            {
                //改到執行前先暫停, 避免服務中斷太久時剛啟動時出現併發
                //3分鐘一次, (配合抓取資料改為3分鐘)
                Thread.Sleep(RecheckProcessingStatusOrderIntervalSeconds * 1000);

                try
                {
                    var oldTPGameOrderParams = new List<OldTPGameOrderParam<ApiParamType>>();

                    // 正在處理轉入訂單
                    oldTPGameOrderParams.AddRange(_tpGameApiReadService.GetTPGameProcessingMoneyInInfo()
                        .Select(s => new OldTPGameOrderParam<ApiParamType>()
                        {
                            ApiParam = ConvertMoneyInInfoToApiParam(s, GetTPGameAccount(s.UserID)),
                            TPGameMoneyInfo = s,
                        }));

                    // 正在處理轉出訂單
                    oldTPGameOrderParams.AddRange(_tpGameApiReadService.GetTPGameProcessingMoneyOutInfo()
                        .Select(s => new OldTPGameOrderParam<ApiParamType>()
                        {
                            ApiParam = ConvertMoneyOutInfoToApiParam(s, GetTPGameAccount(s.UserID)),
                            TPGameMoneyInfo = s,
                        }));

                    lock (locker)
                    {
                        foreach (OldTPGameOrderParam<ApiParamType> oldTPGameOrderParam in oldTPGameOrderParams)
                        {
                            AddTransferJob(TransferJobTypes.RecheckProcessingOrders, oldTPGameOrderParam);
                            //ThreadPool.QueueUserWorkItem(new WaitCallback(CheckProcessingOrderAndSaveToRemoteDBCallback), oldTPGameOrderParam);
                            //Thread.Sleep(1 * 1000);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error($"从{Product.Value}帐户檢查訂單狀態(CheckProcessingOrderAndSaveToRemoteDB)出现异常，信息：" + ex.Message + ",堆栈:" + ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// 把注單資料送回平台資料庫
        /// </summary>
        /// <param name="state"></param>
        protected virtual void SaveBetLogToPlatformJob(object state)
        {
            while (IsWork)
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
                    LogUtil.Error($"从{Product.Value}把注單資料送回平台資料庫(SaveBetLogToPlatformJob)出现异常，信息：" + ex.Message + ",堆栈:" + ex.StackTrace);
                }

                Thread.Sleep(SaveBetLogToPlatformJobIntervalSeconds * 1000);
            }
        }

        private void TransferAllOutJob(object state)
        {
            _tpGameTransferOutQueueService.StartDequeueTransferAllOutJob(Product);
        }

        private void AddTransferJob(TransferJobTypes jobType, OldTPGameOrderParam<ApiParamType> apiParam)
        {
            bool isWaitJobExist = _waitTransferJobs.Any(w => w.OldTPGameOrderParam.TPGameMoneyInfo.GetType().Name == apiParam.TPGameMoneyInfo.GetType().Name &&
            w.OldTPGameOrderParam.TPGameMoneyInfo.GetMoneyID() == apiParam.TPGameMoneyInfo.GetMoneyID());

            bool isProcessingJobExist = _processingTransferJobs
                .Any(w => w.Key.OldTPGameOrderParam.TPGameMoneyInfo.GetType().Name == apiParam.TPGameMoneyInfo.GetType().Name &&
                w.Key.OldTPGameOrderParam.TPGameMoneyInfo.GetMoneyID() == apiParam.TPGameMoneyInfo.GetMoneyID());

            if (!isProcessingJobExist && !isWaitJobExist)
            {
                _waitTransferJobs.Enqueue(new OldTransferParamJob<ApiParamType> { JobType = jobType, OldTPGameOrderParam = apiParam });
            }
        }

        private void DoTransferWork(object state)
        {
            while (IsWork)
            {
                ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
                {
                    OldTransferParamJob<ApiParamType> transferParamJob = null;

                    lock (locker)
                    {
                        if (_waitTransferJobs.Any())
                        {
                            LogUtil.ForcedDebug($"_waitTransferJobs count = {_waitTransferJobs.Count}");
                            _waitTransferJobs.TryDequeue(out transferParamJob);
                            _processingTransferJobs.TryAdd(transferParamJob, null);
                        }
                    }

                    if (transferParamJob == null)
                    {
                        Task.Delay(1000).Wait();

                        return;
                    }

                    var stopwatch = new Stopwatch();

                    try
                    {
                        stopwatch.Start();

                        switch (transferParamJob.JobType)
                        {
                            case TransferJobTypes.RecheckProcessingOrders:
                                CheckProcessingOrderAndSaveToRemoteDBCallback(transferParamJob.OldTPGameOrderParam);
                                break;
                        }
                    }
                    finally
                    {
                        _processingTransferJobs.TryRemove(transferParamJob, out object value);
                        stopwatch.Stop();

                        string info = new
                        {
                            JobType = transferParamJob.JobType.ToString(),
                            MoneyID = transferParamJob.OldTPGameOrderParam.TPGameMoneyInfo.GetMoneyID(),
                            stopwatch.ElapsedMilliseconds
                        }.ToJsonString();

                        LogUtil.ForcedDebug($"TransferJob完成, {info}");
                        Thread.Sleep(1000);
                    }
                });
            }
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

                BaseReturnDataModel<string> result = TPGameAccountReadService.GetTPGameAccountByLocalAccount(userId, Product);

                if (!result.IsSuccess)
                {
                    LogUtil.Error("GetTPGameAccountByLocalAccount Error:" + result.Message + "; UserID:" + userId.ToString());

                    continue;
                }

                string tpGameAccount = result.DataModel;
                BaseReturnDataModel<UserScore> baseReturnDataModel = GetRemoteUserScore(tpGameAccount);

                if (baseReturnDataModel.IsSuccess)
                {
                    userScoreMap.Add(userId, baseReturnDataModel.DataModel);
                }

                Thread.Sleep(500);
            }

            return userScoreMap;
        }

        protected override void OnStop()
        {
            IsWork = false;

            try
            {
                //TelegramService.ExecuteRightNowAlarmMessage("服务已停止");
                LogUtil.ForcedDebug("服务已停止");
                DoJobOnStop();

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                LogUtil.Error("服务中止失败" + ex.ToString());
            }
            finally
            {
                base.OnStop();
            }
        }

        protected virtual void DoJobOnStop()
        { }
    }
}