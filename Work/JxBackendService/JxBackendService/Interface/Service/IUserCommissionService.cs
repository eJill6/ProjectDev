using JxBackendService.Model.Param.Commission;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface IUserCommissionService
    {
        byte[] GetExportUserCommissionBytes(DateTime queryDate);
        
        BaseReturnDataModel<UserCommissionBackSideViewModel> GetUserCommissionForBackSide(string userName, DateTime startDate);
        
        BaseReturnDataModel<UserSelfCommissionApiResult> GetUserSelfCommissionForApi(CommissionSearchParam commissionSearchParam);
        
        BaseReturnDataModel<List<UserTeamCommissionApiResult>> GetUserTeamCommissionForApi(CommissionSearchParam commissionSearchParam);
        
        List<ContributeDetailViewModel> UserContributeDetailForFrontSide(int userId, DateTime startDate, DateTime endDate);
    }
}
