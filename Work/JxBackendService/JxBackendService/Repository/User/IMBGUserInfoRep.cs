using JxBackendService.Interface.Repository.User;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.User
{
    public class IMBGUserInfoRep : BaseTPGameUserInfoRep<IMBGUserInfo>, IIMBGUserInfoRep
    {
        public IMBGUserInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }           
    }
}
