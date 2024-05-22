using IMPTDataBase.Enums;
using IMPTDataBase.Model;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Old;

namespace IMBGDataBase.Merchant
{
    public interface IIMPTBetDetailService : IBetDetailService<IMPTApiParamModel, BetLogResult<List<BetResult>>>
    { }

    public abstract class IMPTBetDetailMerchantService : BaseBetDetailService<IMPTApiParamModel, BetLogResult<List<BetResult>>>, IIMPTBetDetailService
    {
        protected override PlatformProduct Product => PlatformProduct.IMPT;

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