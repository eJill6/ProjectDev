using Microsoft.Extensions.Logging;
using MS.Core.MM.Services.interfaces;
using Quartz;

namespace MMBatch.Infrastructure.Quartz
{
    public class AuditAbnormalOrderJob : BaseJob
    {
        public AuditAbnormalOrderJob(ILogger logger, IIncomeExpenseService incomeExpenseService)
        {
			IncomeExpenseService = incomeExpenseService;
            Logger = logger;
        }

        protected IIncomeExpenseService IncomeExpenseService { get; }

        protected ILogger Logger { get; }

        public override async Task Execute(IJobExecutionContext context)
        {
            Logger.LogInformation("AuditAbnormalOrder(检查无身份无会员卡异常收益单) 排程開始");

            await IncomeExpenseService.AuditAbnormalOrder();

            Logger.LogInformation("AuditAbnormalOrder(检查无身份无会员卡异常收益单) 排程結束");
        }
    }
}
