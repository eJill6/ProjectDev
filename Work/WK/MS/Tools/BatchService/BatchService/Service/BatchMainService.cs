using Autofac;
using BatchService.Interface;
using BatchService.Model.Enum;
using BatchService.Service.Enum.Base;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendServiceNF.DependencyInjection;
using JxBackendServiceNF.Service.Util;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace BatchService.Service
{
    public partial class BatchMainService : ServiceBase
    {
        private static readonly Dictionary<PlatformMerchant, Type> _jobSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
        {
            {PlatformMerchant.MiseLiveStream, typeof(BaseJobSettingService) },
        };

        private static readonly int s_misfireThresholdMilliSeconds = 500;

        private readonly ILogUtilService _logUtilService;

        private readonly IJobSettingService _jobSettingService;

        private readonly EnvironmentUser _envUser = new EnvironmentUser()
        {
            Application = JxApplication.BatchService,
            LoginUser = new BasicUserInfo() { UserId = 0 }
        };

        public BatchMainService()
        {
            InitializeComponent();

            try
            {
                RegisterService();
                _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
                _jobSettingService = DependencyUtil.ResolveKeyed<IJobSettingService>(SharedAppSettings.PlatformMerchant);
            }
            catch (Exception ex)
            {
                ErrorMsgUtil.ErrorHandle(ex, _envUser);
                throw ex;
            }
        }

        protected void RegisterService()
        {
            string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + "\\";
            ContainerBuilder builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, null);
            builder = DependencyUtilNF.GetJxBackendServiceContainerBuilder(assemblyPath, builder);

            //.net framework版本高過元件的關係,quartz必須降級,改為註冊本地服務
            foreach (KeyValuePair<PlatformMerchant, Type> jobSettingServiceType in _jobSettingServiceTypeMap)
            {
                builder.RegisterType(jobSettingServiceType.Value).Keyed<IJobSettingService>(jobSettingServiceType.Key.Value);
            }

            AppendServiceToContainerBuilder(builder);
            DependencyUtil.SetContainer(builder.Build());
        }

        protected virtual void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        { }

        protected virtual List<JobSetting> GetJobSettings() => _jobSettingService.GetAll().Where(w => w.IsEnabled).ToList();

        private async Task RunSchedule()
        {
            var config = new NameValueCollection
            {
                { "quartz.jobStore.misfireThreshold", s_misfireThresholdMilliSeconds.ToString() }
            };

            var factory = new StdSchedulerFactory(config);
            IScheduler scheduler = await factory.GetScheduler();

            var jobNames = new List<string>();
            List<JobSetting> jobSettings = GetJobSettings();

            jobSettings.Where(w => w.ScheduleJobType == ScheduleJobTypes.Quartz).ToList().ForEach(jobSetting =>
            {
                // 建立 Job
                IJobDetail jobDetail = JobBuilder.Create(jobSetting.JobType).WithIdentity(jobSetting.Value).Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity(jobSetting.TriggerName)
                    .WithCronSchedule(jobSetting.CronExpression, x => x.WithMisfireHandlingInstructionDoNothing())
                    .Build();

                scheduler.ScheduleJob(jobDetail, trigger).Wait();
                jobNames.Add(jobSetting.JobType.Name);

                if (jobSetting.IsStartNow)
                {
                    scheduler.TriggerJob(jobDetail.Key, jobDetail.JobDataMap);
                }
            });

            await scheduler.Start();

            //加入thread類的timer
            jobSettings.Where(w => w.ScheduleJobType == ScheduleJobTypes.QueueUserWorkItem).ToList().ForEach(jobSetting =>
            {
                IQueueUserWorkItemJob queueUserWorkItemJob = (IQueueUserWorkItemJob)Activator.CreateInstance(jobSetting.JobType);
                jobNames.Add(jobSetting.JobType.Name);
                ThreadPool.QueueUserWorkItem(new WaitCallback(queueUserWorkItemJob.DoJob));
            });

            string debugMessage = new
            {
                Title = "BatchService 服務啟動",
                Environment.MachineName,
                PlatformMerchantCode = SharedAppSettings.PlatformMerchant.Value,
                EnvironmentCode = SharedAppSettings.GetEnvironmentCode(JxApplication.BatchService).Value,
                Jobs = jobNames
            }.ToJsonString();

            AddDebugMessage(debugMessage);
        }

        protected override void OnStart(string[] args)
        {
            RunSchedule().Wait();
            //.GetAwaiter().GetResult();
        }

        protected override void OnStop()
        {
            try
            {
                AddDebugMessage("服務停止");
                base.OnStop();
            }
            catch (Exception ex)
            {
                ErrorMsgUtil.ErrorHandle(ex, _envUser);
            }
            finally
            {
                Thread.Sleep(2000);
            }
        }

        private void AddDebugMessage(string debugMessage)
        {
            _logUtilService.ForcedDebug(debugMessage);

            TelegramUtil.SendMessageWithEnvInfoAsync(new SendTelegramParam()
            {
                ApiUrl = SharedAppSettings.TelegramApiUrl,
                EnvironmentUser = _envUser,
                Message = debugMessage
            });
        }
    }
}