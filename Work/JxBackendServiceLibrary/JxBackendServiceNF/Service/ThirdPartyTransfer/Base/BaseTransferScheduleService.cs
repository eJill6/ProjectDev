using Autofac;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Merchant;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer;
using JxBackendServiceNF.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace JxBackendServiceNF.Service.ThirdPartyTransfer.Base
{
    public abstract class BaseTransferScheduleService<BetLogType> : ServiceBase where BetLogType : BaseRemoteBetLog
    {
        private static readonly int s_getBetLogFailMaxCount = 10;

        protected int MaxDetailMemoContentCount => 8;

        protected readonly ITPGameApiService _tpGameApiService;

        protected readonly ITPGameApiReadService _tpGameApiReadService;

        private readonly IMerchantSettingService _merchantSettingService;

        private ITransferSqlLiteRepository _transferSqlLiteRepository;

        private readonly ITPGameTransferOutService _tpGameTransferOutService;

        private readonly ITPGameTransferOutQueueService _tpGameTransferOutQueueService;

        private readonly IMessageQueueService _messageQueueService;

        protected readonly List<string> _jobNames = new List<string>();

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

        protected static bool IsWork { get; private set; } = true;

        /// <summary>
        /// OnStart時觸發，初始化設定檔參數
        /// </summary>
        /// <returns></returns>
        public virtual bool InitAppSettings()
        { return true; }

        public abstract PlatformProduct Product { get; }

        public abstract JxApplication Application { get; }

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

        protected readonly int _saveBetLogToSQLiteJobIntervalSeconds =
            new Lazy<int>(() => ScheduleSettingUtil.GetSaveBetLogToSQLiteJobIntervalSeconds()).Value;

        protected virtual int SaveBetLogToPlatformJobIntervalSeconds => 60;

        protected virtual int DoTransferCompensationJobIntervalSeconds => 60;

        #endregion Job頻率設定

        public BaseTransferScheduleService()
        {
            try
            {
                string assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;

                // 加上autofac
                var builder = new ContainerBuilder();
                builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, null);
                builder = DependencyUtilNF.GetJxBackendServiceContainerBuilder(assemblyPath, builder);
                AppendServiceToContainerBuilder(builder);
                DependencyUtil.SetContainer(builder.Build());

                _tpGameApiService = DependencyUtil.ResolveJxBackendService<ITPGameApiService>(Product, Merchant, EnvUser, DbConnectionTypes.Master);
                _tpGameApiReadService = DependencyUtil.ResolveJxBackendService<ITPGameApiReadService>(Product, Merchant, EnvUser, DbConnectionTypes.Slave);
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

        protected override void OnStart(string[] args)
        {
            try
            {
                IsWork = true;

                if (!InitAppSettings())
                {
                    IsWork = false;
                    return;
                }

                InitSqlLite();

                AddNewThreadJob(nameof(DoTransferWork) + "1", DoTransferWork);
                //AddNewThreadJob(nameof(DoTransferWork) + "2", DoTransferWork);
                //AddNewThreadJob(nameof(DoTransferWork) + "3", DoTransferWork);

                //Timer具有時間到就啟動的特性，短間隔的JOB若用TIMER則有可能造成request併發
                if (IsRecheckProcessingStatusOrderJobEnabled)
                {
                    AddNewThreadJob(nameof(RecheckProcessingStatusOrderJob), RecheckProcessingStatusOrderJob);
                }

                if (IsSaveBetLogToSQLiteJobEnabled)
                {
                    AddNewThreadJob(nameof(SaveBetLogToSQLiteJob), SaveBetLogToSQLiteJob);
                }

                if (IsSaveBetLogToPlatformJobEnabled)
                {
                    AddNewThreadJob(nameof(SaveBetLogToPlatformJob), SaveBetLogToPlatformJob);
                }

                if (IsTransferAllOutJobEnabled)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(TransferAllOutJob), string.Empty);
                }

                if (IsClearExpiredProfitLossJobEnabled)
                {
                    var hourTimeInterval = 60 * 60 * 1000;
                    var clearExpiredProfitLoss = new System.Timers.Timer(hourTimeInterval);
                    clearExpiredProfitLoss.Elapsed += ClearExpiredProfitLossJob;
                    clearExpiredProfitLoss.AutoReset = true;
                    clearExpiredProfitLoss.Enabled = true;
                    clearExpiredProfitLoss.Start();
                    ClearExpiredProfitLossJob(null, null); //執行就先啟動
                    _jobNames.Add(nameof(ClearExpiredProfitLossJob));
                }

                if (IsDoTransferCompensationJobEnabled)
                {
                    AddNewThreadJob(nameof(DoTransferCompensationJob), DoTransferCompensationJob);
                }

                AddDebugMessage("服務啟動, jobNames=" + _jobNames.ToJsonString());
            }
            catch (Exception ex)
            {
                ErrorMsgUtil.ErrorHandle(ex, EnvUser);
                Task.Delay(2000).Wait();
            }
        }

        protected void AddNewThreadJob(string jobName, Action<object> job)
        {
            _jobNames.Add(jobName);
            ThreadPool.QueueUserWorkItem(new WaitCallback(job));
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
            var userMap = _tpGameApiReadService.GetUserIdsFromTPGameAccounts(tpGameAccounts);

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

        protected override void OnStop()
        {
            IsWork = false;

            try
            {
                AddDebugMessage("服務停止");
                base.OnStop();
            }
            catch (Exception ex)
            {
                ErrorMsgUtil.ErrorHandle(ex, EnvUser);
            }
            finally
            {
                Task.Delay(2000).Wait();
            }
        }

        /// <summary>
        /// 重新確認處理中的訂單
        /// </summary>
        /// <param name="state"></param>
        private void RecheckProcessingStatusOrderJob(object state)
        {
            while (IsWork)
            {
                //改到執行前先暫停, 避免服務中斷太久時剛啟動時出現併發
                //3分鐘一次, (配合抓取資料改為3分鐘)
                Task.Delay(RecheckProcessingStatusOrderJobIntervalSeconds * 1000).Wait();

                ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
                {
                    var baseTPGameMoneyInfos = new List<BaseTPGameMoneyInfo>();

                    // 正在處理轉入訂單
                    baseTPGameMoneyInfos.AddRange(_tpGameApiReadService.GetTPGameProcessingMoneyInInfo());
                    // 正在處理轉出訂單
                    baseTPGameMoneyInfos.AddRange(_tpGameApiReadService.GetTPGameProcessingMoneyOutInfo());

                    lock (locker)
                    {
                        foreach (BaseTPGameMoneyInfo baseTPGameMoneyInfo in baseTPGameMoneyInfos)
                        {
                            AddTransferJob(TransferJobTypes.RecheckProcessingOrders, baseTPGameMoneyInfo);
                        }
                    }
                });
            }
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

        private void DoTransferWork(object state)
        {
            while (IsWork)
            {
                ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
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
                                _tpGameApiService.RecheckProcessingOrders(transferParamJob.TPGameMoneyInfo);
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
                        Task.Delay(1000).Wait();
                    }
                });
            }
        }

        protected virtual void SaveBetLogToSQLiteJob(object state)
        {
            int currentGetBetLogFailCount = 0;

            while (IsWork)
            {
                ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
                {
                    string lastSearchToken = _transferSqlLiteRepository.GetLastSearchToken();

                    if (lastSearchToken.IsNullOrEmpty())
                    {
                        lastSearchToken = GetNextSearchToken(lastSearchToken, null);
                    }

                    BaseReturnDataModel<RequestAndResponse> requestReturnModel = _tpGameApiService.GetRemoteBetLog(lastSearchToken);

                    if (!requestReturnModel.IsSuccess)
                    {
                        // 適用於FTP情境，沒資料時候TOKEN不推進
                        if (requestReturnModel.Code == ReturnCode.NoDataChanged.Value)
                        {
                            return;
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

                        return;
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

                        if (_tpGameApiReadService.IsBackupBetLog)
                        {
                            _tpGameApiService.BackupBetLog(requestReturnModel);
                        }
                    }

                    //只要沒發生錯誤都要讓SearchToken往後更新, 不同的第三方有不同的推進方式
                    string newToken = GetNextSearchToken(lastSearchToken, requestReturnModel.DataModel);
                    //回寫searchToken
                    _transferSqlLiteRepository.UpdateNextSearchToken(newToken);
                });

                if (!RemoteFileSetting.HasNewRemoteFile)
                {
                    Task.Delay(_saveBetLogToSQLiteJobIntervalSeconds * 1000).Wait();
                }

                RemoteFileSetting.HasNewRemoteFile = false;
            }
        }

        protected virtual void SaveBetLogToPlatformJob(object state)
        {
            while (IsWork)
            {
                ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
                {
                    List<BetLogType> betLogs = _transferSqlLiteRepository.GetBatchProfitlossNotSavedToRemote<BetLogType>();
                    DoSaveBetLogToPlatform(betLogs);
                });

                Task.Delay(SaveBetLogToPlatformJobIntervalSeconds * 1000).Wait();
            }
        }

        protected virtual void DoTransferCompensationJob(object state)
        {
            while (IsWork)
            {
                ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
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
                });

                Task.Delay(DoTransferCompensationJobIntervalSeconds * 1000).Wait();
            }
        }

        private void TransferAllOutJob(object state)
        {
            _tpGameTransferOutQueueService.StartDequeueTransferAllOutJob(Product);
        }

        private bool DoJobAfterReceived(string message)
        {
            TransferOutUserDetail transferOutUserDetail;

            try
            {
                transferOutUserDetail = message.Deserialize<TransferOutUserDetail>();
            }
            catch (Exception ex)
            {
                LogUtilService.ForcedDebug($"TransferAllOut DoJobAfterReceived:{message}");
                ErrorMsgUtil.ErrorHandle(ex, EnvUser);

                return true;
            }

            bool isSuccess = _tpGameTransferOutService.ProcessTransferAllOutQueue(Product, transferOutUserDetail);

            if (!isSuccess)
            {
                LogUtilService.ForcedDebug($"ProcessTransferAllOutQueue Fail:{transferOutUserDetail.ToJsonString()}");
            }

            //不管成功或失敗要讓queue繼續處理下一筆, 最多讓用戶重新轉帳回去
            return true;
        }

        /// <summary>
        /// 每天5點删除过期的盈亏數據
        /// </summary>
        protected void ClearExpiredProfitLossJob(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!IsWork)
            {
                return;
            }

            if (DateTime.Now.Hour == 5)
            {
                ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
                {
                    DoDeleteExpiredProfitLoss();
                });
            }
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
    }

    public class ScheduleSettingUtil
    {
        public static readonly int DefaultSaveBetLogToSQLiteJobIntervalSeconds = 60;

        public static int GetSaveBetLogToSQLiteJobIntervalSeconds()
        {
            if (int.TryParse(ConfigurationManager.AppSettings["SaveBetLogToSQLiteJobIntervalSeconds"], out int saveBetLogToSQLiteJobIntervalSeconds))
            {
                return saveBetLogToSQLiteJobIntervalSeconds;
            }

            return DefaultSaveBetLogToSQLiteJobIntervalSeconds;
        }
    }
}