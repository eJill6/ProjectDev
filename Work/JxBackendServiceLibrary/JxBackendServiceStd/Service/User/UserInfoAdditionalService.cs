using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;

namespace JxBackendService.Service.User
{
    public class UserInfoAdditionalService : BaseService, IUserInfoAdditionalService
    {
        private readonly IUserInfoAdditionalRep _userInfoAdditionalRep;

        private readonly IJxCacheService _jxCacheService;

        public UserInfoAdditionalService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _userInfoAdditionalRep = ResolveJxBackendService<IUserInfoAdditionalRep>();
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(EnvLoginUser.Application);
        }

        public UserInfoAdditional GetSingle(int userId)
        {
            return _userInfoAdditionalRep.GetSingleByKey(
                InlodbType.Inlodb, new UserInfoAdditional() { UserID = userId });
        }

        public BaseReturnModel SaveWithRemoteLock(int userId, Func<UserInfoAdditional, BaseReturnModel> setPropertyValueJob)
        {
            return _jxCacheService.DoWorkWithRemoteLock(
                CacheKey.UserInfoAdditionalLock(userId),
                () =>
                {
                    UserInfoAdditional source = GetSingle(userId);

                    ActTypes actType = ActTypes.Update;

                    if (source == null)
                    {
                        actType = ActTypes.Insert;
                        source = new UserInfoAdditional() { UserID = userId };
                    }

                    BaseReturnModel baseReturnModel = setPropertyValueJob.Invoke(source);

                    if (!baseReturnModel.IsSuccess)
                    {
                        return baseReturnModel;
                    }

                    if (actType == ActTypes.Insert)
                    {
                        return _userInfoAdditionalRep.CreateByProcedure(source).CastByJson<BaseReturnModel>();
                    }

                    bool result = _userInfoAdditionalRep.UpdateByProcedure(source);

                    return result
                        ? new BaseReturnModel(ReturnCode.Success)
                        : new BaseReturnModel(ReturnCode.UpdateFailed);
                });
        }
    }
}