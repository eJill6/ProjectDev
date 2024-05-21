using Autofac;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.Merchant;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ErrorHandle;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer;
using JxBackendServiceN6.Service.Background;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace JxBackendServiceN6.Service.ThirdPartyTransfer.Base
{
    public abstract class BaseTransferScheduleService<BetLogType> : BaseBackgroundService where BetLogType : BaseRemoteBetLog
    {
        private static readonly int s_getBetLogFailMaxCount = 10;

        private static readonly int s_allUserTransferOutWorkerCount = 10;

        protected int MaxDetailMemoContentCount => 8;

        private readonly Lazy<IMerchantSettingService> _merchantSettingService;

        private ITransferSqlLiteRepository _transferSqlLiteRepository;

        private readonly Lazy<ITransferSqlLiteBackupRepository> _transferSqlLiteBackupRepository;

        private readonly Lazy<ITPGameTransferOutService> _tpGameTransferOutService;

        private readonly Lazy<ITPGameTransferOutQueueService> _tpGameTransferOutQueueService;

        private readonly Lazy<IMessageQueueService> _messageQueueService;

        private readonly Lazy<IInternalMessageQueueService> _internalMessageQueueService;

        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        protected Dictionary<string, Task> JobNames { get; private set; } = new Dictionary<string, Task>();

        protected ITPGameApiService TPGameApiService => _tpGameApiService.Value;

        protected ITPGameApiReadService TPGameApiReadService => _tpGameApiReadService.Value;

        private static readonly ConcurrentQueue<TransferParamJob> s_waitTransferJobs = new ConcurrentQueue<TransferParamJob>();

        private static readonly ConcurrentDictionary<TransferParamJob, object> s_processingTransferJobs = new ConcurrentDictionary<TransferParamJob, object>();

        private static readonly ConcurrentDictionary<int, object> s_allUserTransferResultMap = new ConcurrentDictionary<int, object>();

        private static readonly object s_locker = new object();

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

        protected JxApplication Application => s_environmentService.Value.Application;

        /// <summary>轉換為已結算的投注資料</summary>
        protected abstract BaseReturnDataModel<List<BetLogType>> ConvertToBetLogs(string apiResult);

        protected abstract string GetNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse);

        /// <summary>把第三方投注紀錄模型轉換為平台的盈虧模型</summary>
        protected abstract List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(Dictionary<string, int> userMap, List<BetLogType> betLogs);

        protected abstract LocalizationParam GetCustomizeMemo(BetLogType betLog);

        /// <summary>
        /// 取得平台紀錄的第三方帳號名稱是否轉小寫
        /// </summary>
        protected virtual bool IsToLowerTPGameAccount => false;

        protected bool IsComputeAdmissionBetMoney => _merchantSettingService.Value.IsComputeAdmissionBetMoney;

        #region Job開關

        protected virtual bool IsRecheckProcessingStatusOrderJobEnabled => true;

        protected virtual bool IsSaveRemoteBetLogToPlatformJobEnabled => true;

        protected virtual bool IsTransferAllOutJobEnabled => true;

        protected virtual bool IsUpdateTPGameUserScoreJobEnabled => true;

        protected virtual bool IsClearExpiredProfitLossJobEnabled => true;

        protected virtual bool IsDoTransferCompensationJobEnabled => false;

        /// <summary>是否啟用回收所有用戶餘額(用於更換商戶,前台遊戲開關需先關閉避免用戶不斷轉入)</summary>
        protected virtual bool IsAllUserTransferOutEnabled
        {
            get
            {
                //透過config設定，預設為關閉, 回收完成請sre關閉
                var configUtilService = DependencyUtil.ResolveService<IConfigUtilService>().Value;

                return configUtilService.Get("IsAllUserTransferOutEnabled", "0") == "1";
            }
        }

        #endregion Job開關

        #region Job頻率設定

        protected virtual int TransferInJobIntervalSeconds => 10;

        protected virtual int TransferOutJobIntervalSeconds => 10;

        protected virtual int RecheckProcessingStatusOrderJobIntervalSeconds => 180;

        private static readonly int s_defaultSaveRemoteBetLogToPlatformJobIntervalSeconds = 60;

        private readonly Lazy<ITPGameApiService> _tpGameApiService;

        private readonly Lazy<ITPGameApiReadService> _tpGameApiReadService;

        private int? _saveRemoteBetLogToPlatformJobIntervalSeconds;

        protected int SaveRemoteBetLogToPlatformJobIntervalSeconds
        {
            get
            {
                if (!_saveRemoteBetLogToPlatformJobIntervalSeconds.HasValue)
                {
                    _saveRemoteBetLogToPlatformJobIntervalSeconds = s_defaultSaveRemoteBetLogToPlatformJobIntervalSeconds;
                    var configUtilService = DependencyUtil.ResolveService<IConfigUtilService>().Value;

                    if (int.TryParse(configUtilService.Get("SaveRemoteBetLogToPlatformJobIntervalSeconds"), out int saveBetLogToSQLiteJobIntervalSeconds))
                    {
                        _saveRemoteBetLogToPlatformJobIntervalSeconds = saveBetLogToSQLiteJobIntervalSeconds;
                    }
                }

                return _saveRemoteBetLogToPlatformJobIntervalSeconds.Value;
            }
        }

        protected virtual int DoTransferCompensationJobIntervalSeconds => 60;

        protected virtual int DoAllUserTransferOutJobIntervalSeconds => 60;

        #endregion Job頻率設定

        protected BaseTransferScheduleService()
        {
            try
            {
                _tpGameApiService = DependencyUtil.ResolveJxBackendService<ITPGameApiService>(Product, Merchant, EnvUser, DbConnectionTypes.Master);
                _tpGameApiReadService = DependencyUtil.ResolveJxBackendService<ITPGameApiReadService>(Product, Merchant, EnvUser, DbConnectionTypes.Slave);
                _merchantSettingService = DependencyUtil.ResolveJxBackendService<IMerchantSettingService>(Merchant, EnvUser, DbConnectionTypes.Slave);
                _tpGameTransferOutService = DependencyUtil.ResolveJxBackendService<ITPGameTransferOutService>(EnvUser, DbConnectionTypes.Master);
                _tpGameTransferOutQueueService = DependencyUtil.ResolveJxBackendService<ITPGameTransferOutQueueService>(EnvUser, DbConnectionTypes.Master);
                _messageQueueService = DependencyUtil.ResolveService<IMessageQueueService>();
                _internalMessageQueueService = DependencyUtil.ResolveService<IInternalMessageQueueService>();
                _transferSqlLiteBackupRepository = DependencyUtil.ResolveService<ITransferSqlLiteBackupRepository>();
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

                if (IsSaveRemoteBetLogToPlatformJobEnabled)
                {
                    AddNewTaskJob(nameof(SaveRemoteBetLogToPlatformJob), () => SaveRemoteBetLogToPlatformJob(cancellationToken), cancellationToken);
                }

                if (IsTransferAllOutJobEnabled)
                {
                    AddNewTaskJob(nameof(TransferAllOutJob), () => TransferAllOutJob(cancellationToken), cancellationToken);
                }

                if (IsUpdateTPGameUserScoreJobEnabled)
                {
                    AddNewTaskJob(nameof(UpdateTPGameUserScoreJob), () => UpdateTPGameUserScoreJob(cancellationToken), cancellationToken);
                }

                if (IsClearExpiredProfitLossJobEnabled)
                {
                    AddNewTaskJob(nameof(ClearExpiredProfitLossJob), () => ClearExpiredProfitLossJob(cancellationToken), cancellationToken);
                }

                if (IsDoTransferCompensationJobEnabled)
                {
                    AddNewTaskJob(nameof(DoTransferCompensationJob), () => DoTransferCompensationJob(cancellationToken), cancellationToken);
                }

                if (IsAllUserTransferOutEnabled)
                {
                    AddNewTaskJob(nameof(DoAllUserTransferOutJob), () => DoAllUserTransferOutJob(cancellationToken), cancellationToken);
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
            _transferSqlLiteRepository = DependencyUtil.ResolveKeyed<ITransferSqlLiteRepository>(Product, Merchant).Value;
            _transferSqlLiteRepository.InitSettings();
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

                    lock (s_locker)
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
            bool isWaitJobExist = s_waitTransferJobs.Any(w => w.TPGameMoneyInfo.GetType().Name == baseTPGameMoneyInfo.GetType().Name &&
            w.TPGameMoneyInfo.GetMoneyID() == baseTPGameMoneyInfo.GetMoneyID());

            bool isProcessingJobExist = s_processingTransferJobs.Any(w => w.Key.TPGameMoneyInfo.GetType().Name == baseTPGameMoneyInfo.GetType().Name &&
            w.Key.TPGameMoneyInfo.GetMoneyID() == baseTPGameMoneyInfo.GetMoneyID());

            if (!isProcessingJobExist && !isWaitJobExist)
            {
                s_waitTransferJobs.Enqueue(new TransferParamJob() { JobType = jobType, TPGameMoneyInfo = baseTPGameMoneyInfo });
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

                    lock (s_locker)
                    {
                        if (s_waitTransferJobs.Any())
                        {
                            s_waitTransferJobs.TryDequeue(out transferParamJob);
                            s_processingTransferJobs.TryAdd(transferParamJob, null);
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
                        s_processingTransferJobs.TryRemove(transferParamJob, out object value);
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

        protected virtual void SaveRemoteBetLogToPlatformJob(CancellationToken cancellationToken)
        {
            int currentGetBetLogFailCount = 0;

            DoJobWithCancellationToken(
                cancellationToken,
                jobIntervalSeconds: SaveRemoteBetLogToPlatformJobIntervalSeconds,
                doJob: () =>
                {
                    string lastSearchToken = _transferSqlLiteRepository.GetLastSearchToken();

                    if (lastSearchToken.IsNullOrEmpty())
                    {
                        lastSearchToken = GetNextSearchToken(lastSearchToken, requestAndResponse: null);
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
                        betLogs = betLogs.DistinctBy(d => d.KeyId).ToList();
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

                        //備份到SQLite
                        BackupNewBetLogs(betLogs);

                        if (TPGameApiReadService.IsWriteRemoteContentToOtherMerchant)
                        {
                            TPGameApiService.WriteRemoteContentToOtherMerchant(requestReturnModel);
                        }

                        List<InsertTPGameProfitlossParam> tpGameProfitlosses = ConvertToTPGameProfitloss(userMap, betLogs);

                        TPGameApiService.SaveMultipleProfitlossToPlatform(
                            new SaveProfitlossToPlatformParam()
                            {
                                TPGameProfitlosses = tpGameProfitlosses,
                            });
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

        protected virtual void DoTransferCompensationJob(CancellationToken cancellationToken)
        {
            DoJobWithCancellationToken(
               cancellationToken,
               jobIntervalSeconds: DoTransferCompensationJobIntervalSeconds,
               doJob: () =>
               {
                   var transferCompensationService = DependencyUtil.ResolveJxBackendService<ITransferCompensationService>(EnvUser, DbConnectionTypes.Slave).Value;
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
                       _internalMessageQueueService.Value.EnqueueTransferAllOutMessage(Product, transferOutUserDetail);
                   }

                   return true;
               });
        }

        private void DoAllUserTransferOutJob(CancellationToken cancellationToken)
        {
            ITPGameUserInfoService tpGameUserInfoReadService = DependencyUtil.ResolveJxBackendService<ITPGameUserInfoService>(
                Product,
                SharedAppSettings.PlatformMerchant,
                EnvUser,
                DbConnectionTypes.Slave).Value;

            DoJobWithCancellationToken(
               cancellationToken,
               jobIntervalSeconds: DoAllUserTransferOutJobIntervalSeconds,
               doJob: () =>
               {
                   //找出有轉過第三方的用戶
                   List<BaseTPGameUserInfo> tpGameUserInfos = tpGameUserInfoReadService.GetUsersTransferedIn();

                   //排除已經轉回成功的用戶，且餘額為0的資料
                   tpGameUserInfos.RemoveAll(user =>
                       s_allUserTransferResultMap.ContainsKey(user.UserID) &&
                       user.AvailableScores == 0);

                   if (!tpGameUserInfos.Any())
                   {
                       return true;
                   }

                   var userQueue = new ConcurrentQueue<BaseTPGameUserInfo>(tpGameUserInfos);
                   var tasks = new List<Task>();

                   for (int i = 1; i <= s_allUserTransferOutWorkerCount; i++)
                   {
                       tasks.Add(Task.Run(() =>
                       {
                           while (userQueue.Any() && !cancellationToken.IsCancellationRequested)
                           {
                               if (!userQueue.TryDequeue(out BaseTPGameUserInfo tpGameUserInfo))
                               {
                                   TaskUtil.DelayAndWait(1000);

                                   continue;
                               }

                               ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
                               {
                                   //呼叫踢用戶下線API
                                   TPGameApiService.KickUser(tpGameUserInfo.UserID);

                                   var transferOutUserDetail = new TransferOutUserDetail()
                                   {
                                       AffectedUser = new BaseBasicUserInfo() { UserId = tpGameUserInfo.UserID },
                                       CorrelationId = Guid.NewGuid().ToString(),
                                   };

                                   bool isSuccess = _tpGameTransferOutService.Value.ProcessTransferAllOutQueue(Product, transferOutUserDetail);

                                   //加入成功轉回清單
                                   if (isSuccess && !s_allUserTransferResultMap.ContainsKey(tpGameUserInfo.UserID))
                                   {
                                       s_allUserTransferResultMap.TryAdd(tpGameUserInfo.UserID, null);
                                   }
                               });

                               TaskUtil.DelayAndWait(1000);
                           }
                       }));
                   }

                   Task.WaitAll(tasks.ToArray(), cancellationToken);

                   return true;
               });
        }

        private void TransferAllOutJob(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                _tpGameTransferOutQueueService.Value.StartDequeueTransferAllOutJob(Product);
            }
        }

        private void UpdateTPGameUserScoreJob(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                TPGameApiService.StartDequeueUpdateTPGameUserScoreJob();
            }
        }

        /// <summary>每天5點删除过期的盈亏數據</summary>
        private void ClearExpiredProfitLossJob(CancellationToken cancellationToken)
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
            _transferSqlLiteBackupRepository.Value.DeleteExpiredDbFile();
        }

        protected void BackupNewBetLogs(List<BetLogType> betLogs)
        {
            _transferSqlLiteBackupRepository.Value.BackupNewBetLogs(betLogs);
        }
    }
}