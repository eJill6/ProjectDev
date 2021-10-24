using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.db;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.User;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace JxBackendService.Repository.User
{
    public class UserLevelRep : BaseDbRepository<UserLevel>, IUserLevelRep
    {
        public UserLevelRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }
    }
}