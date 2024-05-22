using JxBackendService.Model.Enums;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Base;

namespace ProductTransferService
{
    public class ProductTransferScheduleService : BaseABTransferScheduleService
    {
        protected override PlatformProduct Product => PlatformProduct.ABEB;

        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);
    }
}