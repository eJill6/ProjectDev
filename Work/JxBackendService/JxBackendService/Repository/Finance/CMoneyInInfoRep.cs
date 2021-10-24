using System;
using JxBackendService.Interface.Repository.Finance;
using JxBackendService.Model.Entity.Finance;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Finance;
using JxBackendService.Model.Param.Finance;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Finance
{
    public class CMoneyInInfoRep : BaseDbRepository<CMoneyInInfo>, ICMoneyInInfoRep
    {
        public CMoneyInInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        public DespositInfo GetDepositDoneOrderInfo(int userId, DateTime startDate, DateTime endDate)
        {
            string sql = $@"
SELECT TOP 1 MoneyInID, OrderID, Amount
{GetFromTableSQL(InlodbType.Inlodb)} 
WHERE UserID = @userId AND 
      IsDeal = @IsDeal_Done AND
      OrderTime >= @startDate AND
      OrderTime < @endDate AND
      Amount > 0       
ORDER BY MoneyInID DESC ";

            return DbHelper.QueryFirstOrDefault<DespositInfo>(sql, new
            {
                userId,
                IsDeal_Done = (int)MoneyInDealTypes.Done,
                startDate,
                endDate
            });
        }
    }
}
