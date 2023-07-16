using Autofac;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service.Merchant;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Model.ViewModel;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using JxBackendService.Interface.Service.Config;
using JxBackendService.DependencyInjection;
using JxBackendService.Common.Extensions;
using System.Diagnostics;
using JxBackendService.Model;
using JxBackendService.Service.ThirdPartyTransfer;
using JxBackendServiceN6.DependencyInjection;

namespace JxBackendServiceN6.Service.ThirdPartyTransfer.Base
{
    public abstract class BaseTransferScheduleService<BetLogType> : BackgroundService where BetLogType : BaseRemoteBetLog
    {
        private static readonly int s_getBetLogFailMaxCount = 10;

        protected int MaxDetailMemoContentCount => 8;

        protected ITPGameApiService TPGameApiService { get; private set; }

        protected ITPGameApiReadService TPGameApiReadService { get; private set; }

        private readonly IMerchantSettingService _merchantSettingService;

        private ITransferSqlLiteRepository _transferSqlLiteRepository;

        private readonly ITPGameTransferOutService _tpGameTransferOutService;

        private readonly ITPGameTransferOutQueueService _tpGameTransferOutQueueService;

        private readonly IMessageQueueService _messageQueueService;

        protected Dictionary<string, Task> JobNames { get; private set; } = new Dictionary<string, Task>();

        protected ILogUtilService LogUtilService { get; private set; }

        private static readonly ConcurrentQueue<TransferParamJob> _waitTransferJobs = new ConcurrentQueue<TransferParamJob>();

        private static readonly ConcurrentDictionary<TransferParamJob, object> _processingTransferJobs = new ConcurrentDictionary<TransferParamJob, object>();

        private static readonly object locker = new object();

        protected EnvironmentUser EnvUser => new EnvironmentUser()
        {
            Application = Application,
            LoginUser = new BasicUserInfo()
        };

        protected PlatformMerchant Merchant => SharedAppSettings.PlatformMerchant;

        /// <summary>
        /// OnStart時觸發，初始化設定檔參數
        /// </summary>
        /// <returns></returns>
        public virtual bool InitAppSettings(CancellationToken cancellationToken) => true;

        protected abstract PlatformProduct Product { get; }

        protected abstract JxApplication Application { get; }

        /// <summary>轉換為已結算的投注資料</summary>
        protected abstract BaseReturnDataModel<List<BetLogType>> ConvertToBetLogs(string apiResult);

        protected abstract string GetNextSearchToken(string lastSearchToken, RequestAndResponse dataModel);

        /// <summary>把第三方投注紀錄模型轉換為平台的盈虧模型</summary>
        protected abstract List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(List<BetLogType> betLogs);

        protected abstract LocalizationParam GetCustomizeMemo(BetLogType betLog);

        protected virtual void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        { }

        /// <summary>
        /// 取得平台紀錄的第三方帳號名稱是否轉小寫
        /// </summary>
        protected virtual bool IsToLowerTPGameAccount => false;

        protected bool IsComputeAdmissionBetMoney => _merchantSettingService.IsComputeAdmissionBetMoney;

        #region Job開關

        protected virtual bool IsRecheckProcessingStatusOrderJobEnabled => true;

        protected virtual bool IsSaveBetLogToSQLiteJobEnabled => true;

        protected virtual bool IsSaveBetLogToPlatformJobEnabled => true;

        protected virtual bool IsTransferAllOutJobEnabled => true;

        protected virtual bool IsClearExpiredProfitLossJobEnabled => true;

        protected virtual bool IsDoTransferCompensationJobEnabled => false;

        #endregion Job開關

        #region Job頻率設定

        protected virtual int TransferInJobIntervalSeconds => 10;

        protected virtual int TransferOutJobIntervalSeconds => 10;

        protected virtual int RecheckProcessingStatusOrderJobIntervalSeconds => 180;

        private static readonly int s_defaultSaveBetLogToSQLiteJobIntervalSeconds = 60;

        private int? _saveBetLogToSQLiteJobIntervalSeconds;

