using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace JxBackendService.Repository.Game
{
    public class UserCommissionListRep : BaseDbRepository<UserCommissionList>, IUserCommissionListRep
    {

        public UserCommissionListRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public List<UserCommissionList> GetByProcessMonth(int processMonth)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + "WHERE ProcessMonth = @processMonth ";
            return DbHelper.QueryList<UserCommissionList>(sql, new { processMonth });
        }
    }
}
