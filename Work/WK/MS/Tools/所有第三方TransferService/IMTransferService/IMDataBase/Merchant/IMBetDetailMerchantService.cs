using IMDataBase.Enums;
using IMDataBase.Model;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using System.Collections.Generic;

namespace IMBGDataBase.Merchant
{
    public interface IIMBetDetailService : IBetDetailService<IMApiParamModel, BetLogResult<List<BetResult>>>
    { }

    public abstract class IMBetDetailMerchantService : BaseBetDetailService<IMApiParamModel, BetLogResult<List<BetResult>>>, IIMBetDetailService
    {
        protected override PlatformProduct Product => PlatformProduct.IM;

        protected override JxApplication Application => JxApplication.IMTransferService;

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