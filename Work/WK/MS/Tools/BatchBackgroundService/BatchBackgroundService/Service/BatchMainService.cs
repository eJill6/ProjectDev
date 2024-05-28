using Autofac;
using BatchService.Interface;
using BatchService.Model.Enum;
using BatchService.Service.Enum.Base;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Service.SMS;
using JxBackendServiceN6.Service.Background;
using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;

namespace BatchService
{
    public class BatchMainService : BaseBackgroundService
    {
        private static readonly Dictionary<PlatformMerchant, Type> _jobSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
        {
            {PlatformMerchant.MiseLiveStream, typeof(BaseJobSettingService) },
        };

        private static readonly int s_misfireThresholdMilliSeconds = 500;

        private readonly Lazy<IJobSettingService> _jobSettingService;

        public BatchMainService()
        {
            try
            {
                _jobSettingService = DependencyUtil.ResolveKeyed<IJobSettingService>(SharedAppSettings.PlatformMerchant);
            }
            catch (Exception ex)
            {
                ErrorMsgUtil.ErrorHandle(ex, EnvUser);

                throw;
            }
        }

        protected override Type MainBackgroundServiceType => typeof(BatchMainService);

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            foreach (KeyValuePair<PlatformMerchant, Type> jobSettingServiceType in _jobSettingServiceTypeMap)
            {
                containerBuilder.RegisterType(jobSettingServiceType.Value).Keyed<IJobSettingService>(jobSettingServiceType.Key.Value);
            }

            if (EnvUser.EnvironmentCode.IsTestingEnvironment)
            {
                containerBuilder.RegisterType(typeof(SendSMSManagerMockService)).AsImplementedInterfaces();
            }
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await RunSchedule(cancellationToken);
        }

        protected virtual List<JobSetting> GetJobSettings() => _jobSettingService.Value.GetAll().Where(w => w.IsEnabled).ToList();

        private async Task RunSchedule(CancellationToken cancellationToken)
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

                scheduler.ScheduleJob(jobDetail, trigger, cancellationToken).Wait();
                jobNames.Add(jobSetting.JobType.Name);

                if (jobSetting.IsTriggerQuartzJobOnStartup)
                {
                    scheduler.TriggerJob(jobDetail.Key, jobDetail.JobDataMap);
                }
            });

            await scheduler.Start();

            //加入task類的timer
            jobSettings.Where(w => w.ScheduleJobType == ScheduleJobTypes.QueueUserWorkItem).ToList().ForEach(jobSetting =>
            {
                ITaskJob taskJob = (ITaskJob)Activator.CreateInstance(jobSetting.JobType);
                jobNames.Add(jobSetting.JobType.Name);
                Task.Run(() => taskJob.DoJob(cancellationToken));
            });

            string debugMessage = new
            {
                Title = "BatchService 服務啟動",
                Environment.MachineName,
                PlatformMerchantCode = SharedAppSettings.PlatformMerchant.Value,
                EnvironmentCode = SharedAppSettings.GetEnvironmentCode().Value,
                Jobs = jobNames
            }.ToJsonString();

            AddDebugMessage(debugMessage);
        }
    }
}