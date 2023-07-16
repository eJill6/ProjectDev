using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Service.User
{
    public class UserInfoRelatedService : BaseService, IUserInfoRelatedService, IUserInfoRelatedReadService
    {
        private static readonly int s_idleScoreSeconds = 60;

        private readonly IUserInfoRep _userInfoRep;

        private readonly IUserInfoAdditionalService _userInfoAdditionalService;

        public UserInfoRelatedService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _userInfoRep = ResolveJxBackendService<IUserInfoRep>();
            _userInfoAdditionalService = ResolveJxBackendService<IUserInfoAdditionalService>();
        }

        public List<UserInfo> GetIdleScoreUsers()
        {
            return _userInfoRep.GetIdleScoreUsers(DateTime.Now.AddSeconds(-s_idleScoreSeconds), GlobalVariables.MinTransferToMiseAmount);
        }

        public decimal GetUserAvailableScores(int userId)
        {
            UserInfo userInfo = GetUserInfo(userId);

            if (userInfo == null)
            {
                return 0;
            }

            return userInfo.AvailableScores.GetValueOrDefault();
        }

        public UserInfo GetUserInfo(int userId)
        {
            return _userInfoRep.GetSingleByKey(InlodbType.Inlodb, new UserInfo { UserID = userId });
        }

        public UserInfoAdditional GetUserInfoAdditional(int userId)
        {
            return _userInfoAdditionalService.GetSingle(userId);
        }

        public BaseReturnModel UpdateLastAutoTransInfo(int userId, string productCode)
        {
            return _userInfoAdditionalService.SaveWithRemoteLock(userId, (userInfoAdditional) =>
            {
                UserTransferSetting userTransferSetting = userInfoAdditional.GetUserTransferSetting();

                if (userTransferSetting == null)
                {
                    userTransferSetting = new UserTransferSetting();
                }

                userTransferSetting.LastAutoTransProductCode = productCode;
                userInfoAdditional.SetUserTransferSetting(userTransferSetting);

                return new BaseReturnModel(ReturnCode.Success);
            });
        }
    }
}