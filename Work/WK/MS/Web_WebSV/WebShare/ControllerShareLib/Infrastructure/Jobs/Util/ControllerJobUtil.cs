using ControllerShareLib.Infrastructure.Jobs.Setting;
using Quartz;

namespace ControllerShareLib.Infrastructure.Jobs.Util
{
    public static class ControllerJobUtil
    {
        public static void AddQuartzJobs(this IServiceCollectionQuartzConfigurator quartzConfigurator, List<ControllerJobSetting> controllerJobSettings)
        {
            foreach (ControllerJobSetting controllerJobSetting in controllerJobSettings)
            {
                var jobKey = new JobKey(controllerJobSetting.Value);

                quartzConfigurator.AddJob(
                    jobType: controllerJobSetting.JobType,
                    configure: (c) =>
                    {
                        c.WithIdentity(jobKey);
                    });

                quartzConfigurator.AddTrigger(configure: (trigger) =>
                {
                    trigger.StartAt(DateTimeOffset.Now.AddSeconds(controllerJobSetting.StartDelaySeconds))
                       .ForJob(jobKey)
                       .WithIdentity($"{jobKey.Name} Trigger");

                    if (controllerJobSetting.IntervalSeconds > 0)
                    {
                        trigger.WithSimpleSchedule(x =>
                             x.WithMisfireHandlingInstructionIgnoreMisfires()
                             .WithIntervalInSeconds(controllerJobSetting.IntervalSeconds)
                             .RepeatForever());
                    }
                });
            }

            quartzConfigurator.SetProperty("quartz.jobStore.misfireThreshold", "1000");
            quartzConfigurator.UseSimpleTypeLoader();
            quartzConfigurator.UseInMemoryStore();

            quartzConfigurator.UseDefaultThreadPool(tp =>
            {
                tp.MaxConcurrency = 20;
            });
        }
    }
}