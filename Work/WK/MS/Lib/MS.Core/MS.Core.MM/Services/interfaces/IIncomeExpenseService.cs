using MS.Core.Models;

namespace MS.Core.MM.Services.interfaces
{
    public interface IIncomeExpenseService
    {
        Task<BaseReturnModel> DistributeAmount(DateTime time);

        Task<BaseReturnModel> AuditAbnormalOrder();
	}
}