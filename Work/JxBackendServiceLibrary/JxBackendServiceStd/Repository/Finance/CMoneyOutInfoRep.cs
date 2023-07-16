using JxBackendService.Interface.Repository.Finance;
using JxBackendService.Model.Entity.Finance;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Finance;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam.Finance;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System.Data;

namespace JxBackendService.Repository.Finance
{
    public class CMoneyOutInfoRep : BaseCMoneyInfoRep<CMoneyOutInfo>, ICMoneyOutInfoRep
    {
        protected override string SequenceName => "SEQ_CMoneyOutInfo_MoneyOutID";

        protected override int ProcessingStatusValue => MoneyOutDealType.Processing.Value;

        public CMoneyOutInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public BaseReturnModel CreateCMoneyOutInfo(ProCreateCMoneyOutInfoParam param)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_CreateCMoneyOutInfo";

            return DbHelper.QuerySingle<SPReturnModel>(sql, param, CommandType.StoredProcedure).ToBaseReturnModel();
        }

        public BaseReturnModel ProcessCMoneyOut(ProProcessCMoneyOutParam param)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_ProcessCMoneyOut";

            return DbHelper.QuerySingle<SPReturnModel>(sql, param, CommandType.StoredProcedure).ToBaseReturnModel();
        }
    }
}