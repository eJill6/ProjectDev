using JxBackendService.Model.Entity;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository
{
    public interface IUserInfoRep : IBaseDbRepository<UserInfo>
    {
        /// <summary>
        /// 取得第一層下級
        /// </summary> 
        List<int> GetAllFirstChild(int parentId);
        int? GetBackSideUserId(string userName);
        string GetBackSideUserName(int userId);
        int? GetFrontSideUserId(string userName);
        string GetFrontSideUserName(int userId);
        List<UserBankInfo> GetUserBankInfos(int userId);
        string GetUserBindPhoneNumber(int userId);
        string GetUserEmail(int userId);
        UserInfo GetUserInfo(string userName);
        string GetUserName(int userId);
        List<string> GetUserNames(List<int> userIds);
        bool IsHasBindPhoneNumber(string encryptPhoneNumber);
        bool IsHasEmail(string encryptEmail);
        bool IsInitializeUser(int userId);
        BaseReturnDataModel<string> UpdateUserInfoData(SpUpdateUserInfoDataParam spUpdateUserInfo);

        List<UserInfo> GetAllFirstChildUserInfo(int parentId);

        List<UserInfo> GetUserInfos(List<int> userIds);
        
        BaseReturnModel UpdatePassword(SavePasswordParam saveMoneyPasswordParam);
        
        BaseReturnModel UpdatePasswordByOtherWay(SavePasswordByOtherWayParam param);
        
        bool UpdateUserActive(int userId, bool isActive);
        
        BaseReturnDataModel<int> LowMoneyIn(TransferToChildParam param);

        BaseReturnModel SaveUserNickname(int userId, string nickname);

        string GetUserNickname(int userId);

        BaseReturnModel UpdateBlockChainInfo(BlockChainInfoParam param);

        BaseReturnModel SaveUserSecurityInfo(SpSaveUserSecurityInfoParam spSaveUserSecurityInfoParam);

        bool HasUserActiveBankInfo(int userId);
        
        List<ProSelectBankResult> GetUserAllBankCard(int userId);

        int GetUserLevel(int userId);

        List<ActivityLog> GetUserActivityLogByIpInOneDay(string ip);

        bool AddUserActivityLog(ActivityLog model);

        List<SearchUserEmailModel> GetUserNameByEmail(string emailHash);

        BaseReturnModel UrlRegisterUser(UserRegisterParam userRegisterParam);
    }
}
