using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Base;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : BaseIMLotteryTransferScheduleService
    {
        public override PlatformProduct Product => PlatformProduct.IMSG;

        public override JxApplication Application => JxApplication.IMSGTransferService;
    }
}