using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.BotBet;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.BotBet;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.BackSideUser
{
    public class BotParameterRep : BaseDbRepository<SettingInfoContext>, IBotParameterRep
    {
        public BotParameterRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public bool CreateAnchorInfoContext(AnchorInfoContext param)
        {
            string sql = $@"
            INSERT INTO {InlodbType.Inlodb}.[dbo].{nameof(AnchorInfoContext)} ([Id],[GroupId])
            VALUES (@Id,@GroupId)
            ";
            return DbHelper.Execute(sql, new { Id = param.Id, GroupId = param.GroupId }) > 0;
        }

        public bool CreateSettingInfoContext(BotParameterInput param)
        {
            string table = GetBotTableName(Convert.ToInt16(param.LotteryPatchType));
            string sql = $@"
            INSERT INTO {InlodbType.Inlodb}.[dbo].{table} 
                ([SettingGroupId]
                ,[GroupId]
                ,[TimeType]
                ,[Amount]
                ,[Rate])
            VALUES            
                (@SettingGroupId
                ,@GroupId
                ,@TimeType
                ,@Amount
                ,@Rate)
            ";
            return DbHelper.Execute(sql, new { SettingGroupId = param.SettingGroupId, GroupId = param.GroupId, TimeType = param.TimeType, Amount = param.Amount, Rate = param.Rate }) > 0;
        }

        public bool DeleteAnchorInfoContext(string keyContent)
        {
            string sql = $@"
            DELETE {InlodbType.Inlodb}.[dbo].{nameof(AnchorInfoContext)}
            WHERE Id=@Id";
            return DbHelper.Execute(sql, new { Id = keyContent }) > 0;
        }

        public bool DeleteSettingInfoContext(string keyContent)
        {
            string[] parmms = keyContent.Split(',');
            string id = parmms[0];
            string lotteryPatchType = parmms[1];
            string table = GetBotTableName(Convert.ToInt16(lotteryPatchType));
            string sql = $@"
            DELETE {InlodbType.Inlodb}.[dbo].{table}
            WHERE Id=@Id";
            return DbHelper.Execute(sql, new { Id = id }) > 0;
        }

        public PagedResultModel<AnchorInfoContext> GetAnchorInfoContext(AnchorInfoParam param)
        {
            StringBuilder whereString = new StringBuilder(" WHERE 1 = 1 ");
            if (!string.IsNullOrEmpty(param.Id))
            {
                whereString.AppendLine($" AND Id = @Id");
            }
            if (param.GroupId.HasValue)
            {
                whereString.AppendLine($" AND GroupId = @GroupId");
            }
            PagedSqlQueryParamsModel result = CreateAllColumnsPagedSqlQueryParams(new BuildPagedSqlQueryParam
            {
                InlodbType = InlodbType.Inlodb,
                TableName = nameof(AnchorInfoContext),
                WhereString = whereString.ToString(),
                Parameters = new
                {
                    Id = param.Id,
                    GroupId = param.GroupId,
                },
                RequestParam = new BasePagingRequestParam
                {
                    PageNo = param.PageNo,
                    PageSize = param.PageSize,
                    SortModels = new List<SortModel>
                    {
                        new SortModel { ColumnName = nameof(AnchorInfoContext.Id)},
                        new SortModel { ColumnName = nameof(AnchorInfoContext.GroupId)}
                    }
                }
            });
            List<string> selectColumns = ModelUtil.GetAllColumnInfos<AnchorInfoContext>().Select(s => s.ColumnName).ToList();
            string allSelectColumns = string.Join(",\n", selectColumns);
            result.SelectColumns = allSelectColumns;
            return DbHelper.PagedSqlQuery<AnchorInfoContext>(result);
        }

        public AnchorInfoContext GetAnchorInfoContextDetail(long id)
        {
            List<string> selectColumns = ModelUtil.GetAllColumnInfos<AnchorInfoContext>().Select(s => s.ColumnName).ToList();
            string allSelectColumns = string.Join(",\n", selectColumns);
            string sql = $@"
             SELECT {allSelectColumns} 
             FROM {InlodbType.Inlodb}.[dbo].{nameof(AnchorInfoContext)} 
             WHERE Id=@Id ";
            return DbHelper.QuerySingle<AnchorInfoContext>(sql, new { Id = id });
        }
        public bool IsExistAnchorInfoContext(long id)
        {
            string sql = $@"
             SELECT COUNT(1)
             FROM {InlodbType.Inlodb}.[dbo].{nameof(AnchorInfoContext)} 
             WHERE Id=@Id ";
            return DbHelper.ExecuteScalar<int>(sql, new { Id = id }) > 0;
        }

        public PagedResultModel<SettingInfoContext> GetSettingInfoContext(BotBetParam param)
        {
            StringBuilder whereString = new StringBuilder(" WHERE 1 = 1 ");
            if (param.BotGroup.HasValue)
            {
                whereString.AppendLine($" AND GroupId = @GroupId");
            }
            if (param.TimeType.HasValue)
            {
                whereString.AppendLine($" AND TimeType = @TimeType");
            }
            if (param.SettingGroup.HasValue)
            {
                whereString.AppendLine($" AND SettingGroupId = @SettingGroupId");
            }
            string table = GetBotTableName(param.LotteryPatchType);
            PagedSqlQueryParamsModel result = CreateAllColumnsPagedSqlQueryParams(new BuildPagedSqlQueryParam
            {
                InlodbType = InlodbType.Inlodb,
                TableName = table,
                WhereString = whereString.ToString(),
                Parameters = new
                {
                    GroupId = param.BotGroup,
                    TimeType = param.TimeType,
                    SettingGroupId = param.SettingGroup
                },
                RequestParam = new BasePagingRequestParam
                {
                    PageNo = param.PageNo,
                    PageSize = param.PageSize,
                    SortModels = new List<SortModel>
                    {
                        new SortModel { ColumnName = nameof(SettingInfoContext.GroupId)},
                        new SortModel { ColumnName = nameof(SettingInfoContext.TimeType)},
                        new SortModel { ColumnName = nameof(SettingInfoContext.SettingGroupId)},
                        new SortModel { ColumnName = nameof(SettingInfoContext.Amount)}
                    }
                }
            });
            return DbHelper.PagedSqlQuery<SettingInfoContext>(result);
        }

        public SettingInfoContext GetSettingInfoContextDetail(int id, int type)
        {
            List<string> selectColumns = ModelUtil.GetAllColumnInfos<SettingInfoContext>().Select(s => s.ColumnName).ToList();
            string allSelectColumns = string.Join(",\n", selectColumns);
            string table = GetBotTableName(type);
            string sql = $@"
             SELECT {allSelectColumns} 
             FROM {InlodbType.Inlodb}.[dbo].{table} 
             WHERE Id=@Id ";
            return DbHelper.QuerySingle<SettingInfoContext>(sql, new { Id = id });
        }

        public bool UpdateAnchorInfoContext(AnchorInfoContext param)
        {
            string sql = $@"
            UPDATE {InlodbType.Inlodb}.[dbo].{nameof(AnchorInfoContext)}
            SET Id = @Id,GroupId=@GroupId
            WHERE Id = @Id";
            var result = DbHelper.Execute(sql, new { Id = param.Id, GroupId = param.GroupId });
            return result > 0;
        }

        public bool UpdateSettingInfoContext(BotParameterInput param)
        {
            //判斷要跨表先新增到另一張表，在把原本的刪除
            if (param.OriginalLotteryPatchType != param.LotteryPatchType)
            {
                bool IsCreateSuccess = CreateSettingInfoContext(param);
                if (!IsCreateSuccess)
                {
                    return false;
                }
                bool IsDeleteSuccess = DeleteSettingInfoContext($"{param.Id},{param.OriginalLotteryPatchType}");
                if (!IsDeleteSuccess)
                {
                    return false;
                }
                return true;
            }
            else
            {
                string table = GetBotTableName(param.LotteryPatchType);
                string sql = $@"
                UPDATE {InlodbType.Inlodb}.[dbo].{table}
                SET SettingGroupId=@SettingGroupId,GroupId=@GroupId,TimeType=@TimeType,Amount=@Amount,Rate=@Rate
                WHERE Id=@Id";
                var result = DbHelper.Execute(sql, new { SettingGroupId = param.SettingGroupId, GroupId = param.GroupId, TimeType = param.TimeType, Amount = param.Amount, Rate = param.Rate, Id = param.Id });
                return result > 0;
            }
        }
        public bool IsLast(BotParameterInput param)
        {
            var detail = GetSettingInfoContextDetail(param.Id, param.OriginalLotteryPatchType);           
            int reuslt = GetSettingInfoContext(new BotBetParam { LotteryPatchType= param.OriginalLotteryPatchType ,SettingGroup=(SettingGroup)detail.SettingGroupId,BotGroup=(BotGroup)detail.GroupId,TimeType=detail.TimeType}).TotalCount;
            return reuslt<=1;
        }

        private string GetBotTableName(int type)
        {
            string table = string.Empty;
            if (type == (int)LotteryPatchType.JS)
                table = "JsSettingInfoContext";
            else
                table = "SettingInfoContext";
            return table;
        }
    }
}
