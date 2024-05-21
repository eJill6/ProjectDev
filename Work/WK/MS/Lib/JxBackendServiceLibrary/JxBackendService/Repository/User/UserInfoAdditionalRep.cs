using Dapper;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System.Collections.Generic;
using System.Data;

namespace JxBackendService.Repository.User
{
    public class UserInfoAdditionalRep : BaseDbRepository<UserInfoAdditional>, IUserInfoAdditionalRep
    {
        public UserInfoAdditionalRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }
    }
}