using JxBackendService.Model.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service.Game
{
    public interface IUserDailyReportCommissionInfoService
    {
        UserDailyReport_CommissionInfo GetSingle(int userId);
    }
}
