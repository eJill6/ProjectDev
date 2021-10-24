using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository
{
    public interface IUserCommissionStoredProcedureRep
    {
        BaseReturnDataModel<double> GetCommissionPayBySystem(int yearMonth);
        List<SpUserCommissionSelBackendResult> GetUserCommissionForBackSide(int? userId, DateTime startDate);
        List<SpUserCommissionSelFrontSideResult> GetUserCommissionForFrontSide(int userId, string commissionType, DateTime startDate, DateTime endDate, int type);
    }
}