        protected int SaveBetLogToSQLiteJobIntervalSeconds
        {
            get
            {
                if (!_saveBetLogToSQLiteJobIntervalSeconds.HasValue)
                {
                    _saveBetLogToSQLiteJobIntervalSeconds = s_defaultSaveBetLogToSQLiteJobIntervalSeconds;
                    var configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();

                    if (int.TryParse(configUtilService.Get("SaveBetLogToSQLiteJobIntervalSeconds"), out int saveBetLogToSQLiteJobIntervalSeconds))
                    {
                        _saveBetLogToSQLiteJobIntervalSeconds = saveBetLogToSQLiteJobIntervalSeconds;
                    }
                }

                return _saveBetLogToSQLiteJobIntervalSeconds.Value;
            }
        }

        protected virtual int SaveBetLogToPlatformJobIntervalSeconds => 60;

        protected virtual int DoTransferCompensationJobIntervalSeconds => 60;

        #endregion Job頻率設定

        protected BaseTransferScheduleService()
        {
            try
            {
                string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;

                // 加上autofac
                var builder = new ContainerBuilder();
                builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, null);
                builder = DependencyUtilN6.GetJxBackendServiceContainerBuilder(assemblyPath, builder);
                builder = DependencyUtilN6.RegisterConfiguration(builder);
                builder = DependencyUtilN6.RegisterHttpContextAccessor(builder);
                AppendServiceToContainerBuilder(builder);
                DependencyUtil.SetContainer(builder.Build());

                TPGameApiService = DependencyUtil.ResolveJxBackendService<ITPGameApiService>(Product, Merchant, EnvUser, DbConnectionTypes.Master);
                TPGameApiReadService = DependencyUtil.ResolveJxBackendService<ITPGameApiReadService>(Product, Merchant, EnvUser, DbConnectionTypes.Slave);
                _merchantSettingService = DependencyUtil.ResolveJxBackendService<IMerchantSettingService>(Merchant, EnvUser, DbConnectionTypes.Slave);
                _tpGameTransferOutService = DependencyUtil.ResolveJxBackendService<ITPGameTransferOutService>(EnvUser, DbConnectionTypes.Master);
                _tpGameTransferOutQueueService = DependencyUtil.ResolveJxBackendService<ITPGameTransferOutQueueService>(EnvUser, DbConnectionTypes.Master);
                _messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(EnvUser.Application);
                LogUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            }
            catch (Exception ex)
            {
                try
                {
                    ErrorMsgUtil.ErrorHandle(ex, EnvUser);
                }
                catch (Exception)
                { //do nothing
                    throw ex;
                }
            }
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (!InitAppSettings(cancellationToken))
                {
                    return Task.CompletedTask;
                }

                InitSqlLite();

                AddNewTaskJob(nameof(DoTransferWork) + "1", () => DoTransferWork(cancellationToken), cancellationToken);
                //AddNewThreadJob(nameof(DoTransferWork) + "2", DoTransferWork);
                //AddNewThreadJob(nameof(DoTransferWork) + "3", DoTransferWork);

                //Timer具有時間到就啟動的特性，短間隔的JOB若用TIMER則有可能造成request併發
                if (IsRecheckProcessingStatusOrderJobEnabled)
                {
                    AddNewTaskJob(nameof(RecheckProcessingStatusOrderJob), () => RecheckProcessingStatusOrderJob(cancellationToken), cancellationToken);
                }

                if (IsSaveBetLogToSQLiteJobEnabled)
                {
                    AddNewTaskJob(nameof(SaveBetLogToSQLiteJob), () => SaveBetLogToSQLiteJob(cancellationToken), cancellationToken);
                }

                if (IsSaveBetLogToPlatformJobEnabled)
                {
                    AddNewTaskJob(nameof(SaveBetLogToPlatformJob), () => SaveBetLogToPlatformJob(cancellationToken), cancellationToken);
                }

                if (IsTransferAllOutJobEnabled)
                {
                    AddNewTaskJob(nameof(TransferAllOutJob), () => TransferAllOutJob(cancellationToken), cancellationToken);
                }

                if (IsClearExpiredProfitLossJobEnabled)
                {
                    AddNewTaskJob(nameof(ClearExpiredProfitLossJob), () => ClearExpiredProfitLossJob(cancellationToken), cancellationToken);
                }

