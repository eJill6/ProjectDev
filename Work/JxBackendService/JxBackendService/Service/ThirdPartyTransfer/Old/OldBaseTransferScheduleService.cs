using Autofac;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;

namespace JxBackendService.Service.ThirdPartyTransfer.Old
{
    /// <summary>
    /// 給舊版TransferService用的簡易版底層
    /// </summary>
    public abstract class OldBaseTransferScheduleService<ApiParamType> : ServiceBase
    {
        private readonly ITPGameApiService _tpGameApiService;
        private readonly ITPGameApiReadService _tpGameApiReadService;
        private readonly List<string> _jobNames = new List<string>();
        private static readonly ConcurrentQueue<OldTransferParamJob<ApiParamType>> _waitTransferJobs = new ConcurrentQueue<OldTransferParamJob<ApiParamType>>();
        private static readonly ConcurrentDictionary<OldTransferParamJob<ApiParamType>, object> _processingTransferJobs
            = new ConcurrentDictionary<OldTransferParamJob<ApiParamType>, object>();
        private static readonly object locker = new object();

        protected EnvironmentUser EnvUser => new EnvironmentUser()
        {
            Application = Application,
            LoginUser = new BasicUserInfo() { UserId = 0, UserName = GlobalVariables.SystemOperator }
        };

        protected abstract JxApplication Application { get; }

        protected abstract PlatformProduct Product { get; }

        protected PlatformMerchant Merchant { get; } = SharedAppSettings.PlatformMerchant;

        protected bool IsWork { get; private set; }

        protected virtual bool IsTransferInJobEnabled => true;

        protected virtual bool IsTransferOutJobEnabled => true;

        protected virtual bool IsRecheckProcessingStatusOrderJobEnabled => true;

        protected virtual int RecheckProcessingStatusOrderIntervalSeconds => 180;

        protected abstract bool DoInitialJobOnStart();

        protected abstract ApiParamType ConvertMoneyInInfoToApiParam(TPGameMoneyInInfo tpGameMoneyInInfo);

        protected abstract ApiParamType ConvertMoneyOutInfoToApiParam(TPGameMoneyOutInfo tpGameMoneyOutInfo);

        protected abstract Action<object> TransferCallback { get; }

        protected abstract Action<object> CheckProcessingOrderAndSaveToRemoteDBCallback { get; }

        protected virtual void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder) { }

        protected OldBaseTransferScheduleService()
        {
            try
            {
                string assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\";

                // 加上autofac
                ContainerBuilder builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, null);
                AppendServiceToContainerBuilder(builder);
                DependencyUtil.SetContainer(builder.Build());

                _tpGameApiService = DependencyUtil.ResolveJxBackendService<ITPGameApiService>(Product, Merchant, EnvUser, DbConnectionTypes.Master);
                _tpGameApiReadService = DependencyUtil.ResolveJxBackendService<ITPGameApiReadService>(Product, Merchant, EnvUser, DbConnectionTypes.Master); //尚無支援slave
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

            if (IsTransferInJobEnabled)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(TransferInJob), string.Empty);
            }

            if (IsTransferOutJobEnabled)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(TransferOutJob), string.Empty);
            }

            if (IsRecheckProcessingStatusOrderJobEnabled)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(RecheckProcessingStatusOrder), string.Empty);
            }

            LogUtil.ForcedDebug("服务已啟動");
        }

        /// <summary>
        /// 轉入第三方帳戶
        /// </summary>
        private void TransferInJob(object state)
        {
            while (IsWork)
            {
                //ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
                //{
                try
                {
                    if (_waitTransferJobs.Any())
                    {
                        LogUtil.ForcedDebug($" _waitTransferJobs count = {_waitTransferJobs.Count}");
                        Thread.Sleep(10 * 1000);
                        continue;
                    }

                    List<OldTPGameOrderParam<ApiParamType>> oldTPGameOrderParams = _tpGameApiService
                        .GetTPGameUnprocessedMoneyInInfo()
                        .Select(s => new OldTPGameOrderParam<ApiParamType>()
                        {
                            ApiParam = ConvertMoneyInInfoToApiParam(s),
                            TPGameMoneyInfo = s,
                        }).ToList();

                    lock (locker)
                    {
                        foreach (OldTPGameOrderParam<ApiParamType> oldTPGameOrderParam in oldTPGameOrderParams)
                        {
                            AddTransferJob(TransferJobTypes.TransferIn, oldTPGameOrderParam);

                            //ThreadPool.QueueUserWorkItem(new WaitCallback(TransferCallback), oldTPGameOrderParam);
                            ////每隔一秒開一個THREAD去處理訂單
                            //Thread.Sleep(1 * 1000);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                }
                //});


                Thread.Sleep(10 * 1000);
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
                //ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
                //{
                try
                {
                    if (_waitTransferJobs.Any())
                    {
                        LogUtil.ForcedDebug($" _waitTransferJobs count = {_waitTransferJobs.Count}");
                        Thread.Sleep(10 * 1000);
                        continue;
                    }

                    List<OldTPGameOrderParam<ApiParamType>> oldTPGameOrderParams = _tpGameApiService
                        .GetTPGameUnprocessedMoneyOutInfo()
                        .Select(s => new OldTPGameOrderParam<ApiParamType>()
                        {
                            ApiParam = ConvertMoneyOutInfoToApiParam(s),
                            TPGameMoneyInfo = s,
                        }).ToList();

                    lock (locker)
                    {
                        foreach (OldTPGameOrderParam<ApiParamType> oldTPGameOrderParam in oldTPGameOrderParams)
                        {
                            AddTransferJob(TransferJobTypes.TransferOut, oldTPGameOrderParam);
                            //ThreadPool.QueueUserWorkItem(new WaitCallback(TransferCallback), oldTPGameOrderParam);
                            ////每隔一秒開一個THREAD去處理訂單
                            //Thread.Sleep(1 * 1000);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                }
                //});

                Thread.Sleep(10 * 1000);
            }
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
                            ApiParam = ConvertMoneyInInfoToApiParam(s),
                            TPGameMoneyInfo = s,
                        }));

                    // 正在處理轉出訂單
                    oldTPGameOrderParams.AddRange(_tpGameApiReadService.GetTPGameProcessingMoneyOutInfo()
                        .Select(s => new OldTPGameOrderParam<ApiParamType>()
                        {
                            ApiParam = ConvertMoneyOutInfoToApiParam(s),
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
                    LogUtil.Error($"从{Product.Name}帐户檢查訂單狀態(CheckProcessingOrderAndSaveToRemoteDB)出现异常，信息：" + ex.Message + ",堆栈:" + ex.StackTrace);
                }
            }
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
                //ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
                //{
                try
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
                        Thread.Sleep(1000);
                        continue;
                    }

                    var stopwatch = new Stopwatch();

                    try
                    {
                        stopwatch.Start();

                        switch (transferParamJob.JobType)
                        {
                            case TransferJobTypes.TransferIn:
                            case TransferJobTypes.TransferOut:
                                TransferCallback(transferParamJob.OldTPGameOrderParam);
                                break;
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
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                }
                //});
            }
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

        protected virtual void DoJobOnStop() { }
    }
}
