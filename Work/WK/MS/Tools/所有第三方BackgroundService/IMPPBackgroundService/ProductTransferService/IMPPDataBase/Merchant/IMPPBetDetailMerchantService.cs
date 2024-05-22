using IMPPDataBase.Enums;
using IMPPDataBase.Model;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Old;

namespace IMBGDataBase.Merchant
{
    public interface IIMPPBetDetailService : IBetDetailService<IMPPApiParamModel, BetLogResult<List<BetResult>>>
    { }

    public abstract class IMPPBetDetailMerchantService : BaseBetDetailService<IMPPApiParamModel, BetLogResult<List<BetResult>>>, IIMPPBetDetailService
    {
        protected override PlatformProduct Product => PlatformProduct.IMPP;

        protected override BetLogResult<List<BetResult>> CreateSuccessEmptyResult()
        {
            return new BetLogResult<List<BetResult>>()
            {
                Code = (int)APIErrorCode.Success,
                Result = new List<BetResult>()
            };
        }
    }
}