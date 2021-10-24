using JxBackendService.Model.StoredProcedureParam;
using JxBackendService.Model.ViewModel.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository
{
    public interface IUserReportProfitLossRep
    {
        UserReportProfitLossResult GetUserReportProfitloss(ProGetUserReportProfitlossParam param);
    }
}
