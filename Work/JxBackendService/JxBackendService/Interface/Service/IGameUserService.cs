using JxBackendService.Model.Enums;
using JxBackendService.Model.StoredProcedureParam;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;

namespace JxBackendService.Interface.Service
{
    public interface IGameUserService
    {
        void CreateLoginHistory(PlatformProduct platformProduct, int type, JxIpInformation ipinformation);
        
        UserReportProfitLossResult GetUserReportProfitloss(RequestUserReportProfitlossParam param);
    }
}