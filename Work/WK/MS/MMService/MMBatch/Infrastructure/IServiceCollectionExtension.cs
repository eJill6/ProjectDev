using Microsoft.Extensions.DependencyInjection;
using MMBatch.Infrastructure.Quartz;
using Quartz;

namespace MMService.Infrastructure
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddSchedule(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                q.MisfireThreshold = TimeSpan.FromSeconds(2000);
                q.UseMicrosoftDependencyInjectionJobFactory();

                ////範例
                //q.ScheduleJob<MMService.Infrastructure.Quartz.BaseJob>(trigger =>
                //   trigger
                //   .StartNow()
                //   .ForJob(new JobKey($"{nameof(MMService.Infrastructure.Quartz.BaseJob)}"))
                //   .WithIdentity($"{nameof(MMService.Infrastructure.Quartz.BaseJob)} Trigger")
                //   .WithSimpleSchedule(s =>
                //   {
                //       s.WithInterval(TimeSpan.FromMinutes(5))
                //       .WithMisfireHandlingInstructionIgnoreMisfires()
                //       .RepeatForever();
                //   }));
                q.ScheduleJob<CompleteBookingJob>(trigger =>
                  trigger
                  .StartNow()
                  .ForJob(new JobKey($"{nameof(CompleteBookingJob)}"))
                  .WithIdentity($"{nameof(CompleteBookingJob)} Trigger")
                  .WithSimpleSchedule(s =>
                  {
                      s.WithInterval(TimeSpan.FromMinutes(5))
                      .WithMisfireHandlingInstructionIgnoreMisfires()
                      .RepeatForever();
                  }));
                q.ScheduleJob<DistributePostIncomeJob>(trigger =>
                   trigger
                   .StartNow()
                   .ForJob(new JobKey($"{nameof(DistributePostIncomeJob)}"))
                   .WithIdentity($"{nameof(DistributePostIncomeJob)} Trigger")
                   .WithSimpleSchedule(s =>
                   {
                       s.WithInterval(TimeSpan.FromMinutes(5))
                       .WithMisfireHandlingInstructionIgnoreMisfires()
                       .RepeatForever();
                   }));

                q.ScheduleJob<TimeoutNoAcceptanceJob>(trigger =>
                   trigger
                   .StartNow()
                   .ForJob(new JobKey($"{nameof(TimeoutNoAcceptanceJob)}"))
                   .WithIdentity($"{nameof(TimeoutNoAcceptanceJob)} Trigger")
                   .WithSimpleSchedule(s =>
                   {
                       s.WithInterval(TimeSpan.FromMinutes(5))
                       .WithMisfireHandlingInstructionIgnoreMisfires()
                       .RepeatForever();
                   }));

                q.ScheduleJob<DistributeBookingIncomeJob>(trigger =>
                   trigger
                   .StartNow()
                   .ForJob(new JobKey($"{nameof(DistributeBookingIncomeJob)}"))
                   .WithIdentity($"{nameof(DistributeBookingIncomeJob)} Trigger")
                   .WithSimpleSchedule(s =>
                   {
                       s.WithInterval(TimeSpan.FromMinutes(1))
                       .WithMisfireHandlingInstructionIgnoreMisfires()
                       .RepeatForever();
                   }));

                q.ScheduleJob<RestSetUserUnLock>(trigger =>
                   trigger
                   .StartAt(DateBuilder.TomorrowAt(0, 0, 0))
                   .ForJob(new JobKey($"{nameof(RestSetUserUnLock)}"))
                   .WithIdentity($"{nameof(RestSetUserUnLock)} Trigger")
                   .WithSimpleSchedule(s =>
                   {
                       s.WithInterval(TimeSpan.FromDays(1))
                       .WithMisfireHandlingInstructionIgnoreMisfires()
                       .RepeatForever();
                   }));



                q.ScheduleJob<AuditAbnormalOrderJob>(trigger =>
                   trigger
                   .StartAt(DateBuilder.TomorrowAt(3, 0, 0))
                   .ForJob(new JobKey($"{nameof(AuditAbnormalOrderJob)}"))
                   .WithIdentity($"{nameof(AuditAbnormalOrderJob)} Trigger")
                   .WithSimpleSchedule(s =>
                   {
                       s.WithInterval(TimeSpan.FromDays(1))
                       .WithMisfireHandlingInstructionIgnoreMisfires()
                       .RepeatForever();
                   }));

                q.ScheduleJob<VideoEncryptJob>(trigger =>
                   trigger
                   .StartNow()
                   .ForJob(new JobKey($"{nameof(VideoEncryptJob)}"))
                   .WithIdentity($"{nameof(VideoEncryptJob)} Trigger")
                   .WithSimpleSchedule(s =>
                   {
                       s.WithInterval(TimeSpan.FromSeconds(10))
                       .WithMisfireHandlingInstructionIgnoreMisfires()
                       .RepeatForever();
                   }));

                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
                q.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = 20;
                });



                //q.ScheduleJob<ImageEncryptJob>(trigger =>
                //   trigger
                //   .StartNow()
                //   .ForJob(new JobKey($"{nameof(ImageEncryptJob)}"))
                //   .WithIdentity($"{nameof(ImageEncryptJob)} Trigger")
                //   .WithSimpleSchedule(s =>
                //   {
                //       s.WithInterval(TimeSpan.FromSeconds(10))
                //       .WithMisfireHandlingInstructionIgnoreMisfires()
                //       .RepeatForever();
                //   }));
                //q.ScheduleJob<ImageDecryptJob>(trigger =>
                //   trigger
                //   .StartNow()
                //   .ForJob(new JobKey($"{nameof(ImageDecryptJob)}"))
                //   .WithIdentity($"{nameof(ImageDecryptJob)} Trigger")
                //   .WithSimpleSchedule(s =>
                //   {
                //       s.WithInterval(TimeSpan.FromSeconds(10))
                //       .WithMisfireHandlingInstructionIgnoreMisfires()
                //       .RepeatForever();
                //   }));

                //目前新增照片時就會縮圖，如果以後有漏在開起來讓他跑
                //q.ScheduleJob<ImageDecryptResizeThunbnailJob>(trigger =>
                //   trigger
                //   .StartNow()
                //   .ForJob(new JobKey($"{nameof(ImageDecryptResizeThunbnailJob)}"))
                //   .WithIdentity($"{nameof(ImageDecryptResizeThunbnailJob)} Trigger")
                //   .WithSimpleSchedule(s =>
                //   {
                //       s.WithInterval(TimeSpan.FromSeconds(10))
                //       .WithMisfireHandlingInstructionIgnoreMisfires()
                //       .RepeatForever();
                //   }));

                //q.ScheduleJob<DeleteUncryptImageJob>(trigger =>
                //   trigger
                //   .StartNow()
                //   .ForJob(new JobKey($"{nameof(DeleteUncryptImageJob)}"))
                //   .WithIdentity($"{nameof(DeleteUncryptImageJob)} Trigger")
                //   .WithSimpleSchedule(s =>
                //   {
                //       s.WithMisfireHandlingInstructionIgnoreMisfires();
                //   }));

            });
            services.AddQuartzHostedService(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });

            return services;
        }
    }
}