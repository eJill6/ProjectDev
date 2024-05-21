using JxBackendService.Model.Entity.Game.Lottery;
using JxBackendService.Model.Param.BackSide;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Authenticator;
using JxBackendService.Model.ViewModel.BackSide;
using Management.Web.Modules.SystemSettings.LotteryInfo;
using System.Collections.Generic;

namespace Management.Web.Service
{
    public interface IApiService
    {
        BaseReturnDataModel<ValidateLoginTokenResult> ValidateLoginToken(ValidateLoginTokenRequest request);
        BaseReturnDataModel<QrCodeViewModel> GetQrCode(GetQrCodeRequest request);
        BaseReturnDataModel<List<LotteryInfoResult>> GetLotteryInfoDatas(BackSideModel request);
        BaseReturnDataModel<List<PlayTypeInfo>> GetPlayTypeInfo(BackSideModel request);
        BaseReturnDataModel<string> UpdateLotteryInfo(List<UpdateLotteryInfoRequest> request);
        BaseReturnDataModel<string> UpdateLotteryStatus(BackSideModel request);
        BaseReturnDataModel<string> UpdatePlayTypeStatus(BackSideModel request);
    }
}
