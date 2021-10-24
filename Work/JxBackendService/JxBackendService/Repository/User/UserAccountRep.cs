using JxBackendService.Interface.Repository.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System.Linq;

namespace JxBackendService.Repository.User
{
    public class UserAccountRep : BaseDbRepository, IUserAccountRep
    {
        public UserAccountRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public PagedResultModel<UserScoreLog> GetAccountScoreLogs(UserScoreSearchParam searchParam)
        {
            string sql = $@"
DECLARE @TotalCount INT = 0
EXEC {InlodbType.InlodbBak.Value}.dbo.Pro_GetAccountChangesLogs_S
@UserID = @UserID,
@start = @StartDate,
@end = @EndDate,
@typeGroupName = @GroupName,
@typeId = @ItemKey,
@pageSize = @PageSize,
@pageNum = @PageNum,
@TotalCount = @TotalCount OUTPUT

SELECT @TotalCount AS TotalCount

";
            var pagedResultModel = new PagedResultModel<UserScoreLog>()
            {
                PageNo = searchParam.PageNum,
                PageSize = searchParam.PageSize,
            };

            DbHelper.QueryMultiple(sql, searchParam, (reader) =>
            {
                pagedResultModel.ResultList = reader.Read<UserScoreLog>().ToList();
                pagedResultModel.TotalCount = reader.ReadSingle<int>();
            });

            return pagedResultModel;
        }        
    }
}
