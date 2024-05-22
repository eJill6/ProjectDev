using IMBGDataBase.Enums;
using IMBGDataBase.Model;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Old;

namespace IMBGDataBase.Merchant
{
    public interface IIMBGBetDetailService : IBetDetailService<IMBGApiParamModel, IMBGResp<IMBGBetList<IMBGBetLog>>>
    { }

    public abstract class IMBGBetDetailMerchantService : BaseBetDetailService<IMBGApiParamModel, IMBGResp<IMBGBetList<IMBGBetLog>>>, IIMBGBetDetailService
    {
        protected override PlatformProduct Product => PlatformProduct.IMBG;

        protected override JxApplication Application => JxApplication.IMBGTransferService;

        protected override IMBGResp<IMBGBetList<IMBGBetLog>> CreateSuccessEmptyResult()
        {
            return new IMBGResp<IMBGBetList<IMBGBetLog>>()
            {
                Data = new IMBGBetList<IMBGBetLog>()
                {
                    Code = (int)APIErrorCode.Success
                }
            };
        }
    }
}