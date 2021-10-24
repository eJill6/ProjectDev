using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.BackSideWeb;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service
{
    public interface IGivePrizesService
    {
        GivePrizeInitData GetGivePrizeInitData(int userId);

        List<JxBackendSelectListItem<bool>> GetPrizeTypeItems(WalletType walletType);

        BaseReturnModel SaveGivePrize(GivePrizesProcessParam saveParam);
        
        LocalizationParam CreateGivePrizeMemoJsonParam(ProfitLossTypeName profitLossTypeName, string memo);
    }
}
