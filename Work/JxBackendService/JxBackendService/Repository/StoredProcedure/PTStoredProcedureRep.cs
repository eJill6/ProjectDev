using JxBackendService.Common.Util;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Repository.StoredProcedure
{
    public class PTStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public PTStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.PT;

        public override string VIPFlowProductTableName => "VIPFlowProductPTLog";

        public override string VIPPointsProductTableName => "VIPPointsProductPTLog";

        #region virtual
        protected override List<SqlSelectColumnInfo> PlayInfoSelectColumnInfos
        {
            get
            {
                List<SqlSelectColumnInfo> columns = base.PlayInfoSelectColumnInfos;
                columns.Where(w => w.ColumnName == nameof(TPGamePlayInfoRowModel.BetTime)).Single().ColumnName = "NoteTime";
                columns.Where(w => w.ColumnName == nameof(TPGamePlayInfoRowModel.BetMoney)).Single().ColumnName = "NoteMoney";
                columns.Where(w => w.ColumnName == nameof(TPGamePlayInfoRowModel.ProfitLossTime)).Single().ColumnName = "SaveTime";

                SqlSelectColumnInfo palyInfoIdColumnInfo = columns.Where(w => w.ColumnName == nameof(TPGamePlayInfoRowModel.PalyInfoID)).Single();
                palyInfoIdColumnInfo.ColumnName = "ID";
                palyInfoIdColumnInfo.ColumnDbType = System.Data.DbType.Int32;

                columns.Add(new SqlSelectColumnInfo(nameof(TPGamePlayInfoRowModel.IsFactionAward)));

                return columns;
            }
        }

        protected override List<SqlSelectColumnInfo> ProfitLossSelectColumnInfos
        {
            get
            {
                List<SqlSelectColumnInfo> columns = base.ProfitLossSelectColumnInfos;
                columns.Where(w => w.ColumnName == nameof(TPGamePlayInfoRowModel.BetTime)).Single().ColumnName = "NoteTime";                
                return columns;
            }
        }

        protected override Tuple<string, int?> GetIsWinFilter(List<int> isWins)
        {
            string filter = string.Empty;
            int? isFactionAward = null;
            int? isWin = isWins.Where(w => w != PTBetResultType.Draw.Value).FirstOrDefault();

            if (!isWin.HasValue)
            {
                return new Tuple<string, int?>(null, null);
            }

            PTBetResultType ptBetResultType = PTBetResultType.GetSingle(isWin.Value);

            if (ptBetResultType.Value == PTBetResultType.NoFactionAward.Value)
            {
                filter = "AND IsFactionAward = 0 ";
                isFactionAward = 0;
            }
            else if (ptBetResultType.Value == PTBetResultType.Win.Value)
            {
                filter = "AND IsFactionAward = 1 AND IsWin = 1 ";
                isFactionAward = 1;
            }
            else if (ptBetResultType.Value == PTBetResultType.Lose.Value)
            {
                filter = "AND IsFactionAward = 1 AND IsWin = 0 ";
                isFactionAward = 1;
            }
            else if (ptBetResultType.Value == PTBetResultType.Cancel.Value)
            {
                filter = "AND IsFactionAward = 3 ";
                isFactionAward = 3;
            }

            return new Tuple<string, int?>(filter, isFactionAward);
        }

        protected override string ProfitLossCompareTimeColumnName => "NoteTime";

        protected override TPGameProfitLossRowModelCompareTime CompareTimeProperty => TPGameProfitLossRowModelCompareTime.BetTime;
        #endregion

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_PT";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_PT";

        protected override string RptProfitLossCurrentTableName => "RPT_PtProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_PtProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchPtMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchPtMoneyOutInfo";

        protected override string MoneyInInfoTableName => "PtMoneyInInfo";

        protected override string MoneyOutInfoTableName => "PtMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_PtTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_PtTransferRollback";

        protected override string TransferInSpName => "Pro_PtTransferIn";

        protected override string TransferOutSpName => "Pro_PtTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => throw new System.NotImplementedException();

        protected override string DwDailyAvailableScoresColumn => "PTAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "PTFreezeScores";

        protected override string PlayInfoTableName => "PtPlayInfo_HS";

        protected override string ProfitlossTableName => "PtProfitLoss_HS";        
    }
}
