using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.MM;
using JxBackendService.Model.Entity.Chat;
using JxBackendService.Model.Entity.MM;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Base;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Repository.MM
{
    public class MMUserInfoRep : BaseDbRepository<MMUserInfo>, IMMUserInfoRep
    {
        public MMUserInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public List<BasicMMUserInfo> GetBasicMMUserInfos(List<int> userIds)
        {
            List<SqlSelectColumnInfo> selectColumnInfos = ModelUtil.GetAllColumnInfos<BasicMMUserInfo>();
            List<string> selectColumnNames = selectColumnInfos.Select(s => s.ColumnName).ToList();

            string sql = GetAllQuerySQL(InlodbType.Inlodb, selectColumnNames) + @"
                WHERE UserId IN @userIds ";

            return DbHelper.QueryList<BasicMMUserInfo>(sql, new { userIds });
        }
    }
}