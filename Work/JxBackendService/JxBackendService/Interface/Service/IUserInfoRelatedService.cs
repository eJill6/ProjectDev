using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.BackSideWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    /// <summary>
    /// 後來才開設的ReadService, 原先用到的讀取method有機會在搬        
    /// </summary>
    public interface IUserInfoRelatedReadService
    {
        UserInfo GetUserInfo(int userId);

        UserInfo GetUserInfo(string userName);

        TransferToChildStatus GetUserTransferToChildStatus(int userId);

        UserInfo GetMasterOrSlaveRelationshipUserInfo(int loginUserId, int targetUserId);

        bool IsMoneyPasswordExpired(int userId);

        string GetUserNickname();
        
        BaseReturnModel CheckUserEmail(string email);

        bool HasUserActiveBankInfo();

        string GetMaskUserEmail(int userId);

        string GetUserName(int userId);
        
        int? GetFrontSideUserId(string userName);

        int? GetParentUserId(int userId);
        
        UserLevel GetUserLevel(int userId);
    }

    public interface IUserInfoRelatedService
    {
        BaseReturnModel CheckUserBankHasActive(int userID, ModifyUserDataTypes userDataType);

        bool CheckUserIdInUserPath(int loginUserId, int searchUserID);

        List<int> GetAllFirstChild(int parentId);        

        UserInfo GetUserInfo(string userName);

        UserInfo GetUserInfo(int userId);

        string GetUserModifyContent(int userId, ModifyUserDataTypes userDataType);

        BaseReturnModel ModifyUserDataContent(UserModifyDataParam userModifyDataParam);

        BaseReturnDataModel<string> ValidationUserBank(UserVaildBankParam userVaildBankParam);

        BaseReturnModel SaveChildTransferStatus(int userId, TransferToChildStatus childTransferStatus, string memo);

        List<UserInfo> GetAllFirstChildUserInfo(int parentId, bool isForceRefresh = false);

        BaseReturnModel SavePassword(SavePasswordParam saveMoneyPasswordParam, Action afterSaveLoginPasswordSuccess);

        BaseReturnModel SavePasswordHashByOtherWay(SavePasswordByOtherWayParam param);

        BaseReturnModel SavePasswordByOtherWay(SaveNonHashManualPasswordByOtherWayParam param);

        BaseReturnModel SaveUserNickname(string nickname);

        BaseReturnModel SaveUserSecurityInfo(SaveSecuritySetting securitySetting);

        bool HasUserInitializationComplete(int userId);
        
        bool UpdateUserActive(int userId, bool isActive);
    }
}
