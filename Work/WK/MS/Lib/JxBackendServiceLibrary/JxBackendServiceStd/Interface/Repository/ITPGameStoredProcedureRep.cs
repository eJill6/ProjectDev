using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository
{
    public interface ITPGameStoredProcedureRep
    {
        string ProfitlossTableName { get; }

        TPGameProfitLossRowModelCompareTime ProfitLossCompareTimeProperty { get; }

        List<TPGameMoneyInInfo> GetTPGameUnprocessedMoneyInInfo();

        List<TPGameMoneyInInfo> GetTPGameProcessingMoneyInInfo();

        List<TPGameMoneyOutInfo> GetTPGameUnprocessedMoneyOutInfo();

        List<TPGameMoneyOutInfo> GetTPGameProcessingMoneyOutInfo();

        bool DoTransferSuccess(bool isMoneyIn, BaseTPGameMoneyInfo tpGameMoneyInfo, UserScore userScore);

        bool DoTransferRollback(bool isMoneyIn, BaseTPGameMoneyInfo tpGameMoneyInfo, string msg);

        TPGameTransferMoneyResult CreateMoneyInOrder(int userID, decimal amount, string tpGameAccount, TPGameMoneyInOrderStatus transferInStatus);

        TPGameTransferMoneyResult CreateMoneyOutOrder(CreateTransferOutOrderParam param);

        List<BaseReturnDataModel<string>> AddMultipleProductProfitLossAndPlayInfo(List<InsertTPGameProfitlossParam> tpGameProfitlosses);

        PagedResultModel<TPGameMoneyInInfo> GetMoneyInInfoList(SearchTPGameMoneyInfoParam searchParam);

        PagedResultModel<TPGameMoneyOutInfo> GetMoneyOutInfoList(SearchTPGameMoneyInfoParam searchParam);

        string PlayInfoTableName { get; }

        string MoneyInInfoTableName { get; }

        string MoneyOutInfoTableName { get; }

        List<SqlSelectColumnInfo> ProfitLossSelectColumnInfos { get; }

        List<SqlSelectColumnInfo> PlayInfoSelectColumnInfos { get; }

        TPGameMoneyInInfo GetTPGameMoneyInInfo(string moneyId);

        TPGameMoneyOutInfo GetTPGameMoneyOutInfo(string moneyId);

        List<TPGameMoneyOutInfo> GetTPGameProcessedMoneyOutInfo(DateTime startDate, DateTime endDate, List<int> userIds);
    }
}