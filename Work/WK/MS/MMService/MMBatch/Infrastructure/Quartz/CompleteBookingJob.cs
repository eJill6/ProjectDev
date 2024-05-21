using Microsoft.Extensions.Logging;
using MS.Core.MM.Services.interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMBatch.Infrastructure.Quartz
{
    /// <summary>
    /// 
    /// </summary>
    public class CompleteBookingJob : BaseJob
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingService"></param>
        /// <param name="logger"></param>
        public CompleteBookingJob(IBookingService bookingService, ILogger logger)
        {
            BookingService = bookingService;
            Logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        protected IBookingService BookingService { get; }
        /// <summary>
        /// 
        /// </summary>
        protected ILogger Logger { get; }
        /// <summary>
        /// 48小时候将服务中的状态设为已完成
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task Execute(IJobExecutionContext context)
        {
            Logger.LogInformation($"{nameof(DistributeBookingIncomeJob)} 48小时设为完成--排程開始");

            await BookingService.SetBookingCompleted(20);

            Logger.LogInformation($"{nameof(DistributeBookingIncomeJob)} 48小时设为完成--排程結束");
        }
    }
}
