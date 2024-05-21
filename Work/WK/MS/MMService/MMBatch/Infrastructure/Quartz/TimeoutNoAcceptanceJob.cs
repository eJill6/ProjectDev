using Microsoft.Extensions.Logging;
using MS.Core.MM.Services.interfaces;
using Quartz;

namespace MMBatch.Infrastructure.Quartz
{
    public class TimeoutNoAcceptanceJob : BaseJob
    {
        public TimeoutNoAcceptanceJob(ILogger logger, IBookingService bookingService)
        {
            BookingService = bookingService;
            Logger = logger;
        }

        protected IBookingService BookingService { get; }

        protected ILogger Logger { get; }

        public override async Task Execute(IJobExecutionContext context)
        {
            Logger.LogInformation($"{nameof(IBookingService.TimeoutNoAcceptance)} 派發排程開始");

            await BookingService.TimeoutNoAcceptance();

            Logger.LogInformation($"{nameof(IBookingService.TimeoutNoAcceptance)} 派發排程結束");
        }
    }
}
