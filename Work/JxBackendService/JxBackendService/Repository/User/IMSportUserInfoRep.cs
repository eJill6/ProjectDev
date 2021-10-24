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
    public class IMSportUserInfoRep : BaseTPGameUserInfoRep<IMSportUserInfo>, IIMSportUserInfoRep
    {
        public IMSportUserInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }           
    }
}
