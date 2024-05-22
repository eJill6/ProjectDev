using JxBackendService.Model.Enums;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Base;

namespace ProductTransferService
{
    public class ProductTransferScheduleService : BasePGTransferScheduleService
    {
        protected override PlatformProduct Product => PlatformProduct.PGSL;

        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);
    }
}