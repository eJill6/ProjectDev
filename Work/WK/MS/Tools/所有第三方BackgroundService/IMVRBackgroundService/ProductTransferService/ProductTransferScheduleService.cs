using JxBackendService.Model.Enums;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Base;

namespace ProductTransferService
{
    public class ProductTransferScheduleService : BaseIMLotteryTransferScheduleService
    {
        protected override PlatformProduct Product => PlatformProduct.IMVR;

        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);
    }
}