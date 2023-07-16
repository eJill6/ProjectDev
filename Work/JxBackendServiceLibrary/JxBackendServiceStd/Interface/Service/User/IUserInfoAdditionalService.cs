using JxBackendService.Model.Entity.User;
using JxBackendService.Model.ReturnModel;
using System;

namespace JxBackendService.Interface.Service.User
{
    public interface IUserInfoAdditionalService
    {
        UserInfoAdditional GetSingle(int userId);

        BaseReturnModel SaveWithRemoteLock(int userId, Func<UserInfoAdditional, BaseReturnModel> setPropertyValueJob);
    }
}