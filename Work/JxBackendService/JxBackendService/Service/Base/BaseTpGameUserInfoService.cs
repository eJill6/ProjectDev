using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Base
{
    public abstract class BaseTpGameUserInfoService<T> : BaseService, ITPGameUserInfoService where T : BaseTPGameUserInfo
    {
        public BaseTpGameUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public abstract ITPGameUserInfoRep<T> TPGameUserInfoRep { get; }        

        public bool IsUserExists(int userId)
        {
            return TPGameUserInfoRep.IsUserExists(userId);
        }

        public bool CreateUser(int userId, string userName)
        {
            return TPGameUserInfoRep.CreateUser(userId, userName);
        }

        public BaseTPGameUserInfo GetTPGameUserInfo(int userId)
        {
            return TPGameUserInfoRep.GetDetail(userId);
        }
    }
}
