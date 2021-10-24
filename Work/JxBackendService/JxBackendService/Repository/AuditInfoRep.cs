using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Audit;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using JxBackendService.Resource.Element;
using System.Collections.Generic;

namespace JxBackendService.Repository
{
    public class AuditInfoRep : BaseDbRepository<AuditInfo>, IAuditInfoRep
    {
        private readonly IAppSettingService _appSettingService;

        public AuditInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) 
        {
             _appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(envLoginUser.Application, SharedAppSettings.PlatformMerchant);
        }

        private string QueryString(AuditInfoQueryParam param)
        {
            string sql = string.Empty;
            if (!string.IsNullOrWhiteSpace(param.UserName))
            {
                sql += $" AND {nameof(param.UserName)} = @{nameof(param.UserName)}";
            }
            if (param.AuditType.HasValue && param.AuditType != -1)
            {
                sql += $" AND {nameof(param.AuditType)} = @{nameof(param.AuditType)}";
            }
            if (param.AuditStatus.HasValue && param.AuditStatus != -1)
            {
                sql += $" AND {nameof(param.AuditStatus)} = @{nameof(param.AuditStatus)}";
            }
            if (!string.IsNullOrWhiteSpace(param.CreateUser))
            {
                sql += $" AND {nameof(param.CreateUser)} = @{nameof(param.CreateUser)}";
            }
            return sql;
        }

        public bool CheckUnProcessAuditInfo(int auditType, string refID)
        {
            //檢查audittype 未處理的，是否有同單號的
            string sql = $" SELECT COUNT(1) " +
                         $" {GetFromTableSQL(InlodbType.Inlodb, nameof(AuditInfo))} " +
                         $" WHERE AuditType = @auditType AND AuditStatus = {AuditStatusType.Unprocessed.Value}" +
                         $" AND RefID = @RefID ";

            return DbHelper.QuerySingle<int>(sql, new
            {
                auditType,
                refID
            }) > 0;
        }

        public bool CheckUnProcessAuditInfoIsExistData(int auditType, int userId, string checkAuditValue)
        {
            //檢查audittype 未處理的，是否有同審核內容的
            string sql = $@"SELECT COUNT(1) 
                            {GetFromTableSQL(InlodbType.Inlodb, nameof(AuditInfo))}
                            WHERE AuditType = @auditType AND AuditStatus = {AuditStatusType.Unprocessed.Value} ";

            if (auditType == AuditTypeValue.GivePrize || auditType == AuditTypeValue.UnbindUserAuthenticator)
            {
                sql += " AND UserID = @UserID ";
            }
            else
            {
                sql += " AND AddtionalAuditValue = @checkAuditValue ";
            }

            return DbHelper.ExecuteScalar<int>(sql, new
            {
                auditType,
                userId,
                checkAuditValue = checkAuditValue.ToNVarchar(100)
            }) > 0;
        }

        public PagedResultModel<AuditInfo> GetList(AuditInfoQueryParam param, BasePagingRequestParam pageParam)
        {
            string whereSql = " WHERE 1 = 1 " + QueryString(param);
            pageParam.SortModels = new List<SortModel>() { new SortModel() { ColumnName = nameof(AuditInfo.UpdateDate), Sort = System.Data.SqlClient.SortOrder.Descending } };
            PagedSqlQueryParamsModel pagedParam = CreateAllColumnsPagedSqlQueryParams(whereSql, param, pageParam);
            if (pageParam.PageNo == 0)
            {
                pageParam.PageNo = 1;
            }
            pagedParam.SetPager(pageParam);
            return DbHelper.PagedSqlQuery<AuditInfo>(pagedParam);
        }

        public BaseReturnModel Deal(AuditInfoDealParam param)
        {
            var returnModel = DbHelper.QueryFirst<SPReturnModel>("Pro_DealWithAuditInfo", param, System.Data.CommandType.StoredProcedure);

            if (returnModel.ReturnCode == ReturnCode.SystemError.Value)
            {
                return new BaseReturnModel(returnModel.ReturnMsg);
            }
            else if (returnModel.ReturnCode == ReturnCode.Success)
            {
                if (!string.IsNullOrEmpty(returnModel.ReturnMsg))
                {
                    return new BaseReturnModel(new SuccessMessage(returnModel.GetHandledMsg(_appSettingService.CommonDataHash)));
                }

                return new BaseReturnModel(new SuccessMessage(AuditElement.AuditDone));
                //return new BaseReturnModel(new SuccessMessage(string.Format("{0}\n{1}", AuditElement.AuditDone, returnModel.GetHandledMsg(_appSettingService.CommonDataHash))));
                //return new BaseReturnModel(new SuccessMessage(string.Join("\n", AuditElement.AuditDone, returnModel.GetHandledMsg(_appSettingService.CommonDataHash))));
            }
            else
            {
                return new BaseReturnModel(ReturnCode.GetSingle(returnModel.ReturnCode));
            }
        }
    }
}