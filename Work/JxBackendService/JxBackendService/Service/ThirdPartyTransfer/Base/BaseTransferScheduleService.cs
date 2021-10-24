using Autofac;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace JxBackendService.Service.ThirdPartyTransfer.Base
{
    public abstract class BaseTransferScheduleService<BetLogType> : ServiceBase where BetLogType : BaseRemoteBetLog
    {
        private static readonly int _getBetLogFailMaxCount = 10;
        private static readonly int _defaultSaveBetLogToSQLiteJobIntervalSeconds = 60;
        private static readonly int _maxMemoLength = 1020;
        private readonly ITPGameApiService _tpGameApiService;
        private readonly ITPGameApiReadService _tpGameApiReadService;
        private readonly ITransferSqlLiteRepository _transferSqlLiteRepository;
        private readonly List<string> _jobNames = new List<string>();
        private static readonly ConcurrentQueue<TransferParamJob> _waitTransferJobs = new ConcurrentQueue<TransferParamJob>();
        private static readonly ConcurrentDictionary<TransferParamJob, object> _processingTransferJobs = new ConcurrentDictionary<TransferParamJob, object>();
        private static readonly object locker = new object();

        protected EnvironmentUser EnvUser => new EnvironmentUser()
        {
            Application = Application,
            LoginUser = new BasicUserInfo() { UserId = 0, UserName = GlobalVariables.SystemOperator }
        };

        protected PlatformMerchant Merchant { get; } = SharedAppSettings.PlatformMerchant;
        protected static bool IsWork { get; private set; } = true;

        /// <summary>
        /// OnStart時觸發，初始化設定檔參數
        /// </summary>
        /// <returns></returns>
        public virtual bool InitAppSettings() { return true; }

        public abstract PlatformProduct Product { get; }

        public abstract JxApplication Application { get; }

        /// <summary>轉換為已結算的投注資料</summary>        
        protected abstract BaseReturnDataModel<List<BetLogType>> ConvertToBetLogs(string apiResult);

        protected abstract string GetNextSearchToken(string lastSearchToken, RequestAndResponse dataModel);

        /// <summary>把第三方投注紀錄模型轉換為平台的盈虧模型</summary>
        protected abstract List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(List<BetLogType> betLogs);

        protected abstract string GetCustomizeMemo(BetLogType betLog);

        /// <summary>
        /// 取得平台紀錄的第三方帳號名稱是否轉小寫
        /// </summary>
        protected virtual bool IsToLowerTPGameAccount => false;

        protected string MasterDbConnectionString { get; private set; }

        protected string SlaveDbConnectionString { get; private set; }

        #region Job開關
        protected virtual bool IsTransferInJobEnabled => true;

        protected virtual bool IsTransferOutJobEnabled => true;

        protected virtual bool IsRecheckProcessingStatusOrderJobEnabled => true;

        protected virtual bool IsSaveBetLogToSQLiteJobEnabled => true;

        protected virtual bool IsSaveBetLogToPlatformJobEnabled => true;

        protected virtual bool IsClearExpiredProfitLossJobEnabled => true;
        #endregion

        #region Job頻率設定
        protected virtual int TransferInJobIntervalSeconds => 10;

        protected virtual int TransferOutJobIntervalSeconds => 10;

        protected virtual int RecheckProcessingStatusOrderJobIntervalSeconds => 180;

        private readonly int _saveBetLogToSQLiteJobIntervalSeconds = new Lazy<int>(() =>
        {
            if (int.TryParse(ConfigurationManager.AppSettings["SaveBetLogToSQLiteJobIntervalSeconds"], out int saveBetLogToSQLiteJobIntervalSeconds))
            {
                return saveBetLogToSQLiteJobIntervalSeconds;
            }

            return _defaultSaveBetLogToSQLiteJobIntervalSeconds;
        }).Value;


        protected virtual int SaveBetLogToPlatformJobIntervalSeconds => 60;
        #endregion

        public BaseTransferScheduleService()
        {
            try
            {
                string assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\";

                // 加上autofac
                var builder = new ContainerBuilder();
                builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, null);
                builder = DependencyUtil.RegisterMockService(builder);
                DependencyUtil.SetContainer(builder.Build());

                _tpGameApiService = DependencyUtil.ResolveJxBackendService<ITPGameApiService>(
                    Product,
                    Merchant,
                    EnvUser,
                    DbConnectionTypes.Master);

                _tpGameApiReadService = DependencyUtil.ResolveJxBackendService<ITPGameApiReadService>(Product, Merchant, EnvUser, DbConnectionTypes.Slave);
                _transferSqlLiteRepository = DependencyUtil.ResolveKeyed<ITransferSqlLiteRepository>(Product, Merchant);
            }
            catch (Exception ex)
            {
                ErrorMsgUtil.ErrorHandle(ex, EnvUser);
                throw ex;
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
                if (IsTransferInJobEnabled)
                {
                    AddNewThreadJob(nameof(TransferInJob), TransferInJob);
                }

                if (IsTransferOutJobEnabled)
                {
                    AddNewThreadJob(nameof(TransferOutJob), TransferOutJob);
                }

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

                AddDebugMessage("服務啟動, jobNames=" + _jobNames.ToJsonString());
            }
            catch (Exception ex)
            {
                ErrorMsgUtil.ErrorHandle(ex, EnvUser);
                Thread.Sleep(2000);
            }
        }

        protected void AddNewThreadJob(string jobName, Action<object> job)
        {
            _jobNames.Add(jobName);
            ThreadPool.QueueUserWorkItem(new WaitCallback(job));
        }

        private void AddDebugMessage(string debugMessage)
        {
            LogUtil.ForcedDebug(debugMessage);
            TelegramUtil.SendMessageWithEnvInfoAsync(new Model.SendTelegramParam()
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

        private void InitSqlLite()
        {
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
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// 轉入第三方帳戶
        /// </summary>
        private void TransferInJob(object state)
        {
            while (IsWork)
            {
                ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
                {
                    if (_waitTransferJobs.Any())
                    {
                        return;
                    }

                    List<TPGameMoneyInInfo> tpGameMoneyInInfos = _tpGameApiService.GetTPGameUnprocessedMoneyInInfo();

                    lock (locker)
                    {
                        foreach (TPGameMoneyInInfo tpGameMoneyInInfo in tpGameMoneyInInfos)
                        {
                            AddTransferJob(TransferJobTypes.TransferIn, tpGameMoneyInInfo);
                        }
                    }
                });

                Thread.Sleep(TransferInJobIntervalSeconds * 1000);
            }
        }

        /// <summary>
        /// 轉出第三方帳戶
        /// </summary>
        /// <param name="state"></param>
        private void TransferOutJob(object state)
        {
            while (IsWork)
            {
                ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
                {
                    if (_waitTransferJobs.Any())
                    {
                        return;
                    }

                    List<TPGameMoneyOutInfo> tpGameMoneyOutInfos = _tpGameApiService.GetTPGameUnprocessedMoneyOutInfo();

                    lock (locker)
                    {
                        foreach (TPGameMoneyOutInfo tpGameMoneyOutInfo in tpGameMoneyOutInfos)
                        {
                            AddTransferJob(TransferJobTypes.TransferOut, tpGameMoneyOutInfo);
                        }
                    }
                });

                Thread.Sleep(TransferOutJobIntervalSeconds * 1000);
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
                Thread.Sleep(RecheckProcessingStatusOrderJobIntervalSeconds * 1000);

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
                        Thread.Sleep(1000);
                        return;
                    }

                    var stopwatch = new Stopwatch();

                    try
                    {
                        stopwatch.Start();

                        switch (transferParamJob.JobType)
                        {
                            case TransferJobTypes.TransferIn:
                                _tpGameApiService.TransferIn(transferParamJob.TPGameMoneyInfo);
                                break;
                            case TransferJobTypes.TransferOut:
                                _tpGameApiService.TransferOut(transferParamJob.TPGameMoneyInfo);
                                break;
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

                        LogUtil.ForcedDebug($"TransferJob完成, {info}");
                        Thread.Sleep(1000);
                    }
                });
            }
        }

        private void SaveBetLogToSQLiteJob(object state)
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
                        if(requestReturnModel.Code == ReturnCode.NoDataChanged.Value)
                        {
                            return;
                        }

                        throw new InvalidProgramException(requestReturnModel.Message);
                    }

                    BaseReturnDataModel<List<BetLogType>> betLogReturnModel = ConvertToBetLogs(requestReturnModel.DataModel.ResponseContent);

                    if (!betLogReturnModel.IsSuccess)
                    {
                        currentGetBetLogFailCount++;

                        if (currentGetBetLogFailCount == _getBetLogFailMaxCount)
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
                        List<string> tpGameAccounts = betLogs.Select(s => s.TPGameAccount).ToList();
                        Dictionary<string, int> userMap = GetUserIdsFromTPGameAccounts(tpGameAccounts, IsToLowerTPGameAccount);
                        betLogs.RemoveAll(r => !userMap.ContainsKey(r.TPGameAccount));

                        //取得自訂memo
                        foreach (BetLogType betLog in betLogs)
                        {
                            betLog.Memo = GetCustomizeMemo(betLog).ToShortString(_maxMemoLength);
                        }

                        //存入SQLite
                        _transferSqlLiteRepository.SaveProfitloss(betLogs);
                    }

                    //只要沒發生錯誤都要讓SearchToken往後更新, 不同的第三方有不同的推進方式
                    string newToken = GetNextSearchToken(lastSearchToken, requestReturnModel.DataModel);
                    //回寫searchToken
                    _transferSqlLiteRepository.UpdateNextSearchToken(newToken);
                });

                Thread.Sleep(_saveBetLogToSQLiteJobIntervalSeconds * 1000);
            }
        }


        private void SaveBetLogToPlatformJob(object state)
        {
            while (IsWork)
            {
                ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
                {
                    List<BetLogType> betLogs = _transferSqlLiteRepository.GetBatchProfitlossNotSavedToRemote<BetLogType>();
                    List<InsertTPGameProfitlossParam> tpGameProfitlosses = ConvertToTPGameProfitloss(betLogs);

                    Func<string, SaveBetLogFlags, bool> updateSQLiteToSavedStatus = (keyId, saveBetLogFlag) =>
                    {
                        int affectRowCount;

                        switch (saveBetLogFlag)
                        {
                            case SaveBetLogFlags.Success:
                                affectRowCount = _transferSqlLiteRepository.SaveProfitlossToPlatformSuccess(keyId);
                                break;
                            case SaveBetLogFlags.Fail:
                                affectRowCount = _transferSqlLiteRepository.SaveProfitlossToPlatformFail(keyId);
                                break;
                            case SaveBetLogFlags.Ignore:
                                affectRowCount = _transferSqlLiteRepository.SaveProfitlossToPlatformIgnore(keyId);
                                break;
                            default:
                                throw new NotImplementedException();
                        }

                        return affectRowCount > 0;
                    };

                    _tpGameApiService.SaveProfitlossToPlatform(tpGameProfitlosses, updateSQLiteToSavedStatus);
                });

                Thread.Sleep(SaveBetLogToPlatformJobIntervalSeconds * 1000);
            }
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
                    _transferSqlLiteRepository.DeleteExpiredProfitLoss();
                });
            }
        }
    }
}
