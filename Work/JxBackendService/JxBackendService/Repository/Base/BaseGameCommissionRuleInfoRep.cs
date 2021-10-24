using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Repository.Base
{
    public abstract class BaseGameCommissionRuleInfoRep : BaseDbRepository, IGameCommissionRuleInfoRep
    {
        public abstract CommissionGroupType CommissionGroupType { get; }

        public BaseGameCommissionRuleInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        #region abstract
        protected abstract string RuleInfoTableName { get; }

        protected abstract string SaveRuleInfoSpName { get; }
        #endregion

        public List<GameCommissionRuleInfo> GetGameCommissionRuleInfos(int userId)
        {
            List<SqlSelectColumnInfo> columnInfos = ModelUtil.GetAllColumnInfos<GameCommissionRuleInfo>();

            string sql = $"SELECT {string.Join(",", columnInfos.Select(s => s.ColumnName))} FROM {InlodbType.Inlodb}.dbo.{RuleInfoTableName} WITH(NOLOCK) " +
                $"WHERE UserID = @UserID ";

            return DbHelper.QueryList<GameCommissionRuleInfo>(sql, new { userId });
        }

        /// <summary>
        /// 查出第一層下級所有分紅設置
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<GameCommissionRuleInfo> GetGameCommissionRuleInfosByParentId(int parentId)
        {
            List<SqlSelectColumnInfo> columnInfos = ModelUtil.GetAllColumnInfos<GameCommissionRuleInfo>();

            string sql = $"SELECT {string.Join(",", columnInfos.Select(s => s.ColumnName))} FROM {InlodbType.Inlodb}.dbo.{RuleInfoTableName} WITH(NOLOCK) " +
                $"WHERE UserID IN (SELECT UserID FROM UserInfo WITH(NOLOCK) WHERE ParentID = @parentId) ";

            return DbHelper.QueryList<GameCommissionRuleInfo>(sql, new { parentId });
        }

        public BaseReturnModel SaveRuleInfo(List<SaveCommissionRuleInfo> saveCommissionRuleInfos)
        {
            List<CommissionAccRuleTable> commissionAccRules = saveCommissionRuleInfos.Select(s => new CommissionAccRuleTable()
            {
                UserID = s.UserID,
                UserName = s.UserName,
                MinProfitLossRange = s.MinProfitLossRange,
                MaxProfitLossRange = s.MaxProfitLossRange,
                Visible = Convert.ToByte(s.Visible),
                CommissionPercent = s.CommissionPercent,
            }).ToList();

            string sql = $"{InlodbType.Inlodb}.dbo.{SaveRuleInfoSpName}";
            var dynamicParameters = new DynamicParameters();
            DataTable commissionAccRuleTable = ToCommissionAccRuleTable(commissionAccRules);
            dynamicParameters.Add("commissionAccRuleTable", commissionAccRuleTable.AsTableValuedParameter("dbo.CommissionAccRuleTable"));

            string errorMsg = DbHelper.ExecuteScalar<string>(sql,
                dynamicParameters,
                CommandType.StoredProcedure);

            return new BaseReturnModel(errorMsg);
        }

        private DataTable ToCommissionAccRuleTable(List<CommissionAccRuleTable> commissionAccRules)
        {
            var commissionAccRuleTable = new DataTable();
            commissionAccRuleTable.Columns.Add("RowID", typeof(int));
            commissionAccRuleTable.Columns.Add("Type", typeof(byte));
            commissionAccRuleTable.Columns.Add("UserID", typeof(int));
            commissionAccRuleTable.Columns.Add("UserName", typeof(string));
            commissionAccRuleTable.Columns.Add("MinProfitLossRange", typeof(decimal));
            commissionAccRuleTable.Columns.Add("MaxProfitLossRange", typeof(decimal));
            commissionAccRuleTable.Columns.Add("Visible", typeof(byte));
            commissionAccRuleTable.Columns.Add("CommissionPercent", typeof(double));

            for (int i = 0; i < commissionAccRules.Count; i++)
            {
                CommissionAccRuleTable commissionAccRule = commissionAccRules[i];
                DataRow dataRow = commissionAccRuleTable.NewRow();
                dataRow["RowID"] = i;
                dataRow["Type"] = 0;
                dataRow["UserID"] = commissionAccRule.UserID;
                dataRow["UserName"] = commissionAccRule.UserName;
                dataRow["MinProfitLossRange"] = commissionAccRule.MinProfitLossRange;
                dataRow["MaxProfitLossRange"] = commissionAccRule.MaxProfitLossRange;
                dataRow["Visible"] = commissionAccRule.Visible;
                dataRow["CommissionPercent"] = commissionAccRule.CommissionPercent;
                commissionAccRuleTable.Rows.Add(dataRow);
            }

            return commissionAccRuleTable;
        }

        public string GetRuleInfoTableName() => RuleInfoTableName;
    }
}