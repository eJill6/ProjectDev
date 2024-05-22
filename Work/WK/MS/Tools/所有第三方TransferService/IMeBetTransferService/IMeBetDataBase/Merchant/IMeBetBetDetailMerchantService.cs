using IMeBetDataBase.Enums;
using IMeBetDataBase.Model;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using System.Collections.Generic;

namespace IMBGDataBase.Merchant
{
    public interface IIMeBetBetDetailService : IBetDetailService<IMeBetApiParamModel, BetLogResult<List<BetResult>>>
    { }

    public abstract class IMeBetBetDetailMerchantService : BaseBetDetailService<IMeBetApiParamModel, BetLogResult<List<BetResult>>>, IIMeBetBetDetailService
    {
        protected override PlatformProduct Product => PlatformProduct.IMeBET;

        protected override JxApplication Application => JxApplication.IMeBetTransferService;

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