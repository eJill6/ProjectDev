using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using ProductTransferService.SportDataBase.Model;

namespace ProductTransferService.SportDataBase.Merchant
{
    public interface ISportBetDetailService : IBetDetailService<SportApiParamModel, ApiResult<BetResult>>
    {
    }

    public abstract class SportBetDetailMerchantService : BaseBetDetailService<SportApiParamModel, ApiResult<BetResult>>, ISportBetDetailService
    {
        protected override PlatformProduct Product => PlatformProduct.Sport;

        protected override ApiResult<BetResult> CreateSuccessEmptyResult()
        {
            var apiBetResult = new ApiResult<BetResult>
            {
                Data = new BetResult()
                {
                    last_version_key = "0"
                }
            };

            return apiBetResult;
        }
    }
}