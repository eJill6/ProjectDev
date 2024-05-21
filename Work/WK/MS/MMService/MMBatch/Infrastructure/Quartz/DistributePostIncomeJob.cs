using Microsoft.Extensions.Logging;
using MS.Core.Extensions;
using MS.Core.MM.Services.interfaces;
using Quartz;

namespace MMBatch.Infrastructure.Quartz
{
    /// <summary>
    /// 贴子收益派發
    /// </summary>
    public class DistributePostIncomeJob : BaseJob
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="incomeExpenseService"></param>
        /// <param name="logger"></param>
        public DistributePostIncomeJob(IIncomeExpenseService incomeExpenseService, ILogger logger)
        {
            IncomeExpenseService = incomeExpenseService;
            Logger = logger;
        }

        private IIncomeExpenseService IncomeExpenseService { get; }
        private ILogger Logger { get; }

        /// <summary>
        /// 贴子收益派發
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task Execute(IJobExecutionContext context)
        {
            Logger.LogInformation("DistributePostIncomeJob 派發排程開始");

            await IncomeExpenseService.DistributeAmount(DateTimeExtension.GetCurrentTime());

            Logger.LogInformation("DistributePostIncomeJob 派發排程結束");
        }
    }
}
