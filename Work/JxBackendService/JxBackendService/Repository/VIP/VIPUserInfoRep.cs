using System.Collections.Generic;
using System.Data;
using JxBackendService.Interface.Repository.VIP;
using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam.VIP;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;

namespace JxBackendService.Repository.VIP
{
    public class VIPUserInfoRep : BaseDbRepository<VIPUserInfo>, IVIPUserInfoRep
    {
        public VIPUserInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser,
            dbConnectionType)
        {
        }

        public BaseReturnModel ReceivedPrize(ProVIPPrizesParam proVipPrizesParam)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_VIP_Prizes";
            var returnCode = DbHelper.ExecuteScalar<string>(sql, proVipPrizesParam, CommandType.StoredProcedure);

            return new BaseReturnModel(ReturnCode.GetSingle(returnCode));
        }

        public int? GetUserCurrentLevel(int userId)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, new List<string>() { nameof(VIPUserInfo.CurrentLevel) }) +
                "WHERE UserID = @userId ";

            return DbHelper.ExecuteScalar<int?>(sql, new { userId });
        }


        public List<VIPUserInfo> GetVIPUserInfos(List<int> userIds)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, new List<string>() { nameof(VIPUserInfo.UserID), nameof(VIPUserInfo.CurrentLevel) }) +
                "WHERE UserID IN @userIds ";

            return DbHelper.QueryList<VIPUserInfo>(sql, new { userIds });
        }

        public BaseReturnModel RegisterVIPUser(RegisterVIPUserParam registerVIPUserParam)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_RegisterVIPUser";
            var returnCode = DbHelper.ExecuteScalar<string>(sql, registerVIPUserParam, CommandType.StoredProcedure);

            return new BaseReturnModel(ReturnCode.GetSingle(returnCode));
        }

        public BaseReturnModel CheckQualifiedForUser(int userId, WalletType walletType)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_VIP_CheckQualifiedForUser";
            SPReturnModel returnModel = DbHelper.QuerySingle<SPReturnModel>(
                sql,
                new { 
                    userId, 
                    walletType = walletType.Value,
                    RC_Success = ReturnCode.Success.Value.ToVarchar(6),
                    RC_SearchResultIsEmpty = ReturnCode.SearchResultIsEmpty.Value.ToVarchar(6)
                },
                CommandType.StoredProcedure);

            return new BaseReturnModel(returnModel.ToReturnCodeModel());
        }
    }
}