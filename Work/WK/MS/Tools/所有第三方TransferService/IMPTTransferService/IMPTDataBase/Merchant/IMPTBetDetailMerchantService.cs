using IMPTDataBase.Enums;
using IMPTDataBase.Model;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using System.Collections.Generic;

namespace IMBGDataBase.Merchant
{
    public interface IIMPTBetDetailService : IBetDetailService<IMPTApiParamModel, BetLogResult<List<BetResult>>>
    { }

    public abstract class IMPTBetDetailMerchantService : BaseBetDetailService<IMPTApiParamModel, BetLogResult<List<BetResult>>>, IIMPTBetDetailService
    {
        protected override PlatformProduct Product => PlatformProduct.IMPT;

        protected override JxApplication Application => JxApplication.IMPTTransferService;

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