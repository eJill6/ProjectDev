using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.Game;
using JxBackendService.Model.Entity.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Game
{
    public class UserDailyReportCommissionInfoRep : BaseDbRepository<UserDailyReport_CommissionInfo>, IUserDailyReportCommissionInfoRep
    {
        public UserDailyReportCommissionInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }
       
    }
}