                if (IsDoTransferCompensationJobEnabled)
                {
                    AddNewTaskJob(nameof(DoTransferCompensationJob), () => DoTransferCompensationJob(cancellationToken), cancellationToken);
                }

                AddDebugMessage("服務啟動, jobNames=" + JobNames.Keys.ToJsonString());

                Task.WaitAll(JobNames.Values.ToArray(), cancellationToken);
            }
            catch (Exception ex)
            {
                ErrorMsgUtil.ErrorHandle(ex, EnvUser);
                TaskUtil.DelayAndWait(2000);
            }

            return Task.CompletedTask;
        }

        protected void AddNewTaskJob(string jobName, Action job, CancellationToken cancellationToken)
        {
            JobNames.Add(jobName, Task.Run(job, cancellationToken));
        }

        private void AddDebugMessage(string debugMessage)
        {
            LogUtilService.ForcedDebug(debugMessage);

            TelegramUtil.SendMessageWithEnvInfoAsync(new SendTelegramParam()
            {
                ApiUrl = SharedAppSettings.TelegramApiUrl,
                EnvironmentUser = EnvUser,
                Message = debugMessage
            });
        }

        protected Dictionary<string, int> GetUserIdsFromTPGameAccounts(List<string> tpGameAccounts, bool isToLowerTPGameAccount)
        {
            var userMap = TPGameApiReadService.GetUserIdsFromTPGameAccounts(tpGameAccounts);

            if (isToLowerTPGameAccount)
            {
                userMap = userMap.ToDictionary(d => d.Key.ToLower(), d => d.Value);
            }

            return userMap;
        }

        protected virtual void InitSqlLite()
        {
            //因為並非所有第三方都有實作此服務,故移到這邊做服務注入
            _transferSqlLiteRepository = DependencyUtil.ResolveKeyed<ITransferSqlLiteRepository>(Product, Merchant);
            _transferSqlLiteRepository.TryCreateDataBase();
            _transferSqlLiteRepository.TryCreateTableProfitLoss();
            _transferSqlLiteRepository.TryCreateTableLastSearchToken();
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                AddDebugMessage("服務停止");
            }
            catch (Exception ex)
            {
                ErrorMsgUtil.ErrorHandle(ex, EnvUser);
            }
            finally
            {
                TaskUtil.DelayAndWait(2000);
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 重新確認處理中的訂單
        /// </summary>
        private void RecheckProcessingStatusOrderJob(CancellationToken cancellationToken)
        {
            //祕色沒有非同步轉帳流程,所以一啟動就可以先Recheck卡住的訂單,不會出現同時處理訂單的情況
            DoJobWithCancellationToken(
                cancellationToken,
                jobIntervalSeconds: RecheckProcessingStatusOrderJobIntervalSeconds,
                doJob: () =>
                {
                    var baseTPGameMoneyInfos = new List<BaseTPGameMoneyInfo>();

                    // 正在處理轉入訂單
                    baseTPGameMoneyInfos.AddRange(TPGameApiReadService.GetTPGameProcessingMoneyInInfo());
                    // 正在處理轉出訂單
                    baseTPGameMoneyInfos.AddRange(TPGameApiReadService.GetTPGameProcessingMoneyOutInfo());

                    lock (locker)
                    {
                        foreach (BaseTPGameMoneyInfo baseTPGameMoneyInfo in baseTPGameMoneyInfos)
                        {
                            AddTransferJob(TransferJobTypes.RecheckProcessingOrders, baseTPGameMoneyInfo);
                        }
                    }

                    return true;
                });
        }

        private void AddTransferJob(TransferJobTypes jobType, BaseTPGameMoneyInfo baseTPGameMoneyInfo)
        {
            bool isWaitJobExist = _waitTransferJobs.Any(w => w.TPGameMoneyInfo.GetType().Name == baseTPGameMoneyInfo.GetType().Name &&
            w.TPGameMoneyInfo.GetMoneyID() == baseTPGameMoneyInfo.GetMoneyID());

            bool isProcessingJobExist = _processingTransferJobs.Any(w => w.Key.TPGameMoneyInfo.GetType().Name == baseTPGameMoneyInfo.GetType().Name &&
            w.Key.TPGameMoneyInfo.GetMoneyID() == baseTPGameMoneyInfo.GetMoneyID());

            if (!isProcessingJobExist && !isWaitJobExist)
            {
                _waitTransferJobs.Enqueue(new TransferParamJob() { JobType = jobType, TPGameMoneyInfo = baseTPGameMoneyInfo });
            }
        }

        private void DoTransferWork(CancellationToken cancellationToken)
        {
            DoJobWithCancellationToken(
                cancellationToken,
                jobIntervalSeconds: 1,
                doJob: () =>
                {
                    TransferParamJob transferParamJob = null;

                    lock (locker)
                    {
                        if (_waitTransferJobs.Any())
                        {
                            _waitTransferJobs.TryDequeue(out transferParamJob);
                            _processingTransferJobs.TryAdd(transferParamJob, null);
                        }
                    }

                    if (transferParamJob == null)
                    {
                        return true;
                    }

                    var stopwatch = new Stopwatch();

                    try
                    {
                        stopwatch.Start();

                        switch (transferParamJob.JobType)
                        {
                            case TransferJobTypes.RecheckProcessingOrders:
                                TPGameApiService.RecheckProcessingOrders(transferParamJob.TPGameMoneyInfo);

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
                            MoneyID = transferParamJob.TPGameMoneyInfo.GetMoneyID(),
                            stopwatch.ElapsedMilliseconds
                        }.ToJsonString();

                        LogUtilService.ForcedDebug($"TransferJob完成, {info}");
                    }

                    return true;
                });
        }

        protected virtual void SaveBetLogToSQLiteJob(CancellationToken cancellationToken)
        {
            int currentGetBetLogFailCount = 0;

            DoJobWithCancellationToken(
                cancellationToken,
                jobIntervalSeconds: SaveBetLogToSQLiteJobIntervalSeconds,
                doJob: () =>
                {
                    string lastSearchToken = _transferSqlLiteRepository.GetLastSearchToken();

                    if (lastSearchToken.IsNullOrEmpty())
                    {
                        lastSearchToken = GetNextSearchToken(lastSearchToken, null);
                    }

                    BaseReturnDataModel<RequestAndResponse> requestReturnModel = TPGameApiService.GetRemoteBetLog(lastSearchToken);

                    if (!requestReturnModel.IsSuccess)
                    {
                        // 適用於Remote File情境，沒資料時候TOKEN不推進
                        if (requestReturnModel.Code == ReturnCode.NoDataChanged.Value)
                        {
                            return true;
                        }

                        throw new InvalidProgramException(requestReturnModel.Message);
                    }

                    BaseReturnDataModel<List<BetLogType>> betLogReturnModel = ConvertToBetLogs(requestReturnModel.DataModel.ResponseContent);

                    if (!betLogReturnModel.IsSuccess)
                    {
                        currentGetBetLogFailCount++;

                        if (currentGetBetLogFailCount == s_getBetLogFailMaxCount)
                        {
                            currentGetBetLogFailCount = 0;
                            ErrorMsgUtil.ErrorHandle(new Exception("ConvertToBetLogs Fail:" + requestReturnModel.DataModel.ResponseContent), EnvUser);
                        }

                        return true;
                    }

                    //排除平台以外的資料
                    List<BetLogType> betLogs = betLogReturnModel.DataModel;

                    if (betLogs.AnyAndNotNull())
                    {
                        List<string> tpGameAccounts = betLogs.Select(s => s.TPGameAccount).Distinct().ToList();
                        Dictionary<string, int> userMap = GetUserIdsFromTPGameAccounts(tpGameAccounts, IsToLowerTPGameAccount);

                        IEnumerable<BetLogType> removeBetLogs = betLogs.Where(r => !userMap.ContainsKey(r.TPGameAccount));
                        string removeDataJson = removeBetLogs.Select(s => s.TPGameAccount).Distinct().ToJsonString();
                        betLogs.RemoveAll(r => !userMap.ContainsKey(r.TPGameAccount));

                        //取得自訂memo
                        foreach (BetLogType betLog in betLogs)
                        {
                            betLog.Memo = GetCustomizeMemo(betLog).ToLocalizationJsonString();
                        }

                        //存入SQLite
                        _transferSqlLiteRepository.SaveProfitloss(betLogs);

                        if (TPGameApiReadService.IsBackupBetLog)
                        {
                            TPGameApiService.BackupBetLog(requestReturnModel);
                        }
                    }

                    //只要沒發生錯誤都要讓SearchToken往後更新, 不同的第三方有不同的推進方式
                    string newToken = GetNextSearchToken(lastSearchToken, requestReturnModel.DataModel);
                    //回寫searchToken
                    _transferSqlLiteRepository.UpdateNextSearchToken(newToken);

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

        protected virtual void SaveBetLogToPlatformJob(CancellationToken cancellationToken)
        {
            DoJobWithCancellationToken(
               cancellationToken,
               jobIntervalSeconds: SaveBetLogToPlatformJobIntervalSeconds,
               doJob: () =>
               {
                   List<BetLogType> betLogs = _transferSqlLiteRepository.GetBatchProfitlossNotSavedToRemote<BetLogType>();
                   DoSaveBetLogToPlatform(betLogs);

                   return true;
               });
        }

        protected virtual void DoTransferCompensationJob(CancellationToken cancellationToken)
        {
            DoJobWithCancellationToken(
               cancellationToken,
               jobIntervalSeconds: DoTransferCompensationJobIntervalSeconds,
               doJob: () =>
               {
                   var transferCompensationService = DependencyUtil.ResolveJxBackendService<ITransferCompensationService>(EnvUser, DbConnectionTypes.Slave);
                   List<int> userIds = transferCompensationService.GetTransferCompensationUserIds(Product);

                   foreach (int userId in userIds)
                   {
                       var transferOutUserDetail = new TransferOutUserDetail
                       {
                           AffectedUser = new BaseBasicUserInfo
                           {
                               UserId = userId
                           },
                           IsCompensation = true,
                       };

                       //放入queue做非同步處理
                       _messageQueueService.EnqueueTransferAllOutMessage(Product, transferOutUserDetail);
                   }

                   return true;
               });
        }

        private void TransferAllOutJob(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                _tpGameTransferOutQueueService.StartDequeueTransferAllOutJob(Product);
            }
        }

        /// <summary>每天5點删除过期的盈亏數據</summary>
        protected void ClearExpiredProfitLossJob(CancellationToken cancellationToken)
        {
            DoJobWithCancellationToken(
                cancellationToken,
                jobIntervalSeconds: 60 * 60,
                doJob: () =>
                {
                    if (DateTime.Now.Hour == 5)
                    {
                        ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
                        {
                            DoDeleteExpiredProfitLoss();
                        });
                    }

                    return true;
                });
        }

        protected virtual void DoDeleteExpiredProfitLoss()
        {
            _transferSqlLiteRepository.DeleteExpiredProfitLoss();
        }

        protected virtual void DoSaveBetLogToPlatform(List<BetLogType> betLogs)
        {
            List<InsertTPGameProfitlossParam> tpGameProfitlosses = ConvertToTPGameProfitloss(betLogs);
            var commonBetLogService = DependencyUtil.ResolveJxBackendService<ICommonBetLogService>(EnvUser, DbConnectionTypes.Master);
            commonBetLogService.SaveBetLogToPlatform(Product, tpGameProfitlosses);
        }

        protected void DoJobWithCancellationToken(CancellationToken cancellationToken, int jobIntervalSeconds, Func<bool> doJob)
        {
            int totalDelaySeconds = int.MaxValue;

            while (!cancellationToken.IsCancellationRequested)
            {
                bool isDelayAndWait = true;

                if (totalDelaySeconds >= jobIntervalSeconds)
                {
                    totalDelaySeconds = 0;

                    ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
                    {
                        isDelayAndWait = doJob.Invoke();
                    });
                }
                else
                {
                    totalDelaySeconds++;
                }

                if (isDelayAndWait)
                {
                    TaskUtil.DelayAndWait(1000);
                }
                else
                {
                    totalDelaySeconds = int.MaxValue;
                }
            }
        }
    }
}