using JxBackendService.Interface.Repository.Base;
using JxBackendService.Interface.Repository.Finance;
using JxBackendService.Model.Entity.Finance;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Finance;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam.Finance;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System.Collections.Generic;
using System.Data;

namespace JxBackendService.Repository.Base
{
    public abstract class BaseCMoneyInfoRep<T> : BaseDbRepository<T>, IBaseCMoneyInfoRep<T> where T : class
    {
        private static readonly int _minRecheckOrderMinutes = -1;

        protected abstract string SequenceName { get; }

        protected abstract int ProcessingStatusValue { get; }

        public BaseCMoneyInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public string CreateMoneyID() => GetSequenceIdentity(SequenceName);

        public List<T> GetProcessingOrders3DaysAgo()
        {
            string sql = $@"
                -- 撈取設定分鐘前為處理中的訂單
                DECLARE @ProcessingDate DATETIME = DATEADD(MINUTE, {_minRecheckOrderMinutes}, GETDATE())
                DECLARE	@StartDate DATETIME = DATEADD(DAY, -3, @ProcessingDate),
		                @EndDate DATETIME =  @ProcessingDate

                {GetAllQuerySQL(InlodbType.Inlodb)}
                WHERE
                    DealType = @ProcessingStatus AND
                    OrderTime >= @StartDate AND
                    OrderTime <= @EndDate ";

            return DbHelper.QueryList<T>(sql, new
            {
                ProcessingStatus = ProcessingStatusValue
            });
        }
    }
}