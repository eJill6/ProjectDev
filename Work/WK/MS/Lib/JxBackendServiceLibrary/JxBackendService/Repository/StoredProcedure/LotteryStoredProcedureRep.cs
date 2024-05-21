using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity.Game.Lottery;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Repository.StoredProcedure
{
    public class LotteryStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        private readonly IProfitLossTypeNameService _profitLossTypeNameService;

        private static readonly Dictionary<string, string> s_playInfoColumnNameMap = new Dictionary<string, string>()
        {
            {nameof(TPGamePlayInfoRowModel.PlayInfoID), "PalyID" },
            {nameof(TPGamePlayInfoRowModel.BetTime), "NoteTime" },
            {nameof(TPGamePlayInfoRowModel.ProfitLossTime), "LotteryTime"},
            {nameof(TPGamePlayInfoRowModel.BetMoney), "NoteMoney"},
            {nameof(TPGamePlayInfoRowModel.SaveTime), "NoteTime"}
        };

        public LotteryStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _profitLossTypeNameService = DependencyUtil.ResolveKeyed<IProfitLossTypeNameService>(SharedAppSettings.PlatformMerchant);
        }

        public override PlatformProduct Product => PlatformProduct.Lottery;

        public override string DWDailyProfitLossTableName => "DW_Daily_ProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => throw new NotImplementedException();

        protected override string SearchUnprocessedMoneyOutInfoSpName => throw new NotImplementedException();

        protected override string MoneyInInfoTableName => throw new NotImplementedException();

        protected override string MoneyOutInfoTableName => throw new NotImplementedException();

        protected override string TransferSuccessSpName => throw new NotImplementedException();

        protected override string TransferRollbackSpName => throw new NotImplementedException();

        protected override string TransferInSpName => throw new NotImplementedException();

        protected override string TransferOutSpName => throw new NotImplementedException();

        protected override string AddProfitlossAndPlayInfoSpName => throw new NotImplementedException();

        public override string PlayInfoTableName => "PalyInfo";

        public override string ProfitlossTableName => "ProfitLoss";

        public override List<SqlSelectColumnInfo> PlayInfoSelectColumnInfos
        {
            get
            {
                List<SqlSelectColumnInfo> columns = base.PlayInfoSelectColumnInfos;

                foreach (SqlSelectColumnInfo column in columns)
                {
                    if (s_playInfoColumnNameMap.TryGetValue(column.ColumnName, out string newColumnName))
                    {
                        column.ColumnName = newColumnName;
                    }
                }

                columns.Single(w => w.AliasName == nameof(TPGamePlayInfoRowModel.PlayInfoID)).ColumnDbType = System.Data.DbType.Int32;

                HashSet<string> palyInfoColumnSet = ModelUtil.GetAllColumnInfos<PalyInfo>().Select(s => s.ColumnName).ToHashSet();
                columns.RemoveAll(r => !palyInfoColumnSet.Contains(r.ColumnName));

                return columns;
            }
        }

        public override List<SqlSelectColumnInfo> ProfitLossSelectColumnInfos
        {
            get
            {
                List<SqlSelectColumnInfo> columns = base.ProfitLossSelectColumnInfos;
                columns.Where(w => w.ColumnName == nameof(TPGameProfitLossRowModel.BetTime)).Single().ColumnName = "NULL";
                columns.Where(w => w.ColumnName == nameof(TPGameProfitLossRowModel.AllBetMoney)).Single().ColumnName = nameof(TPGameProfitLossRowModel.ProfitLossMoney);

                return columns;
            }
        }

        protected override string GetProfitLossTypeFilter(string profitLossType)
        {
            if (profitLossType.IsNullOrEmpty())
            {
                ProfitLossTypeName commissionTypeName = _profitLossTypeNameService.GetSingle(ProfitLossTypeName.Commission.Value);

                if (commissionTypeName != null)
                {
                    return $"AND ProfitLossType <> N'{ProfitLossTypeName.Commission.Value}' ";
                }

                return null;
            }

            ProfitLossTypeName profitLossTypeName = _profitLossTypeNameService.GetSingle(profitLossType);

            if (profitLossTypeName == null)
            {
                return " AND 1 = 2 ";
            }

            if (profitLossTypeName.TableValues.AnyAndNotNull())
            {
                return $" AND ProfitLossType IN ({string.Join(",", profitLossTypeName.TableValues.Select(s => $"N'{s}'")) }) ";
            }

            return base.GetProfitLossTypeFilter(profitLossType);
        }
    }
}