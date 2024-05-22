using IMSportDataBase.Enums;
using IMSportDataBase.Model;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using System.Collections.Generic;

namespace IMBGDataBase.Merchant
{
    public interface IIMSportBetDetailService : IBetDetailService<IMSportApiParamModel, BetLogResult<List<BetResult>>>
    { }

    public abstract class IMSportBetDetailMerchantService : BaseBetDetailService<IMSportApiParamModel, BetLogResult<List<BetResult>>>, IIMSportBetDetailService
    {
        protected override PlatformProduct Product => PlatformProduct.IMSport;

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