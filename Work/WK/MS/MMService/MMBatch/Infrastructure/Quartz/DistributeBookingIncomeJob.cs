using Microsoft.Extensions.Logging;
using MS.Core.MM.Services.interfaces;
using Quartz;

namespace MMBatch.Infrastructure.Quartz
{
    public class DistributeBookingIncomeJob : BaseJob
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="incomeExpenseService"></param>
        /// <param name="logger"></param>
        public DistributeBookingIncomeJob(IBookingService bookingService, ILogger logger)
        {
            BookingService = bookingService;
            Logger = logger;
        }

        protected IBookingService BookingService { get; }
        protected ILogger Logger { get; }
        public override async Task Execute(IJobExecutionContext context)
        {
            Logger.LogInformation($"{nameof(DistributeBookingIncomeJob)} 派發排程開始");

            await BookingService.Distribute(20);

            Logger.LogInformation($"{nameof(DistributeBookingIncomeJob)} 派發排程結束");
        }
    }
}
