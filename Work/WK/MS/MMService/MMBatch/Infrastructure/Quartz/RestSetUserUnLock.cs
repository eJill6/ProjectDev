using Microsoft.Extensions.Logging;
using MS.Core.MM.Services.interfaces;
using Quartz;

namespace MMBatch.Infrastructure.Quartz
{
    public class RestSetUserUnLock : BaseJob
    {
        public RestSetUserUnLock(ILogger logger, IUserSummaryService userSummaryService)
        {
            UserSummaryService = userSummaryService;
            Logger = logger;
        }

        protected IUserSummaryService UserSummaryService { get; }

        protected ILogger Logger { get; }

        public override async Task Execute(IJobExecutionContext context)
        {
            Logger.LogInformation("RestSetUserUnLock 派發排程開始");

            await UserSummaryService.RestSetUserUnLock();

            Logger.LogInformation("RestSetUserUnLock 派發排程結束");
        }
    }
}
