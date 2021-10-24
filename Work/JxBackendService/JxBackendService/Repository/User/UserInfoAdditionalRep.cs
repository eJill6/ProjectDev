using JxBackendService.Interface.Repository.User;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System.Data;

namespace JxBackendService.Repository.User
{
    public class UserInfoAdditionalRep : BaseDbRepository<UserInfoAdditional>, IUserInfoAdditionalRep
    {
        public UserInfoAdditionalRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public SPReturnModel SaveUserTransferChildStatus(int userId, string loginUserName, bool isLowMoneyIn, bool? isAllowSetTransferByParent)
        {
            var param = new ProSaveUserTransferChildStatusParam()
            {
                UserID = userId,
                LoginUserName = loginUserName,
                IsLowMoneyIn = isLowMoneyIn,
                IsAllowSetTransferByParent = isAllowSetTransferByParent,
                RT_Success = ReturnCode.Success.Value,
                RT_UpdateFail = ReturnCode.UpdateFailed.Value
            };

            string sql = $"{InlodbType.Inlodb}.dbo.Pro_SaveUserTransferChildStatus";
            return DbHelper.QuerySingle<SPReturnModel>(sql, param, CommandType.StoredProcedure);
        }

        
    }
}
