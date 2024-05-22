using ProductTransferService.LCDataBase.Model;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using ProductTransferService.LCDataBase.Enums;

namespace ProductTransferService.LCDataBase.Merchant
{
    public interface ILCBetDetailService : IBetDetailService<LCApiParamModel, ApiResult<BetResult>>
    {
    }

    public abstract class LCBetDetailMerchantService : BaseBetDetailService<LCApiParamModel, ApiResult<BetResult>>, ILCBetDetailService
    {
        protected override PlatformProduct Product => PlatformProduct.LC;

        protected override ApiResult<BetResult> CreateSuccessEmptyResult()
        {
            return new ApiResult<BetResult>()
            {
                Data = new BetResult()
                {
                    Code = (int)APIErrorCode.Success,
                    BetDetails = new BetDetails()
                }
            };
        }
    }
}