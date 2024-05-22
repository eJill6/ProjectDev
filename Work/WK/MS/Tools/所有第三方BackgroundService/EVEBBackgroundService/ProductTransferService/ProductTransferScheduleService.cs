using JxBackendService.Model.Enums;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Base;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : BaseEVTransferScheduleService
    {
        protected override PlatformProduct Product => PlatformProduct.EVEB;

        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);
    }
}