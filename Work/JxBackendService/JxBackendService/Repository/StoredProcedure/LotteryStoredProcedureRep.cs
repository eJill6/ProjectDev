using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Base;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Repository.StoredProcedure
{
    public class LotteryStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public LotteryStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.Lottery;

        public override string VIPFlowProductTableName => "VIPFlowProductLotteryLog";

        public override string VIPPointsProductTableName => "VIPPointsProductLotteryLog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_Lottery";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_Lottery";

        protected override string RptProfitLossCurrentTableName => "RPT_ProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_ProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => throw new System.NotImplementedException();

        protected override string SearchUnprocessedMoneyOutInfoSpName => throw new System.NotImplementedException();

        protected override string MoneyInInfoTableName => throw new System.NotImplementedException();

        protected override string MoneyOutInfoTableName => throw new System.NotImplementedException();

        protected override string TransferSuccessSpName => throw new System.NotImplementedException();

        protected override string TransferRollbackSpName => throw new System.NotImplementedException();

        protected override string TransferInSpName => throw new System.NotImplementedException();

        protected override string TransferOutSpName => throw new System.NotImplementedException();

        protected override string AddProfitlossAndPlayInfoSpName => throw new System.NotImplementedException();

        protected override string DwDailyAvailableScoresColumn => throw new System.NotImplementedException();

        protected override string DwDailyFreezeScoresColumn => throw new System.NotImplementedException();

        protected override string PlayInfoTableName => "PalyInfo";

        protected override string ProfitlossTableName => "ProfitLoss";

        protected override List<SqlSelectColumnInfo> PlayInfoSelectColumnInfos
        {
            get
            {
                List<SqlSelectColumnInfo> columns = base.PlayInfoSelectColumnInfos;
                columns.Where(w => w.ColumnName == nameof(TPGamePlayInfoRowModel.BetTime)).Single().ColumnName = "NoteTime";
                columns.Where(w => w.ColumnName == nameof(TPGamePlayInfoRowModel.BetMoney)).Single().ColumnName = "NoteMoney";
                columns.Where(w => w.ColumnName == nameof(TPGamePlayInfoRowModel.ProfitLossTime)).Single().ColumnName = "NoteTime";

                SqlSelectColumnInfo palyInfoIdColumnInfo = columns.Where(w => w.ColumnName == nameof(TPGamePlayInfoRowModel.PalyInfoID)).Single();
                palyInfoIdColumnInfo.ColumnName = "PalyID";
                palyInfoIdColumnInfo.ColumnDbType = System.Data.DbType.Int32;

                return columns;
            }
        }
    }
}
