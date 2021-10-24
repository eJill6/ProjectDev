using JxBackendService.Interface.Repository.Game;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Entity.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Game
{
    public class UserDailyReportCommissionInfoService : BaseService, IUserDailyReportCommissionInfoService
    {
        private readonly IUserDailyReportCommissionInfoRep _userDailyReportCommissionInfoRep;

        public UserDailyReportCommissionInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _userDailyReportCommissionInfoRep = ResolveJxBackendService<IUserDailyReportCommissionInfoRep>();
        }

        public UserDailyReport_CommissionInfo GetSingle(int userId)
        {
            return _userDailyReportCommissionInfoRep
                .GetSingleByKey(InlodbType.InlodbBak, new UserDailyReport_CommissionInfo() { UserID = userId });
        }

    }
}
