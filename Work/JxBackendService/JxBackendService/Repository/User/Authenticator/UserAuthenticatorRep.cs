using JxBackendService.Interface.Repository;
using JxBackendService.Model.db;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Entity.User.Authenticator;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.User.Authenticator
{
    public class UserAuthenticatorRep : BaseDbRepository<UserAuthenticator>, IUserAuthenticatorRep
    {
        public UserAuthenticatorRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        
    }
}
