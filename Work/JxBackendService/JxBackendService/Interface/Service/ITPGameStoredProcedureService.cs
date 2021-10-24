using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.StoredProcedureParam;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface ITPGameStoredProcedureService
    {
        PagedResultModel<TPGameMoneyInfoViewModel> GetMoneyInfoList(PlatformProduct product, SearchTransferType searchTransferType, SearchTPGameMoneyInfoParam param);
        
        PlatformTotalProfitlossStat GetPlatformProfitLoss(SearchPlatformProfitLossParam searchParam);
        
        PagedResultModel<PlatformUserProfitloss> GetPlatformUserProfitLosses(CommonSearchTPGameProfitLossParam searchParam);
        
        PagedResultWithAdditionalData<TPGamePlayInfoRowModel, TPGamePlayInfoFooter> GetPlayInfoList(SearchTPGamePlayInfoParam searchParam);
        
        DateTime GetReportCenterLastModifiedTime(PlatformProduct product);
        
        TPGameSelfProfitLossSearchResult GetSelfReport(PlatformProduct product, TPGameProfitLossSearchParam searchParam);
        
        TPGamePlayInfoRowModel GetSinglePlayInfo(PlatformProduct product, int userId, string playInfoId);
        
        PagedResultWithAdditionalData<TeamUserTotalProfitloss, TeamUserTotalProfitlossStat> GetTeamProfitloss(SearchProductProfitlossParam searchParam);
        
        TPGameTeamProfitLossSearchResult GetTeamReport(PlatformProduct product, TPGameTeamProfitLossSearchParam searchParam);
        
        PagedResultWithAdditionalData<TPGameProfitLossRowModel, ProfitLossStatColumn> GetUserProfitLossDetails(SearchTPGameProfitLossParam searchParam);

        bool CheckDateWithinRange(DateTime startDate, DateTime endDate);

        bool CheckDateBetweenRange(DateTime startDate, DateTime endDate);
    }
}
