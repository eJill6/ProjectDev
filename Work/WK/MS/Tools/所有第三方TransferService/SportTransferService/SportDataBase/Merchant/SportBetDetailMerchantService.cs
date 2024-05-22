using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using SportDataBase.Model;

namespace SportDataBase.Merchant
{
    public interface ISportBetDetailService : IBetDetailService<SportApiParamModel, ApiResult<BetResult>>
    {
    }

    public abstract class SportBetDetailMerchantService : BaseBetDetailService<SportApiParamModel, ApiResult<BetResult>>, ISportBetDetailService
    {
        protected override PlatformProduct Product => PlatformProduct.Sport;

        protected override JxApplication Application => JxApplication.SportTransferService;

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