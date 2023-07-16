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
	public class CMoneyInInfoRep : BaseCMoneyInfoRep<CMoneyInInfo>, ICMoneyInInfoRep
	{
		protected override string SequenceName => "SEQ_CMoneyInInfo_MoneyInID";

		protected override int ProcessingStatusValue => MoneyInDealType.Processing.Value;

		public CMoneyInInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
		{
		}

		public BaseReturnModel ProcessCMoneyIn(ProProcessCMoneyInParam param)
		{
			string sql = $"{InlodbType.Inlodb}.dbo.Pro_ProcessCMoneyIn";

			return DbHelper.QuerySingle<SPReturnModel>(sql, param, CommandType.StoredProcedure).ToBaseReturnModel();
		}
	}
}