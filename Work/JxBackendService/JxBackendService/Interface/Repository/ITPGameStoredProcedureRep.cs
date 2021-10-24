using System;
using System.Collections.Generic;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param;
using JxBackendService.Model.StoredProcedureParam;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Model.Param.ThirdParty;

namespace JxBackendService.Interface.Repository
{
    public interface ITPGameStoredProcedureRep
    {
        TPGameSelfProfitLossSearchResult GetSelfReport(TPGameProfitLossSearchParam searchParam);

        TPGameTeamProfitLossSearchResult GetTeamReport(TPGameTeamProfitLossSearchParam searchParam);

        DateTime GetReportCenterLastModifiedTime();

        PagedResultWithAdditionalData<TeamUserTotalProfitloss, TeamUserTotalProfitlossStat> GetTeamProfitloss(ProGetGameTeamUserTotalProfitLossParam param);

        List<TPGameMoneyInInfo> GetTPGameUnprocessedMoneyInInfo();

        List<TPGameMoneyInInfo> GetTPGameProcessingMoneyInInfo();

        List<TPGameMoneyOutInfo> GetTPGameUnprocessedMoneyOutInfo();

        List<TPGameMoneyOutInfo> GetTPGameProcessingMoneyOutInfo();

        bool DoTransferSuccess(bool isMoneyIn, string moneyInfoId, UserScore userScore);

        bool DoTransferRollback(bool isMoneyIn, string moneyInfoId, string msg);

        bool UpdateMoneyInOrderStatusFromManualToProcessing(string moneyInId);
        
        bool UpdateMoneyOutOrderStatusFromManualToProcessing(string moneyOutId);

        string CreateMoneyInOrder(int userID, decimal amount, string tpGameAccount);
        
        string CreateMoneyOutOrder(int userID, decimal amount, string tpGameAccount);
        
        BaseReturnModel AddProductProfitLossAndPlayInfo(InsertTPGameProfitlossParam tpGameProfitloss);
        
        PlatformTotalProfitlossStat GetPlatformProfitLoss(SearchPlatformProfitLossParam searchParam);

        void CreateLoginHistory(TPGameCreateLoginHistoryParam tpgameCreateLoginHistoryParam);

        List<TPGameProfitLoss> GetTPGameProfitLossDeal(SearchTPGameProfitLossParam searchTPGameProfitLossDealParam);

        PagedResultModel<TPGameMoneyInInfo> GetMoneyInInfoList(SearchTPGameMoneyInfoParam searchParam);
        
        PagedResultModel<TPGameMoneyOutInfo> GetMoneyOutInfoList(SearchTPGameMoneyInfoParam searchParam);
        
        PagedResultWithAdditionalData<TPGamePlayInfoRowModel, TPGamePlayInfoFooter> GetPlayInfoList(SearchTPGamePlayInfoParam searchParam);
        
        TPGamePlayInfoRowModel GetSinglePlayInfo(int userId, string playInfoId);
        
        PagedResultWithAdditionalData<TPGameProfitLossRowModel, ProfitLossStatColumn> GetUserProfitLossDetails(CommonSearchTPGameProfitLossParam searchParam);

        void ConvertProfitlossToColumns(BasicUserProfitlossStat userProfitlossStat, ProfitLossStatColumn profitlossStatColumn);

        string VIPFlowProductTableName { get; }

        string VIPPointsProductTableName { get; }
    }
}