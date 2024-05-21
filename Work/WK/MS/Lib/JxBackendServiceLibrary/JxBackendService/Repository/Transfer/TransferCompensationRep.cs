using System.Collections.Generic;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;

namespace JxBackendService.Repository.Transfer
{
    public class TransferCompensationRep : BaseDbRepository<TransferCompensation>, ITransferCompensationRep
    {
        public TransferCompensationRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public bool HasUnProcessedCompensation(SearchUserCompensationParam searchParam)
        {
            string sql = $@"
                SELECT COUNT(0)
                {GetFromTableSQL(InlodbType.Inlodb)}
                WHERE
                    UserID = @UserID AND
                    ProductCode = @ProductCode AND
                    [Type] = @Type AND
                    IsProcessed = 0 AND
                    CreateDate >= @StartDate AND
                    CreateDate < @EndDate ";

            return DbHelper.ExecuteScalar<int>(sql,
                new
                {
                    searchParam.UserID,
                    ProductCode = searchParam.ProductCode.ToVarchar(10),
                    Type = searchParam.Type.ToVarchar(30),
                    searchParam.StartDate,
                    searchParam.EndDate
                }) > 0;
        }

        public List<TransferCompensation> GetUserUnProcessedCompensations(SearchUserCompensationParam searchParam)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) +
                $@"
                WHERE
                    UserID = @UserID AND
                    ProductCode = @ProductCode AND
                    [Type] = @Type AND
                    IsProcessed = 0 AND
                    CreateDate >= @StartDate AND
                    CreateDate < @EndDate ";

            return DbHelper.QueryList<TransferCompensation>(sql,
                new
                {
                    searchParam.UserID,
                    ProductCode = searchParam.ProductCode.ToVarchar(10),
                    Type = searchParam.Type.ToVarchar(30),
                    searchParam.StartDate,
                    searchParam.EndDate
                });
        }

        public List<TransferCompensation> GetUnProcessedCompensations(SearchProductCompensationParam searchParam)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) +
                $@"
                WHERE
                    ProductCode = @ProductCode AND
                    [Type] = @Type AND
                    IsProcessed = 0 AND
                    CreateDate >= @StartDate AND
                    CreateDate < @EndDate ";

            return DbHelper.QueryList<TransferCompensation>(sql,
                new
                {
                    ProductCode = searchParam.ProductCode.ToVarchar(10),
                    Type = searchParam.Type.ToVarchar(30),
                    searchParam.StartDate,
                    searchParam.EndDate
                });
        }
    }
}